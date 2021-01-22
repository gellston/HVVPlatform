#pragma once


#include "object.h"

#include <string>


using namespace hv;
using namespace v1;


/// <summary>
/// Object
/// </summary>
/// <param name="name"></param>
/// <param name="type"></param>
/// <returns></returns>

hv::v1::object::object(std::string _name, std::string _type) {
	this->_name = _name;
	this->_type = _type;
}

std::string hv::v1::object::name() {
	return this->_name;
}

std::string hv::v1::object::type() {
	return this->_type;
}

std::string hv::v1::object::to_string() {
	return "object";
}

