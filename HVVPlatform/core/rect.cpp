
#include "rect.h"

hv::v1::rect::rect(std::string _name, double _x, double _y, double _width, double _height) : hv::v1::object(_name, "rect") {
	this->_x = _x;
	this->_y = _y;
	this->_width = _width;
	this->_height = _height;
}


double hv::v1::rect::x() {
	return this->_x;
}

double hv::v1::rect::y() {
	return this->_y;
}

double hv::v1::rect::width() {
	return this->_width;
}

double hv::v1::rect::height() {
	return this->_height;
}


std::string hv::v1::rect::to_string() {
	std::string temp = "";
	temp += this->type();
	temp += ":";
	temp += this->name();
	temp += ":[x:";
	temp += std::to_string(this->_x);
	temp += ",y:";
	temp += std::to_string(this->_y);
	temp += ",width:";
	temp += std::to_string(this->_width);
	temp += ",height:";
	temp += std::to_string(this->_height);
	temp += "]";

	return temp;
}