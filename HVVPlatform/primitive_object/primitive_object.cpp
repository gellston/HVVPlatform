

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











// double array

hv::v1::array<std::vector<double>>::array(std::string name, std::vector<double> & data) : object(name, "array"){
	
	this->__data = data;
	_data_type = "number";

}


std::vector<double> & hv::v1::array<std::vector<double>>::data() {
	return this->__data;
}

unsigned int hv::v1::array<std::vector<double>>::size() {
	return static_cast<unsigned int>(this->__data.size());
}

std::string hv::v1::array<std::vector<double>>::to_string() {
	std::string temp = "";
	temp += "array = [ \n";
	for (unsigned int index = 0; index < this->__data.size(); index++) {
		temp += std::to_string(this->__data[index]) += ",";
	}
	temp += "]\n";
	return temp;
}

std::string hv::v1::array<std::vector<double>>::data_type() {
	return this->_data_type;
}






// std::string array

hv::v1::array<std::vector<std::string>>::array(std::string name, std::vector<std::string>& data) : object(name, "array") {

	this->__data = data;
	_data_type = "string";

}


std::vector<std::string>& hv::v1::array<std::vector<std::string>>::data() {
	return this->__data;
}

unsigned int hv::v1::array<std::vector<std::string>>::size() {
	return static_cast<unsigned int>(this->__data.size());
}

std::string hv::v1::array<std::vector<std::string>>::to_string() {
	std::string temp = "";
	temp += "array = [ \n";
	for (unsigned int index = 0; index < this->__data.size(); index++) {
		temp += this->__data[index] += ",";
	}
	temp += "]\n";
	return temp;
}

std::string hv::v1::array<std::vector<std::string>>::data_type() {
	return this->_data_type;
}
