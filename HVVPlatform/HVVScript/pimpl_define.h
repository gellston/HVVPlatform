#pragma once


#ifndef HV_PIMPL_DEFINE
#define HV_PIMPL_DEFINE

#include <string>
#include <map>
#include <functional>
#include <memory>

#include <v8pp/function.hpp>
#include <v8pp/object.hpp>
#include <v8pp/context.hpp>
#include <v8pp/convert.hpp>
#include <v8pp/utility.hpp>
#include <v8pp/ptr_traits.hpp>


#include <libplatform/libplatform.h>

#include "pimpl.h"
#include "primitive_object.h"


namespace hv::v1 {

	template <typename TYPE> std::shared_ptr<TYPE> extract_pimpl(std::shared_ptr<pimpl> & pointer) {
		std::shared_ptr<TYPE> converted = std::static_pointer_cast<TYPE>(pointer);
		return converted;
	}

	//class pimpl_object_hash;
	//class pimpl_v8_isolate;
	//class pimpl_v8_platform;
	//class pimpl_converter_lambda;
	//class pimpl_local_var_implement;

	class pimpl_object_hash : public pimpl{
	public:
		pimpl_object_hash() {}
		~pimpl_object_hash(){}
		std::map<std::string, std::shared_ptr<object>> _instance;
	};

	class pimpl_v8_isolate : public pimpl {
	public:
		pimpl_v8_isolate() : _instance(nullptr) {  }
		~pimpl_v8_isolate() {}
		v8::Isolate* _instance;
	};

	class pimpl_v8_platform : public pimpl {

	public:
		pimpl_v8_platform() : _instance(nullptr) { }
		~pimpl_v8_platform() {}
		std::unique_ptr<v8::Platform> _instance;
	};

	class pimpl_local_var_solid : public pimpl_local_var {
	public:
		pimpl_local_var_solid() : _isolate(nullptr) {}
		~pimpl_local_var_solid() {}
		v8::Local<v8::Value> _local;
		v8::Isolate* _isolate;
		std::string _key;

		//virtual std::shared_ptr<object> data() override {
		//	
		//

		//	if (_isolate == nullptr) return nullptr;

		//	if (_local->IsBoolean()) {
		//		bool v8_data = v8pp::from_v8<bool>(_isolate, _local->ToBoolean(_isolate));
		//		std::shared_ptr<object> data((object*) new boolean(this->_key, v8_data));
		//		return data;
		//	}
		//	else if (_local->IsNumber()) {
		//		double v8_data = v8pp::from_v8<double>(_isolate, _local->ToNumber(_isolate));
		//		std::shared_ptr<object> data((object*) new number(this->_key, v8_data));
		//		return data;
		//	}
		//	else if (_local->IsString()) {
		//		std::string v8_data = v8pp::from_v8<std::string>(_isolate, _local->ToString(_isolate));
		//		std::shared_ptr<object> data((object*) new string(this->_key, v8_data));
		//		return data;
		//	}
		//	else {
		//		return nullptr;
		//	}
		//}


		void set(v8::Local<v8::Value> local, v8::Isolate * isolate, std::string key) {
			this->_local = local;
			this->_isolate = isolate;
			this->_key = key;
		}

		std::string key() override {
			return this->_key;
		}
	};

}


#endif