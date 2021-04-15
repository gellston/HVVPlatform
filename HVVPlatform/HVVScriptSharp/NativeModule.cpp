#include "NativeModule.h"

HV::V1::NativeModule::NativeModule(System::String^ fileName, System::IntPtr handle) {
	this->_file_path = fileName;
	this->_handle = handle;
}

System::String^ HV::V1::NativeModule::FilePath::get() {
	return this->_file_path;
}


System::IntPtr HV::V1::NativeModule::Handle::get() {
	return this->_handle;
}