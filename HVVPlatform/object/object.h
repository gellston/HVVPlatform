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
		std::string _stack_name;
		
		object() = delete;

	protected:



	public:

		virtual ~object() {};
		object(std::string name, std::string type);
		std::string name();
		std::string type();
		std::string stack_name();
		void set_stack_name(std::string name);
		virtual std::string to_string();


	};

}


#endif