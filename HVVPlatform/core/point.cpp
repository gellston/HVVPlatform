
#include "point.h"



hv::v1::point::point(std::string _name, double _x, double _y) : hv::v1::object(_name, "point") {
	this->_x = _x;
	this->_y = _y;

}


double hv::v1::point::x() {
	return this->_x;
}

double hv::v1::point::y() {
	return this->_y;
}


std::string hv::v1::point::to_string() {
	std::string temp = "";
	temp += this->type();
	temp += ":";
	temp += this->name();
	temp += ":[x:";
	temp += std::to_string(this->_x);
	temp += ",y:";
	temp += std::to_string(this->_y);
	temp += "]";

	return temp;
}