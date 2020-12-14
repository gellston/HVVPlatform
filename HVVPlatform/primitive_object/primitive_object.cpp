

#include "primitive_object.h"


#include <stdexcept>



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











hv::v1::array_number::array_number(std::string name, double * data, unsigned int size) :
																				_size(0),
																				__data(nullptr),
																				object(name, "array_number") {

	if (size < 0 || size == 0)
		throw std::runtime_error("Invalid size");

	if (data == nullptr)
		throw std::runtime_error("Invalid pointer");

	this->_size = size;
	
	std::shared_ptr<double> smart_ptr(new double[size], [](double* pointer) {
		delete[] pointer;
	});

	this->__data = smart_ptr;

	memcpy(this->__data.get(), data, sizeof(double) * size);

}

hv::v1::array_number::array_number(std::string name, std::shared_ptr<double> data, unsigned int size) : 
																							   _size(0),
																							   __data(nullptr),
																							   object(name, "array_number") {

	if (size < 0 || size == 0)
		throw std::runtime_error("Invalid size");

	if (data.get() == nullptr)
		throw std::runtime_error("Invalid pointer");

	this->_size = size;


	this->__data = data;
}



std::shared_ptr<double> hv::v1::array_number::data() {

	return this->__data;
}

unsigned int hv::v1::array_number::size() {
	return this->_size;
}

void hv::v1::array_number::data(double * data, unsigned int size) {

	if (size < 0 || size == 0)
		throw std::runtime_error("Invalid size");

	if (data == nullptr)
		throw std::runtime_error("Invalid pointer");

	if (this->_size != size) {
		this->_size = size;
		std::shared_ptr<double> smart_ptr(new double[size], [](double* pointer) {
			delete[] pointer;
		});

		this->__data = smart_ptr;
	}
	
	memcpy(this->__data.get(), data, size);

}

void hv::v1::array_number::data(std::shared_ptr<double> data, unsigned int size) {
	if (size < 0 || size == 0)
		throw std::runtime_error("Invalid size");

	if (data == nullptr)
		throw std::runtime_error("Invalid pointer");

	this->_size = size;

	this->__data = data;
}


std::string hv::v1::array_number::to_string() {
	std::string temp = "";
	temp += this->name();
	temp += " : ";
	temp += this->type();
	return temp;
}
