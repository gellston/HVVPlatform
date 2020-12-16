

#include "primitive_object.h"

#include <stdexcept>
#include <iostream>


/// <summary>
/// boolean
/// </summary>
/// <param name="name"></param>
/// <param name="type"></param>
/// <returns></returns>


hv::v1::boolean::boolean(std::string name, bool data) : object(name, "boolean") {
	this->_data = data;
}

bool hv::v1::boolean::data() {
	return this->_data;
}

void hv::v1::boolean::data(bool data) {
	this->_data = data;
}

std::string hv::v1::boolean::to_string() {
	if (this->_data == true) return "true";
	else return "false";
}




/// <summary>
/// number
/// </summary>
/// <param name="name"></param>
/// <param name="type"></param>
/// <returns></returns>

hv::v1::number::number(std::string name, double data) : object(name, "number") {
	this->_data = data;
}


double hv::v1::number::data() {
	return this->_data;
}

void hv::v1::number::data(double data) {
	this->_data = data;
}

std::string hv::v1::number::to_string() {
	return std::to_string(this->_data);
}




hv::v1::string::string(std::string name, std::string data) :object(name, "string") {
	this->_data = data;
}

std::string hv::v1::string::data() {
	return this->_data;
}

void hv::v1::string::data(std::string data) {
	this->_data = data;
}

std::string hv::v1::string::to_string() {
	return this->_data;
}











hv::v1::array::array(std::string name, double * data, unsigned int size) :
																			_size(0),
																			__data(std::monostate{}),
																			object(name, "array"),
																			_data_type("number"){

	if (size < 0 || size == 0)
		throw std::runtime_error("Invalid size");

	if (data == nullptr)
		throw std::runtime_error("Invalid pointer");

	this->_size = size;
	std::vector<double> vec_data;
	vec_data.resize(_size);
	memcpy(vec_data.data(), data, sizeof(double) * size);
	this->__data = vec_data;
}

hv::v1::array::array(std::string name, hv::v1::array_type& data) :
																	_size(0),
																	__data(std::monostate{}),
																	object(name, "array"){
	
	if (std::holds_alternative<std::vector<std::string>>(data)) {
		auto string_vector = std::get<std::vector<std::string>>(data);
		unsigned int size = static_cast<unsigned int>(string_vector.size());
		this->__data = string_vector;
		this->_size = size;
		_data_type = "string";
	}
	else if (std::holds_alternative<std::vector<double>>(data)) {
		auto double_vector = std::get<std::vector<double>>(data);
		unsigned int size = static_cast<unsigned int>(double_vector.size());
		this->__data = double_vector;
		this->_size = size;
		_data_type = "number";
	}
	else {
		throw std::runtime_error("Invalid pointer");
	}
}


hv::v1::array_type & hv::v1::array::data() {
	return this->__data;
}

unsigned int hv::v1::array::size() {
	return this->_size;
}

void hv::v1::array::data(double * data, unsigned int size) {

	if (size < 0 || size == 0)
		throw std::runtime_error("Invalid size");

	if (data == nullptr)
		throw std::runtime_error("Invalid pointer");

	if (this->_size != size) {
		this->_size = size;

		std::vector<double> data;
		data.resize(size);
		this->__data = data;
	}
	auto member_data = std::get<std::vector<double>>(this->__data);
	auto pointer = member_data.data();

	memcpy(pointer, data, size);

	_data_type = "number";
}

std::string hv::v1::array::to_string() {
	std::string temp = "";
	temp += "array = [ \n";
	for (unsigned int index = 0; index < this->_size; index++) {
		if (!this->_data_type.compare("number")) {
			auto __double_array = std::get<std::vector<double>>(this->__data);
			temp += std::to_string(__double_array[index]) += ",";
		}
		else if (!this->_data_type.compare("string")) {
			auto __string_vector = std::get<std::vector<std::string>>(this->__data);
			temp += __string_vector[index] += ",";
		}
	}
	temp += "]\n";
	return temp;
}

std::string hv::v1::array::data_type() {
	return this->_data_type;
}
