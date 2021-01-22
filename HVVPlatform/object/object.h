#pragma once


#include "macro.h"

#ifndef HV_OBJECT
#define HV_OBJECT



#include <string>

namespace hv::v1 {


	class object;
	class object {
	private:
		std::string _name;
		std::string _type;
		
		object() = delete;

	protected:



	public:

		virtual ~object() {};
		object(std::string name, std::string type);
		std::string name();
		std::string type();
		virtual std::string to_string();


	};

}


#endif