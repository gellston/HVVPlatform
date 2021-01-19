
// Native Header
#include <msclr/marshal_cppstd.h>
#include <stdexcept>
#include <data_type.h>


// Managed Header
#include "HVVScriptSharp.h"
#include "Boolean.h"
#include "Number.h"
#include "Image.h"
#include "String.h"
#include "Point.h"

namespace hv::v1 {

	class pimpl_casting_container {
	private:
		std::map<std::string, int> _converter;
	public:
		pimpl_casting_container() {
			_converter["object"] = hv::v1::casting::casting_type::object;
			_converter["number"] = hv::v1::casting::casting_type::number;
			_converter["string"] = hv::v1::casting::casting_type::string;
			_converter["boolean"] = hv::v1::casting::casting_type::boolean;
			_converter["image"] = hv::v1::casting::casting_type::image;
			_converter["point"] = hv::v1::casting::casting_type::point;
		}

		std::map<std::string, int> &  converter() {
			return _converter;
		}
	};
}


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
	catch (std::runtime_error error) {
		throw gcnew System::Exception(gcnew System::String(error.what()));
	}

	return check;

}
bool HV::V1::Interpreter::RunFile(System::String^ path) {
	auto convert_value = msclr::interop::marshal_as<std::string>(path);
	bool check = false;
	try {
		check = this->_instance->run_file(convert_value);
	}
	catch (std::runtime_error error) {
		throw gcnew System::Exception(gcnew System::String(error.what()));
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

Dictionary<System::String^, System::Object^>^ HV::V1::Interpreter::GlobalObjects::get() {
	auto global_object = gcnew Dictionary<System::String^, System::Object^>();


	auto globals = this->_instance->global_objects();
	for (auto& [key, val] : globals) {
		auto managed_object = gcnew HV::V1::Object(val);
		global_object->Add(gcnew System::String(key.c_str()), managed_object);
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
Dictionary<System::String^, System::Object^>^ HV::V1::Interpreter::ExternalObjects::get() {
	auto external_object = gcnew Dictionary<System::String^, System::Object^>();

	auto externals = this->_instance->external_objects();
	auto casting = this->_casting_pimpl->converter();

	for (auto& [key, val] : externals) {
		auto managed_object = gcnew HV::V1::Object(val);
		
		switch (casting[val->type()]) {
		case hv::v1::casting::object:
			external_object->Add(gcnew System::String(key.c_str()), managed_object);
			break;
		case hv::v1::casting::number:
			external_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Number(managed_object));
			break;
		case hv::v1::casting::image:
			external_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Image(managed_object));
			break;
		case hv::v1::casting::boolean:
			external_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Boolean(managed_object));
			break;
		case hv::v1::casting::string:
			external_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::String(managed_object));
			break;
		case hv::v1::casting::point:
			external_object->Add(gcnew System::String(key.c_str()), gcnew HV::V1::Point(managed_object));
			break;
		default:
			external_object->Add(gcnew System::String(key.c_str()), managed_object);
			break;
		}
	}

	return external_object;
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
