#include "interpreter.h"


#include <v8.h>
#include <v8pp/function.hpp>
#include <v8pp/object.hpp>
#include <v8pp/context.hpp>
#include <v8pp/convert.hpp>
#include <v8pp/utility.hpp>
#include <v8pp/ptr_traits.hpp>
#include <v8pp/module.hpp>
#include <v8pp/class.hpp>


#include <libplatform/libplatform.h>
#include <windows.h>


#include <chrono>
#include <map>
#include <memory>




#include "pimpl_define.h"
#include "object.h"
#include "converter.h"
#include "exception.h"
#include "string_cvt.h"
#include "binding.h"


HV_CREATE_SHARED_CONVERTER(hv::v1::object);



using namespace hv;
using namespace v1;

std::shared_ptr<pimpl> interpreter::_platform_instance = std::make_shared<pimpl_v8_platform>();


interpreter::~interpreter() {

	this->_is_thread_running = false;

	this->_script_start_signal();

	this->_interpreter_thread.join();

	// global native object deletion
	for (auto& element : *this->_native_modules) {
		if (element.second.handle == nullptr) continue;
		::FreeLibrary(static_cast<HMODULE>(element.second.handle));
	}

}

interpreter::interpreter() : _isolate(std::make_shared<pimpl_v8_isolate>()),
							_global_object_hash(std::make_shared<pimpl_object_hash>()),
							_native_modules(std::make_shared<std::map<std::string, hv::v1::native_module>>()),
							_is_thread_running(true),
							_is_script_running(false),
							_interpreter_thread(),
							_is_content(false),
						    _has_error(false),
							_error_message(""),
							_error_start_column(-1),
						    _error_end_column(-1),
							_error_rows(-1),
							_script_module_path(""),
							_is_terminating(false){

	_trace_callback = [&](std::string data) {
		// empty callback

	};
	_interpreter_thread = std::move(std::thread(&interpreter::_loop, this));

	/// <summary>
	/// Converter Lambda defionition
	/// </summary>
	this->register_converter("boolean", new converter([&](std::shared_ptr<pimpl_local_var> local_variable) -> object* {
		std::string key = local_variable->key();

		if (hv::v1::is_boolean(local_variable) == false) return nullptr;

		bool data = hv::v1::convert_to_boolean(local_variable);
		auto native_bool = new boolean(key, data);

		return native_bool;
	}));

	this->register_converter("number", new converter([&](std::shared_ptr<pimpl_local_var> local_variable) -> object* {
		std::string key = local_variable->key();

		if (hv::v1::is_number(local_variable) == false) return nullptr;

		double data = hv::v1::convert_to_number(local_variable);
		auto native_number = new number(key, data);

		return native_number;
	}));

	this->register_converter("string", new converter([&](std::shared_ptr<pimpl_local_var> local_variable) -> object* {
		std::string key = local_variable->key();

		if (hv::v1::is_string(local_variable) == false) return nullptr;

		std::string data = hv::v1::convert_to_string(local_variable);
		auto native_string = new string(key, data);

		return native_string;
	}));

	this->register_converter("array", new converter([&](std::shared_ptr<pimpl_local_var> local_variable) -> object* {
		std::string key = local_variable->key();

		if (hv::v1::is_array(local_variable) == false) return nullptr;

		// Array
		try {
			auto data = hv::v1::convert_to_array<std::string>(local_variable);
			auto array = new hv::v1::array<std::string>(key, data);
			return array;
		}
		catch (std::exception e) {
		}

		try {
			auto data = hv::v1::convert_to_array<double>(local_variable);
			auto array = new hv::v1::array<double>(key, data);
			return array;
		}
		catch (std::exception e) {
		}

		try {
			auto data = hv::v1::convert_to_array<bool>(local_variable);
			auto array = new hv::v1::array<bool>(key, data);
			return array;
		}
		catch (std::exception e) {
		}


		return nullptr;
	}));

	this->register_converter("map", new converter([&](std::shared_ptr<pimpl_local_var> local_variable) -> object* {
		std::string key = local_variable->key();

		if (hv::v1::is_map(local_variable) == false) return nullptr;

		// Map
		try {
			auto data = hv::v1::convert_to_map<double>(local_variable);
			auto map = new hv::v1::map<double>(key, data);
			return map;
		}
		catch (std::exception e) {
		}

		try {
			auto data = hv::v1::convert_to_map<std::string>(local_variable);
			auto map = new hv::v1::map<std::string>(key, data);
			return map;
		}
		catch (std::exception e) {
		}

		try {
			auto data = hv::v1::convert_to_map<bool>(local_variable);
			auto map = new hv::v1::map<bool>(key, data);
			return map;
		}
		catch (std::exception e) {
		}
		return nullptr;
	}));

	
	this->register_converter("object", new converter([&](std::shared_ptr<pimpl_local_var> local_variable) -> object* {
		std::string key = local_variable->key();

		// Map
		try {
			auto global_hash = extract_pimpl<pimpl_object_hash>(this->_global_object_hash);
			auto data = hv::v1::convert_to_object<object>(local_variable);
			if (data != nullptr) 
				global_hash->_instance[key] = data;
		}
		catch (std::exception e) {
		}

		return nullptr;
	}));
}

void hv::v1::interpreter::trace(std::string input)
{
	this->_trace_callback(input);
	std::cout << input << std::endl;
}

std::list<std::string> interpreter::global_names() {
	std::scoped_lock lock(this->_mtx_event_global_hash);

	return this->_global_names;
}

std::map<std::string, std::shared_ptr<object>> interpreter::global_objects() {
	std::scoped_lock lock(this->_mtx_event_global_hash);

	auto objects = extract_pimpl<pimpl_object_hash>(this->_global_object_hash);

	return objects->_instance;
}


void interpreter::_script_start_signal() {
	{
		std::scoped_lock<std::mutex> lock(this->_mtx_event_start_script);
		this->_is_script_running = true;
	}
	this->_event_start_script.notify_one();
}

void interpreter::_script_start_wait() {
	std::unique_lock<std::mutex> lock(this->_mtx_event_start_script);
	this->_event_start_script.wait(lock, [&]() ->bool {
		return this->_is_script_running;
	});
}

void interpreter::_script_end_signal() {
	{
		std::scoped_lock<std::mutex> lock(this->_mtx_event_end_script);
		this->_is_script_running = false;
	}
	this->_event_end_script.notify_one();
}

void interpreter::_script_end_wait() {
	std::unique_lock<std::mutex> lock(this->_mtx_event_start_script);
	this->_event_end_script.wait(lock, [&]()->bool {
		return !this->_is_script_running;
	});
}

void interpreter::_clear_error_message() {
	this->_has_error = false;
	this->_error_message = "";
}
void interpreter::_set_error_info(std::string message, int start_col, int end_col, int row) {
	this->_has_error = true;
	this->_error_message = message;
	this->_error_start_column = start_col;
	this->_error_end_column = end_col;
	this->_error_rows = row;
}


void interpreter::_loop() {

	while (this->_is_thread_running) {


		auto isolate = extract_pimpl<pimpl_v8_isolate>(this->_isolate);
		auto global_hash = extract_pimpl<pimpl_object_hash>(this->_global_object_hash);
		



		v8pp::context parent_context(nullptr,nullptr,true,this->_native_modules);
		isolate->_instance = parent_context.isolate();

		v8::HandleScope handleScope(isolate->_instance);

	
		/// <summary>
		/// Object Type register
		/// </summary>
		hv::v1::class_<hv::v1::object,hv::v1::shared_ptr_traits> object_class(isolate->_instance);
		object_class.ctor<std::string, std::string>()
			.auto_wrap_objects(true)
			.set("to_string", &hv::v1::object::to_string)
			.set("name", &hv::v1::object::name)
			.set("type", &hv::v1::object::type);

		v8pp::module object_module(isolate->_instance);
		object_module.set("object", object_class);
		isolate->_instance->GetCurrentContext()->Global()->Set(v8pp::to_v8(isolate->_instance, "internal"), object_module.new_instance());


		/// <summary>
		///  Class 함수 등록
		/// </summary>
		/// 
		typedef v8pp::class_<interpreter> wrap_class_interpreter;
		wrap_class_interpreter interpreter_instance(isolate->_instance);
		interpreter_instance.set("trace", &interpreter::trace);
		interpreter_instance.set("check_external_object", &interpreter::check_external_object);
		interpreter_instance.set("external_object", &interpreter::external_object);
		
		auto val = wrap_class_interpreter::reference_external(isolate->_instance, this);
		auto key = v8pp::to_v8(isolate->_instance, "script");
		isolate->_instance->GetCurrentContext()->Global()->Set(key, val);
	


		// 오브젝트 등록을 위해 여기서 멈춰있어야함. 
		// start signal wait;
		this->_script_start_wait();

		if (this->_is_thread_running == false)
			return;

		// exception info clear;
		this->_clear_error_message();



		// Try catch object 생성;
		v8::TryCatch try_catch(isolate->_instance);
		
		try {
			if (this->_is_content == false) {
				this->_is_terminating = true;
				auto script_result = parent_context.run_file(this->_script_file_path);
				this->_is_terminating = false;
			}
			else {
				this->_is_terminating = true;
				auto script_result = parent_context.run_script(this->_script_content);
				this->_is_terminating = false;
			}
		}
		catch (std::exception e) {
			this->_set_error_info(e.what(), -1, -1, -1);
		}
		

		/// <summary>
		/// Context after run script
		/// </summary>
		auto currentContext = isolate->_instance->GetCurrentContext();


		if (try_catch.HasCaught())
		{
			std::string const msg = v8pp::from_v8<std::string>(isolate->_instance, try_catch.Exception()->ToString(currentContext).ToLocalChecked());
			int lineNumber = try_catch.Message()->GetLineNumber(currentContext).FromJust();
			int startColumn = try_catch.Message()->GetStartColumn(currentContext).FromJust();
			int endColumn = try_catch.Message()->GetEndColumn(currentContext).FromJust();

			this->_set_error_info(msg, startColumn, endColumn, lineNumber);
		}



		// global hash lock
		{
			std::scoped_lock lock(this->_mtx_event_global_hash);

			auto globals = currentContext->Global();
			auto global_names = globals->GetOwnPropertyNames(currentContext).ToLocalChecked();

			global_hash->_instance.clear();
			this->_global_names.clear();

			for (unsigned int i = 0; i < global_names->Length(); i++) {

				auto key_local = global_names->Get(i);
				auto val_local = globals->Get(key_local);

				// C++20에 추가될 u8string이 추가될 경우 변경되어야됨.

				auto v8KeyValue = key_local->ToString(isolate->_instance);
				auto v8TypeValue = val_local->TypeOf(isolate->_instance);

				std::string u8_key = v8pp::from_v8<std::string>(isolate->_instance, v8KeyValue);
				std::string u8_type = v8pp::from_v8<std::string>(isolate->_instance, v8TypeValue);

				std::string key = u8string_to_string(u8_key);
				std::string type = u8string_to_string(u8_type);

				if (val_local->IsNullOrUndefined() || val_local->IsFunction() == true || val_local->IsDate())
					continue;

				

				if (val_local->IsArray() == true && !val_local.IsEmpty() && val_local->IsObject()) type = "array";
				else if (val_local->IsMap() == true && !val_local.IsEmpty() && val_local->IsObject()) type = "map";
				else if (val_local->IsObject()) type = "object";


				if (this->_converter_lambda.find(type) == this->_converter_lambda.end()) continue;

				std::shared_ptr<pimpl_local_var_solid> local_var_solid = std::make_shared<pimpl_local_var_solid>();
				auto local_var = std::static_pointer_cast<pimpl_local_var>(local_var_solid);

				local_var_solid->set(val_local, isolate->_instance, key);
				
				auto converter = this->_converter_lambda[type];
				object* result = (*converter)(local_var);
				if (result != nullptr) {
					this->_global_names.push_back(key);
					global_hash->_instance[key] = std::shared_ptr<object>(result);
				}

			}
		}
		/// <summary>
		/// Clear v8pp context explicitly 
		/// </summary>
		/// 
		// isolate->_instance->RequestGarbageCollectionForTesting(v8::Isolate::GarbageCollectionType::kFullGarbageCollection);

		//v8pp::cleanup(isolate->_instance);
		isolate->_instance = nullptr;
		//std::string const v8_flags = "--expose_gc";
		//v8::V8::SetFlagsFromString(v8_flags.data(), (int)v8_flags.length());
		//isolate->_instance->RequestGarbageCollectionForTesting(
		//v8::Isolate::GarbageCollectionType::kFullGarbageCollection);
		/// <summary>
		/// Script end signal
		/// </summary>

		this->_script_end_signal();
	}
}

bool interpreter::register_converter(std::string type , converter* _converter) {
	if (this->_converter_lambda.find(type) != this->_converter_lambda.end()) return false;

	this->_converter_lambda[type] = std::shared_ptr<converter>(_converter);

	return true;
}

bool interpreter::set_module_path(std::string path) {
	if (this->_is_script_running == true) return false;
	this->_script_module_path = path;

	return true;
}

bool interpreter::run_script(std::string content) {

	/// <summary>
	/// Break script freezing
	/// </summary>
	if (this->_is_script_running == true) return false;

	this->_script_content = content;
	this->_is_content = true;


	this->_script_start_signal();


	/// <summary>
	/// Script end check
	/// </summary>
	this->_script_end_wait();

	if (this->_has_error == true)
		throw hv::v1::script_error(this->_error_message, this->_error_start_column, this->_error_end_column, this->_error_rows);

	return true;
}
bool interpreter::run_file(std::string path) {
	

	/// <summary>
	/// Break script freezing
	/// </summary>
	if (this->_is_script_running == true) return false;

	this->_script_file_path = path;
	this->_is_content = false;


	this->_script_start_signal();


	/// <summary>
	/// Script end check
	/// </summary>
	this->_script_end_wait();

	if (this->_has_error == true)
		throw hv::v1::script_error(this->_error_message, this->_error_start_column, this->_error_end_column, this->_error_rows);

	return true;
}


bool interpreter::register_external_object(std::string key, std::shared_ptr<object> data) {
	std::scoped_lock lock(this->_mtx_event_external_hash);

	this->_external_hash_map[key] = data;

	return true;
}

std::shared_ptr<object> interpreter::external_object(std::string key) {
	std::scoped_lock lock(this->_mtx_event_external_hash);

	if (this->_external_hash_map.find(key) == this->_external_hash_map.end()) {
		std::shared_ptr<object> null_pointer(nullptr);
		return null_pointer;
	}

	auto shared_pointer = this->_external_hash_map[key];

	return shared_pointer;
}

std::list<std::string> interpreter::external_names() {
	std::scoped_lock lock(this->_mtx_event_external_hash);
	std::list<std::string> name_list;

	for (auto& pair : this->_external_hash_map) {
		name_list.push_back(pair.first);
	}

	return name_list;
}
std::map<std::string, std::shared_ptr<object>> interpreter::external_objects() {
	std::scoped_lock lock(this->_mtx_event_external_hash);

	return this->_external_hash_map;
}



bool interpreter::check_external_object(std::string key) {
	std::scoped_lock lock(this->_mtx_event_external_hash);

	if (this->_external_hash_map.find(key) == this->_external_hash_map.end()) return false;


	return true;
}

void interpreter::clear_external_object() {
	std::scoped_lock lock(this->_mtx_event_external_hash);

	this->_external_hash_map.clear();
}


bool interpreter::terminate() {
	
	auto isolate = extract_pimpl<pimpl_v8_isolate>(this->_isolate);
	if (isolate == nullptr) return false;
	if (this->_is_terminating == false) return false;
	v8::Locker locker(isolate->_instance);
	isolate->_instance->TerminateExecution();

	return true;

}

void interpreter::release_native_modules() {
	// global native object deletion
	for (auto& element : *this->_native_modules) {
		if (element.second.handle == nullptr) continue;
		::FreeLibrary(static_cast<HMODULE>(element.second.handle));
	}
}


std::function<void(std::string)>& interpreter::set_trace_callback() {
	return this->_trace_callback;
}

void interpreter::reset_trace_callback() {
	this->_trace_callback = [&](std::string data) {
		//empty callback
	};
}


bool interpreter::init_v8_startup_data(std::string path) {

	if (path.length() == 0) return false;

	v8::V8::InitializeExternalStartupData(static_cast<const char*>(path.c_str()));

	return true;;
}

void interpreter::init_v8_platform() {

	auto pimpl_platform = extract_pimpl<pimpl_v8_platform>(interpreter::_platform_instance);

	if (pimpl_platform->_instance == nullptr) {
		auto platform = v8::platform::NewDefaultPlatform();
		pimpl_platform->_instance.swap(platform);
	}
	v8::V8::InitializePlatform(pimpl_platform->_instance.get());
}

bool interpreter::init_v8_engine() {
	
	return v8::V8::Initialize();
}


void interpreter::set_v8_flag(std::string flag) {

	int size = static_cast<int>(flag.length());

	v8::V8::SetFlagsFromString(flag.c_str(), size);
}



