
// Managed Header
#include <msclr/marshal_cppstd.h>




// Native Header
#include "HVVScriptSharp.h"



HV::V1::Interpreter::Interpreter() {
	_instance = new hv::v1::interpreter_managed();
}

HV::V1::Interpreter::~Interpreter() {
	this->reset();
}

HV::V1::Interpreter::!Interpreter() {
	this->reset();
}


void HV::V1::Interpreter::reset() {
	delete this->_instance;
}

bool HV::V1::Interpreter::SetModulePath(String^ path) {
	auto convert_value = msclr::interop::marshal_as<std::string>(path);
	return this->_instance->set_module_path(convert_value);
}
bool HV::V1::Interpreter::RunScript(String^ content) {
	auto convert_value = msclr::interop::marshal_as<std::string>(content);
	return this->_instance->run_script(convert_value);
}
bool HV::V1::Interpreter::RunFile(String^ path) {
	auto convert_value = msclr::interop::marshal_as<std::string>(path);
	return this->_instance->run_file(convert_value);
}
bool HV::V1::Interpreter::CheckExternalData(String^ key) {
	auto convert_value = msclr::interop::marshal_as<std::string>(key);
	return this->_instance->check_external_data(convert_value);
}
void HV::V1::Interpreter::ClearExternalData() {
	this->_instance->clear_external_data();
}


bool HV::V1::Interpreter::InitV8StartupData(String^ path) {
	auto convert_value = msclr::interop::marshal_as<std::string>(path);
	return hv::v1::interpreter_managed::init_v8_startup_data(convert_value);
}
void HV::V1::Interpreter::InitV8Platform() {
	hv::v1::interpreter_managed::init_v8_platform();
}
bool HV::V1::Interpreter::InitV8Engine() {
	return hv::v1::interpreter_managed::init_v8_engine();
}
void HV::V1::Interpreter::SetV8Flag(String^ flag) {
	auto convert_value = msclr::interop::marshal_as<std::string>(flag);
	hv::v1::interpreter_managed::set_v8_flag(convert_value);
}

List<String^>^ HV::V1::Interpreter::GlobalNames() {
	List<String^>^ list = gcnew List<String^>();
	auto native_list = this->_instance->global_names();
	for (auto& element : native_list) {
		list->Add(gcnew String(element.c_str()));
	}

	return list;
}