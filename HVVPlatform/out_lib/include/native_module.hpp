#pragma once


#ifndef HV_NATIVE_MODULE
#define HV_NATIVE_MODULE

#include <string>

namespace hv::v1 {
	struct native_module {
	public:
		std::string _file_path;
		void* _handle;

		native_module(std::string _file_path, void* _handle) {
			this->_file_path = _file_path;
			this->_handle = _handle;
		}

		native_module() : _file_path(""),
			_handle(nullptr) {

		}

		native_module(native_module& module) {
			this->_file_path = module._file_path;
			this->_handle = module._handle;
		}

		std::string file_path(){
			return this->_file_path;
		}

		void* handle() {
			return this->_handle;
		}
	};

};

#endif