
// Native Header
#include <msclr/marshal_cppstd.h>
#include <stdexcept>
#include <data_type.h>
#include <exception.h>
#include <native_module.hpp>


// Managed Header
#include "HVVScriptSharp.h"
#include "Exception.h"
#include "Boolean.h"
#include "Number.h"
#include "Image.h"
#include "String.h"
#include "Point.h"



using namespace System::Reflection;
using namespace System::Diagnostics;



HV::V1::Interpreter::Interpreter() : _instance(new hv::v1::interpreter_managed()){
	
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
	for (auto element : native_list) {
		list->Add(gcnew System::String(element.c_str()));
	}

	return list;
}

Dictionary<System::String^, HV::V1::Object^>^ HV::V1::Interpreter::GlobalObjects::get() {
	auto global_object = gcnew Dictionary<System::String^, HV::V1::Object^>();
	auto globals = this->_instance->global_objects();


	std::map<std::string, std::shared_ptr<hv::v1::object>>::iterator it;

	for (it = globals->begin(); it != globals->end(); it++)
	{
		std::string key = it->first;
		auto native_object = it->second;

		if (native_object->type().length() <= 0) continue;
		
		std::string upperTypeName = native_object->type();
		upperTypeName[0] = toupper(upperTypeName[0]);
		
		std::string namespaceName = "HV.V1.";
		std::string fullNamespaceName = namespaceName + upperTypeName;

		try {
			System::String^ managedTypeName = gcnew System::String(fullNamespaceName.c_str());

			Type^ object_type = Type::GetType(managedTypeName);

			HV::V1::Object^ managedObject = gcnew HV::V1::Object(native_object);
			HV::V1::Object^ createdObject = (HV::V1::Object^)Activator::CreateInstance(object_type, managedObject);
			global_object->Add(gcnew System::String(key.c_str()), createdObject);
		}
		catch (Exception^ e) {
			Debug::WriteLine(e->Message);
			HV::V1::Object^ managedObject = gcnew HV::V1::Object(native_object);
			global_object->Add(gcnew System::String(key.c_str()), managedObject);
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

	std::map<std::string, std::shared_ptr<hv::v1::object>>::iterator it;

	for (it = externals->begin(); it != externals->end(); it++)
	{
		std::string key = it->first;
		auto native_object = it->second;

		if (native_object->type().length() <= 0) continue;

		std::string upperTypeName = native_object->type();
		upperTypeName[0] = toupper(upperTypeName[0]);

		std::string namespaceName = "HV.V1.";
		std::string fullNamespaceName = namespaceName + upperTypeName;

		try {
			System::String^ managedTypeName = gcnew System::String(fullNamespaceName.c_str());

			Type^ object_type = Type::GetType(managedTypeName);

			HV::V1::Object^ managedObject = gcnew HV::V1::Object(native_object);
			HV::V1::Object^ createdObject = (HV::V1::Object^)Activator::CreateInstance(object_type, managedObject);
			external_object->Add(gcnew System::String(key.c_str()), createdObject);
		}
		catch (Exception^ e) {
			Debug::WriteLine(e->Message);
			HV::V1::Object^ managedObject = gcnew HV::V1::Object(native_object);
			external_object->Add(gcnew System::String(key.c_str()), managedObject);
		}

	}

	return external_object;
}

Dictionary<System::String^, HV::V1::NativeModule^>^ HV::V1::Interpreter::NativeModules::get(){
	Dictionary<System::String^, HV::V1::NativeModule^> ^ dictionary = gcnew Dictionary<System::String^, HV::V1::NativeModule^>();

	std::map<std::string, hv::v1::native_module>::iterator it;
	auto native_modules = this->_instance->native_modules();

	for (it = native_modules->begin(); it != native_modules->end(); it++)
	{
		std::string key = it->first;
		auto native_moudle = it->second;
		dictionary->Add(gcnew System::String(key.c_str()), gcnew HV::V1::NativeModule(gcnew System::String(native_moudle.file_path().c_str()), System::IntPtr(native_moudle.handle())));
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


void HV::V1::Interpreter::ReleaseNativeModules() {
	this->_instance->release_native_modules();
}