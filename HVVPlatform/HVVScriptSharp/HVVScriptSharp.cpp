
// Native Header
#include <msclr/marshal_cppstd.h>
#include <stdexcept>
#include <data_type.h>
#include <exception.h>
#include <native_module.hpp>
#include "secure_macro.h"


// Managed Header
#include "HVVScriptSharp.h"
#include "Exception.h"
#include "Object.h"
#include "Boolean.h"
#include "Number.h"
#include "Image.h"
#include "String.h"
#include "Point.h"


namespace hv::v1 {

	class pimpl_hvvscript_casting_container {
	private:
		std::map<std::string, int> _converter;
	public:
		pimpl_hvvscript_casting_container() {
			register_casting_type(point);
			register_casting_type(image);
			register_casting_type(string);
			register_casting_type(number);
			register_casting_type(boolean);
		}

		std::map<std::string, int>& converter() {
			return _converter;
		}
	};
}


HV::V1::Interpreter::Interpreter() : _instance(new hv::v1::interpreter_managed()),
									_casting_pimpl(new hv::v1::pimpl_hvvscript_casting_container()){
	
	this->EventTraceCallback = gcnew HV::V1::DelegateTrace(this, &HV::V1::Interpreter::Trace);
	this->Handle = GCHandle::Alloc(this->EventTraceCallback);


}

HV::V1::Interpreter::~Interpreter() {
	if (this->Handle.IsAllocated) {
		this->Handle.Free();
	}
	this->EventTraceCallback = nullptr;
	this->HandlePtr = IntPtr::Zero;
	this->_instance->reset_trace_callback();
}

HV::V1::Interpreter::!Interpreter() {
	if (this->Handle.IsAllocated) {
		this->Handle.Free();
	}
	this->EventTraceCallback = nullptr;
	this->HandlePtr = IntPtr::Zero;
	this->_instance->reset_trace_callback();
}


bool HV::V1::Interpreter::SetModulePath(System::String^ path) {
	auto convert_value = msclr::interop::marshal_as<std::string>(path);
	return this->_instance->set_module_path(convert_value);
}
bool HV::V1::Interpreter::RunScript(System::String^ content) {
	auto convert_value = msclr::interop::marshal_as<std::string>(content);
	bool check = false;
	try {
		check = this->_instance->run_script(convert_value);
	}
	catch (hv::v1::script_error error) {
		throw gcnew HV::V1::ScriptError(gcnew System::String(error.what()), error.start_column(), error.end_column(), error.line());
	}

	return check;

}
bool HV::V1::Interpreter::RunFile(System::String^ path) {
	auto convert_value = msclr::interop::marshal_as<std::string>(path);
	bool check = false;
	try {
		check = this->_instance->run_file(convert_value);
	}
	catch (hv::v1::script_error error) {
		throw gcnew HV::V1::ScriptError(gcnew System::String(error.what()), error.start_column(), error.end_column(), error.line());
	}

	return check;
}

bool HV::V1::Interpreter::Terminate() {
	return this->_instance->terminate();
}

bool HV::V1::Interpreter::CheckExternalData(System::String^ key) {
	auto convert_value = msclr::interop::marshal_as<std::string>(key);
	return this->_instance->check_external_data(convert_value);
}
void HV::V1::Interpreter::ClearExternalData() {
	this->_instance->clear_external_data();
}


bool HV::V1::Interpreter::InitV8StartupData(System::String^ path) {
	auto convert_value = msclr::interop::marshal_as<std::string>(path);
	return hv::v1::interpreter_managed::init_v8_startup_data(convert_value);
}
void HV::V1::Interpreter::InitV8Platform() {
	hv::v1::interpreter_managed::init_v8_platform();
}
bool HV::V1::Interpreter::InitV8Engine() {
	return hv::v1::interpreter_managed::init_v8_engine();
}
void HV::V1::Interpreter::SetV8Flag(System::String^ flag) {
	auto convert_value = msclr::interop::marshal_as<std::string>(flag);
	hv::v1::interpreter_managed::set_v8_flag(convert_value);
}

List<String^>^ HV::V1::Interpreter::GlobalNames::get() {
	List<System::String^>^ list = gcnew List<System::String^>();
	auto native_list = this->_instance->global_names();
	for (auto& element : native_list) {
		list->Add(gcnew System::String(element.c_str()));
	}

	return list;
}

Dictionary<System::String^, HV::V1::Object^>^ HV::V1::Interpreter::GlobalObjects::get() {
	auto global_object = gcnew Dictionary<System::String^, HV::V1::Object^>();


	auto globals = this->_instance->global_objects();
	auto casting = this->_casting_pimpl->converter();

	for (auto& [key, val] : globals) {
		switch (casting[val->type()]) {

		case hv::v1::casting::number:
			global_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Number(val));
			break;
		case hv::v1::casting::image:
			global_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Image(val));
			break;
		case hv::v1::casting::boolean:
			global_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Boolean(val));
			break;
		case hv::v1::casting::string:
			global_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::String(val));
			break;
		case hv::v1::casting::point:
			global_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Point(val));
			break;
		default:
			global_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Object(val));
			break;
		}
	}
	return global_object;
}

List<System::String^>^ HV::V1::Interpreter::ExternalNames::get() {
	List<System::String^>^ list = gcnew List<System::String^>();
	auto native_list = this->_instance->external_names();
	for (auto& element : native_list) {
		list->Add(gcnew System::String(element.c_str()));
	}
	return list;
}
Dictionary<System::String^, HV::V1::Object^>^ HV::V1::Interpreter::ExternalObjects::get() {
	auto external_object = gcnew Dictionary<System::String^, HV::V1::Object^>();

	auto externals = this->_instance->external_objects();
	auto casting = this->_casting_pimpl->converter();

	for (auto& [key, val] : externals) {
		
		
		switch (casting[val->type()]) {

		case hv::v1::casting::number:
			external_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Number(val));
			break;
		case hv::v1::casting::image:
			external_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Image(val));
			break;
		case hv::v1::casting::boolean:
			external_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Boolean(val));
			break;
		case hv::v1::casting::string:
			external_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::String(val));
			break;
		case hv::v1::casting::point:
			external_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Point(val));
			break;
		default:
			external_object->Add(gcnew System::String(key.c_str()),  gcnew HV::V1::Object(val));
			break;
		}
	}

	return external_object;
}

Dictionary<System::String^, HV::V1::NativeModule^>^ HV::V1::Interpreter::NativeModules::get(){
	Dictionary<System::String^, HV::V1::NativeModule^> ^ dictionary = gcnew Dictionary<System::String^, HV::V1::NativeModule^>();
	auto native_modules = this->_instance->native_modules();


	for (auto& [key, val] : *native_modules) {
		dictionary->Add(gcnew System::String(key.c_str()), gcnew HV::V1::NativeModule(gcnew System::String(val.file_path.c_str()), System::IntPtr(val.handle)));
	}

	return dictionary;
}

bool HV::V1::Interpreter::RegisterExternalData(System::String^ key, HV::V1::Object^ data) {

	auto native_key =  msclr::interop::marshal_as<std::string>(key);

	this->_instance->register_external_data(native_key, data->_instance.get());


	return true;
}
System::Object^ HV::V1::Interpreter::ExternalData(System::String^ key) {

	auto native_key = msclr::interop::marshal_as<std::string>(key);
	auto shared_native_pointer = this->_instance->external_data(native_key);
	auto managed_object = gcnew HV::V1::Object(shared_native_pointer);
	
	return managed_object;
}

void HV::V1::Interpreter::Trace(System::String^ data) {

}
