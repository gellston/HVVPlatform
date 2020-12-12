#pragma once


#ifndef HV_OBJECT
#define HV_OBJECT


#include "macro.h"
#include <string>

namespace hv::v1 {


	class object;
	class HVAPI_EXPORT object {
	private:
		std::string _name;
		std::string _type;
		object() = delete;
	public:

		virtual ~object();
		object(std::string name, std::string type);
		std::string name();
		std::string type();
		virtual std::string to_string() = 0;

	};

	
}


#endif