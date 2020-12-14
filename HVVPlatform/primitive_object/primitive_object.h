#pragma once



#ifndef  HV_PRIMITIVE_OBJECT
#define HV_PRIMITIVE_OBJECT

#include "object.h"

#include <vector>
#include <memory>


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


	class HVAPI_EXPORT array_number : public object {
	private:
		
		std::shared_ptr<double> __data;
		unsigned int _size;

		array_number() = delete;
	public:

		array_number(std::string name, double * data, unsigned int size);
		array_number(std::string name, std::shared_ptr<double> data, unsigned int size);
		~array_number() override { }
		std::shared_ptr<double> data();
		void data(double* data, unsigned int size);
		void data(std::shared_ptr<double> data, unsigned int size);
		unsigned int size();

		std::string to_string()  override;
	};
}



#endif // ! HV_PRIMITIVE_OBJECT
