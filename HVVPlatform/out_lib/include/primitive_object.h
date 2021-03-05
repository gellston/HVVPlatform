#pragma once

#ifndef  HV_PRIMITIVE_OBJECT
#define HV_PRIMITIVE_OBJECT

#include "object.h"
#include <vector>
#include <memory>
#include <map>

namespace hv::v1 {

	class boolean;
	class number;
	class string;

	template<class T> class array: public object {
	private:

		std::vector<T> __data;
		std::string _data_type;
		array() = delete;

	public:

		array(std::string name, std::vector<T>& data);
		array(std::vector<T>& data);
		~array() override { }

		std::vector<T>& data();
		unsigned int size();
		std::string data_type();
		std::string to_string()  override;
	};

	template<class T> class map : public object {
	private:

		std::map<std::string, T> __data;
		std::string _data_type;
		map() = delete;

	public:

		map(std::string name, std::map<std::string, T> & data);
		map(std::map<std::string, T>& data);
		~map() override { }

		std::map<std::string, T> & data();
		unsigned int size();
		bool exist(std::string);
		T find(std::string);
		std::string data_type();
		std::string to_string()  override;
	};

	class boolean : public object {
	private:
		bool _data;
		boolean() = delete;
	public:

		boolean(std::string name, bool data);
		boolean(bool data);
		~boolean() override { }
		bool data();
		void data(bool data);
		std::string to_string() override;

	};


	class number : public object {
	private:
		double _data;
		number() = delete;
	public:

		number(std::string name, double data);
		number(double data);
		~number() override { }
		double data();
		void data(double data);
		std::string to_string() override;

	};


	class string : public object {
	private:
		std::string _data;
		string() = delete;
	public:

		string(std::string name, std::string data);
		string(std::string data);
		~string() override { }
		std::string data();
		void data(std::string data);
		std::string to_string() override;
	};


	

	
	template<> class array<double> : public object {
	private:
		
		std::vector<double> __data;
		std::string _data_type;
		array() = delete;

	public:

		array(std::string name, std::vector<double> & data);
		array(std::vector<double>& data);
		~array() override { }

		std::vector<double> & data();
		unsigned int size();
		std::string data_type();
		std::string to_string()  override;
	};


	template<> class array<std::string> : public object {
	private:

		std::vector<std::string> __data;
		std::string _data_type;
		array() = delete;

	public:

		array(std::string name, std::vector<std::string> & data);
		array(std::vector<std::string>& data);
		~array() override { }

		std::vector<std::string>& data();
		unsigned int size();
		std::string data_type();
		std::string to_string()  override;
	};


	template<> class array<bool> : public object {
	private:

		std::vector<bool> __data;
		std::string _data_type;
		array() = delete;

	public:

		array(std::string name, std::vector<bool>& data);
		array(std::vector<bool>& data);
		~array() override { }

		std::vector<bool>& data();
		unsigned int size();
		std::string data_type();
		std::string to_string()  override;
	};

	template<> class map<double> : public object {
	private:

		std::map<std::string, double> __data;
		std::string _data_type;
		map() = delete;

	public:

		map(std::string name, std::map<std::string, double>& data);
		map(std::map<std::string, double>& data);
		~map() override { }

		std::map<std::string, double>& data();
		unsigned int size();
		bool exist(std::string);
		double find(std::string);
		std::string data_type();
		std::string to_string()  override;
	};

	template<> class map<std::string> : public object {
	private:

		std::map<std::string, std::string> __data;
		std::string _data_type;
		map() = delete;

	public:

		map(std::string name, std::map<std::string, std::string>& data);
		map(std::map<std::string, std::string>& data);
		~map() override { }

		std::map<std::string, std::string>& data();
		unsigned int size();
		bool exist(std::string);
		std::string find(std::string);
		std::string data_type();
		std::string to_string()  override;
	};

	template<> class map<bool> : public object {
	private:

		std::map<std::string, bool> __data;
		std::string _data_type;
		map() = delete;

	public:

		map(std::string name, std::map<std::string, bool>& data);
		map(std::map<std::string, bool>& data);
	    ~map() override { }

		std::map<std::string, bool>& data();
		unsigned int size();
		bool exist(std::string);
		bool find(std::string);
		std::string data_type();
		std::string to_string()  override;
	};
}

#endif // ! HV_PRIMITIVE_OBJECT
