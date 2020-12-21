

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

hv::v1::boolean::boolean(bool data) : boolean("anonymous", data) {
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

hv::v1::number::number(double data) : number("anonymous", data) {
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

hv::v1::string::string(std::string data) : string("anonymous", data) {
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










// array
// double array

hv::v1::array<double>::array(std::string name, std::vector<double> & data) : object(name, "array"){
	
	this->__data = data;
	_data_type = "number";

}

hv::v1::array<double>::array(std::vector<double>& data) : array("anonymous", data) {
}

std::vector<double> & hv::v1::array<double>::data() {
	return this->__data;
}

unsigned int hv::v1::array<double>::size() {
	return static_cast<unsigned int>(this->__data.size());
}

std::string hv::v1::array<double>::to_string() {
	std::string temp = "";
	temp += "array = [ \n";
	for (unsigned int index = 0; index < this->__data.size(); index++) {
		temp += std::to_string(this->__data[index]) += ",";
	}
	temp += "]\n";
	return temp;
}

std::string hv::v1::array<double>::data_type() {
	return this->_data_type;
}






// std::string array

hv::v1::array<std::string>::array(std::string name, std::vector<std::string>& data) : object(name, "array") {

	this->__data = data;
	_data_type = "string";

}

hv::v1::array<std::string>::array(std::vector<std::string>& data) : array("anonymous", data) {

}


std::vector<std::string>& hv::v1::array<std::string>::data() {
	return this->__data;
}

unsigned int hv::v1::array<std::string>::size() {
	return static_cast<unsigned int>(this->__data.size());
}

std::string hv::v1::array<std::string>::to_string() {
	std::string temp = "";
	temp += "array = [ \n";
	for (unsigned int index = 0; index < this->__data.size(); index++) {
		temp += this->__data[index] += ",";
	}
	temp += "]\n";
	return temp;
}

std::string hv::v1::array<std::string>::data_type() {
	return this->_data_type;
}





// std::string array

hv::v1::array<bool>::array(std::string name, std::vector<bool>& data) : object(name, "array") {

	this->__data = data;
	_data_type = "boolean";

}

hv::v1::array<bool>::array(std::vector<bool>& data) : array("anonymous", data) {

}

std::vector<bool>& hv::v1::array<bool>::data() {
	return this->__data;
}

unsigned int hv::v1::array<bool>::size() {
	return static_cast<unsigned int>(this->__data.size());
}

std::string hv::v1::array<bool>::to_string() {
	std::string temp = "";
	temp += "array = [ \n";
	for (unsigned int index = 0; index < this->__data.size(); index++) {
		temp += std::to_string(this->__data[index]) += ",";
	}
	temp += "]\n";
	return temp;
}

std::string hv::v1::array<bool>::data_type() {
	return this->_data_type;
}




// map
// double
hv::v1::map<double>::map(std::string name, std::map<std::string, double>& data) : object(name, "map") {

	this->__data = data;
	_data_type = "number";

}

hv::v1::map<double>::map(std::map<std::string, double>& data) : map("anonymous", data) {

}

std::map<std::string, double>&  hv::v1::map<double>::data() {

	return this->__data;
}

unsigned int hv::v1::map<double>::size() {

	return static_cast<unsigned int>(this->__data.size());
}

bool hv::v1::map<double>::exist(std::string key) {
	if (this->__data.find(key) == this->__data.end())
		return false;

	return true;
}

double hv::v1::map<double>::find(std::string key) {
	if (this->__data.find(key) == this->__data.end())
		throw std::runtime_error("Key is not exists");

	return this->__data[key];
}

std::string hv::v1::map<double>::data_type() {
	return this->_data_type;
}

std::string hv::v1::map<double>::to_string() {

	std::string temp = "";
	temp += "map = [ \n";
	for (auto element : this->__data) {
		temp += element.first;
		temp += " : ";
		temp += std::to_string(element.second);
		temp += "\n";
	}
	temp += "]\n";
	return temp;

}





// std::string
hv::v1::map<std::string>::map(std::string name, std::map<std::string, std::string>& data) : object(name, "map") {

	this->__data = data;
	_data_type = "string";

}

hv::v1::map<std::string>::map(std::map<std::string, std::string>& data) : map("anonymous", data) {

}


std::map<std::string, std::string>& hv::v1::map<std::string>::data() {

	return this->__data;
}

unsigned int hv::v1::map<std::string>::size() {

	return static_cast<unsigned int>(this->__data.size());
}

bool hv::v1::map<std::string>::exist(std::string key) {
	if (this->__data.find(key) == this->__data.end())
		return false;

	return true;
}

std::string hv::v1::map<std::string>::find(std::string key) {
	if (this->__data.find(key) == this->__data.end())
		throw std::runtime_error("Key is not exists");

	return this->__data[key];
}

std::string hv::v1::map<std::string>::data_type() {
	return this->_data_type;
}

std::string hv::v1::map<std::string>::to_string() {

	std::string temp = "";
	temp += "map = [ \n";
	for (auto element : this->__data) {
		temp += element.first;
		temp += " : ";
		temp += element.second;
		temp += "\n";
	}
	temp += "]\n";
	return temp;

}






// bool
hv::v1::map<bool>::map(std::string name, std::map<std::string, bool>& data) : object(name, "map") {

	this->__data = data;
	_data_type = "boolean";

}

hv::v1::map<bool>::map(std::map<std::string, bool>& data) : map("anonymous", data) {

}

std::map<std::string, bool>& hv::v1::map<bool>::data() {

	return this->__data;
}

unsigned int hv::v1::map<bool>::size() {

	return static_cast<unsigned int>(this->__data.size());
}

bool hv::v1::map<bool>::exist(std::string key) {
	if (this->__data.find(key) == this->__data.end())
		return false;

	return true;
}

bool hv::v1::map<bool>::find(std::string key) {
	if (this->__data.find(key) == this->__data.end())
		throw std::runtime_error("Key is not exists");

	return this->__data[key];
}

std::string hv::v1::map<bool>::data_type() {
	return this->_data_type;
}

std::string hv::v1::map<bool>::to_string() {

	std::string temp = "";
	temp += "map = [ \n";
	for (auto element : this->__data) {
		temp += element.first;
		temp += " : ";
		temp += element.second;
		temp += "\n";
	}
	temp += "]\n";
	return temp;

}