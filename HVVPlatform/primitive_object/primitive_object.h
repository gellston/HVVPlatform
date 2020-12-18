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

	template<class T> class array: public object {
	private:

		std::vector<double> __data;
		std::string _data_type;
		array() = delete;

	public:

		array(std::string name, std::vector<T>& data);
		~array() override { }

		std::vector<T>& data();
		unsigned int size();
		std::string data_type();
		std::string to_string()  override;
	};

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


	

	
	template<> class HVAPI_EXPORT array<std::vector<double>> : public object {
	private:
		
		std::vector<double> __data;
		std::string _data_type;
		array() = delete;

	public:

		array(std::string name, std::vector<double> & data);
		~array() override { }

		std::vector<double> & data();
		unsigned int size();
		std::string data_type();
		std::string to_string()  override;
	};


	template<> class HVAPI_EXPORT array<std::vector<std::string>> : public object {
	private:

		std::vector<std::string> __data;
		std::string _data_type;
		array() = delete;

	public:

		array(std::string name, std::vector<std::string> & data);
		~array() override { }

		std::vector<std::string>& data();
		unsigned int size();
		std::string data_type();
		std::string to_string()  override;
	};
}

#endif // ! HV_PRIMITIVE_OBJECT
