#pragma once

#ifndef  HV_PRIMITIVE_OBJECT
#define HV_PRIMITIVE_OBJECT

#include "object.h"

#include <vector>
#include <memory>
#include <variant>

namespace hv::v1 {

	using array_type = std::variant<std::monostate, std::vector<std::string>, std::vector<double>>;

	class boolean;
	class number;
	class string;
	class array;

	class HVAPI_EXPORT boolean : public object {
	private:
		bool _data;
		boolean() = delete;
	public:

		boolean(std::string name, bool data);
		~boolean() override { }
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
		~number() override { }
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
		~string() override { }
		std::string data();
		void data(std::string data);
		std::string to_string() override;
	};


	class HVAPI_EXPORT array : public object {
	private:
		
		array_type __data;
		std::string _data_type;
		unsigned int _size;
		array() = delete;

	public:

		array(std::string name, double * data, unsigned int size);
		array(std::string name, array_type & data);
		~array() override { }

		array_type & data();
		void data(double* data, unsigned int size);
		unsigned int size();
		std::string data_type();
		std::string to_string()  override;
	};
}

#endif // ! HV_PRIMITIVE_OBJECT
