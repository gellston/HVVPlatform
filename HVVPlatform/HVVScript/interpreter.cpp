#include "interpreter.h"



#include <v8pp/function.hpp>
#include <v8pp/object.hpp>
#include <v8pp/context.hpp>
#include <v8pp/convert.hpp>
#include <v8pp/utility.hpp>
#include <v8pp/ptr_traits.hpp>
#include <v8pp/module.hpp>
#include <v8pp/class.hpp>


#include <libplatform/libplatform.h>



#include <chrono>
#include <map>
#include <memory>
#include <iostream>



#include "pimpl_define.h"
#include "object.h"
#include "converter.h"
#include "exception.h"




using namespace hv;
using namespace v1;

std::shared_ptr<pimpl> interpreter::_platform_instance = std::make_shared<pimpl_v8_platform>();

interpreter::~interpreter() {

	this->_is_thread_running = false;


	// Start lock 해제 
	{
		std::scoped_lock<std::mutex> lock(this->_mtx_event_start_script);
		this->_is_script_running = true;
	}

	this->_event_start_script.notify_one();
	// Start lock 해제 


	if (this->_interpreter_thread.joinable())
		this->_interpreter_thread.join();

}

interpreter::interpreter() : _isolate(std::make_shared<pimpl_v8_isolate>()),
							_global_object_hash(std::make_shared<pimpl_object_hash>()),
							_is_thread_running(true),
							_is_script_running(false),
							_interpreter_thread(&interpreter::_loop, this),
							_is_content(false),
						    _has_error(false),
							_error_message(""),
							_error_start_column(-1),
						    _error_end_column(-1),
							_error_rows(-1){




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

		bool data = hv::v1::convert_to_number(local_variable);
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

		try {
			auto data = hv::v1::convert_to_array(local_variable);
			auto native_array = new hv::v1::array_number(key, data.data(), data.size());
			return native_array;
		}
		catch (std::exception e) {

			return nullptr;
		}
		


		return nullptr;
	}));
}

void hv::v1::interpreter::trace(std::string input)
{

	std::cout << input << std::endl;
}

std::list<std::string> interpreter::global_names() {
	std::scoped_lock lock(this->_mtx_event_global_hash);

	return this->_global_names;
}

std::map<std::string, std::shared_ptr<object>>* interpreter::global_objects() {

	auto objects = extract_pimpl<pimpl_object_hash>(this->_global_object_hash);

	return &(objects->_instance);
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

		v8pp::context parent_context;

		
		auto isolate = extract_pimpl<pimpl_v8_isolate>(this->_isolate);
		auto global_hash = extract_pimpl<pimpl_object_hash>(this->_global_object_hash);

		isolate->_instance = parent_context.isolate();
		v8::HandleScope handleScope1(isolate->_instance);

		// start signal wait;
		this->_script_start_wait();

		if (this->_is_thread_running == false)
			return;

		// exception info clear;
		this->_clear_error_message();

		/// <summary>
		///  Class 함수 등록
		/// </summary>
		typedef v8pp::class_<interpreter> wrap_class_interpreter;
		wrap_class_interpreter interpreter_instance(isolate->_instance);
		interpreter_instance.set("trace", &interpreter::trace);
		v8::Local<v8::Value> val = wrap_class_interpreter::reference_external(isolate->_instance, this);
		isolate->_instance->GetCurrentContext()->Global()->Set(v8pp::to_v8(isolate->_instance, "script"), val);
	

		v8::TryCatch try_catch(isolate->_instance);



		try {
			if (this->_is_content == false) {
				auto result = parent_context.run_file(this->_script_file_path);
			}
			else {
				auto result = parent_context.run_script(this->_script_content);
			}
		}
		catch (std::exception e) {
			this->_set_error_info(e.what(), -1, -1, -1);
		}


		if (try_catch.HasCaught())
		{
			std::string const msg = v8pp::from_v8<std::string>(isolate->_instance, try_catch.Exception()->ToString(isolate->_instance->GetCurrentContext()).ToLocalChecked());
			int lineNumber = try_catch.Message()->GetLineNumber(isolate->_instance->GetCurrentContext()).FromJust();
			int startColumn = try_catch.Message()->GetStartColumn(isolate->_instance->GetCurrentContext()).FromJust();
			int endColumn = try_catch.Message()->GetEndColumn(isolate->_instance->GetCurrentContext()).FromJust();

			this->_set_error_info(msg, startColumn, endColumn, lineNumber);
		}

	
		
		auto globals = isolate->_instance->GetCurrentContext()->Global();
		auto global_names = globals->GetOwnPropertyNames(isolate->_instance->GetCurrentContext()).ToLocalChecked();


		// global hash lock
		{
			std::scoped_lock lock(this->_mtx_event_global_hash);
			global_hash->_instance.clear();
			this->_global_names.clear();

			for (unsigned int i = 0; i < global_names->Length(); i++) {
				auto key_local = global_names->Get(i);
				auto val_local = globals->Get(key_local);

				std::string key = v8pp::from_v8<std::string>(isolate->_instance, key_local->ToString(isolate->_instance));
				std::string type = v8pp::from_v8<std::string>(isolate->_instance, val_local->TypeOf(isolate->_instance));
				if (val_local->IsArray() == true && !val_local.IsEmpty()) type = "array";
				
				if (this->_converter_lambda.find(type) == this->_converter_lambda.end()) continue;

				std::shared_ptr<pimpl_local_var_solid> local_var_solid = std::make_shared<pimpl_local_var_solid>();
				auto local_var = std::static_pointer_cast<pimpl_local_var>(local_var_solid);

				local_var_solid->set(val_local, isolate->_instance, key);

				auto converter = this->_converter_lambda[type];
				object* result = (*converter)(local_var);
				if (result) {
					this->_global_names.push_back(key);
					std::shared_ptr<object> smart_ptr = std::shared_ptr<object>(result);
					global_hash->_instance[key] = smart_ptr;
				}
			}
		}
		

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