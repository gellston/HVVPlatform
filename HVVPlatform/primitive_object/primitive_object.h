#pragma once



#ifndef  HV_PRIMITIVE_OBJECT
#define HV_PRIMITIVE_OBJECT

#include "object.h"

namespace hv::v1 {

	class boolean;
	class number;
	class string;

	class HVAPI_EXPORT boolean : public object {
	private:
		bool _data;
		boolean() = delete;
	public:

		boolean(std::string name, bool data);
		~boolean();
		bool data();
		void data(bool data);
		std::string to_string() override;

	};


	class HVAPI_EXPORT number : public object {
	private:
		double _data;
		number() = delete;
	public:

		number(std::string name, double data);
		~number();
		double data();
		void data(double data);
		std::string to_string() override;

	};


	class HVAPI_EXPORT string : public object {
	private:
		std::string _data;
		string() = delete;
	public:

		string(std::string name, std::string data);
		~string();
		std::string data();
		void data(std::string data);
		std::string to_string() override;
	};
}



#endif // ! HV_PRIMITIVE_OBJECT
