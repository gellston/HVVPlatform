

#include "primitive_object.h"



/// <summary>
/// boolean
/// </summary>
/// <param name="name"></param>
/// <param name="type"></param>
/// <returns></returns>


hv::v1::boolean::boolean(std::string name, bool data) : object(name, "boolean") {
	this->_data = data;
}

hv::v1::boolean::~boolean() {

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


hv::v1::number::~number() {

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

hv::v1::string::~string() {

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