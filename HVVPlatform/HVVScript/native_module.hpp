#pragma once


#ifndef HV_NATIVE_MODULE
#define HV_NATIVE_MODULE

#include <string>

namespace hv::v1 {
	struct native_module {
	public:
		std::string file_path;
		void* handle;

		native_module(std::string file_path, void* handle) {
			this->file_path = file_path;
			this->handle = handle;
		}

		native_module() : file_path(""),
			handle(nullptr) {

		}

		native_module(native_module& module) {
			this->file_path = module.file_path;
			this->handle = module.handle;
		}
	};

};

#endif