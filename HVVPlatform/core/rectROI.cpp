
#include "rectROI.h"

hv::v1::rectROI::rectROI(std::string _name, double _x, double _y, double _width, double _height) : hv::v1::object(_name, "rectROI") {
	this->_x = _x;
	this->_y = _y;
	this->_width = _width;
	this->_height = _height;

	if (this->_x < 0)
		this->_x = 0;

	if (this->_y < 0)
		this->_y = 0;

}


double hv::v1::rectROI::x() {
	return this->_x;
}

double hv::v1::rectROI::y() {
	return this->_y;
}

double hv::v1::rectROI::width() {
	return this->_width;
}

double hv::v1::rectROI::height() {
	return this->_height;
}


std::string hv::v1::rectROI::to_string() {
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