
#include "circleROI.h"

hv::v1::circleROI::circleROI(std::string name, double _x, double _y, double _radius) : hv::v1::object(name, "circleROI") {
	this->_x = _x;
	this->_y = _y;
	this->_radius = _radius;


	if (this->_x < 0)
		this->_x = 0;

	if (this->_y < 0)
		this->_y = 0;

	if (this->_radius < 0)
		this->_radius = 0;

}


double hv::v1::circleROI::x() {
	return this->_x;
}

double hv::v1::circleROI::y() {
	return this->_y;
}

double hv::v1::circleROI::radius() {
	return this->_radius;
}


std::string hv::v1::circleROI::to_string() {
	std::string temp = "";
	temp += this->type();
	temp += ":";
	temp += this->name();
	temp += ":[x:";
	temp += std::to_string(this->_x);
	temp += ",y:";
	temp += std::to_string(this->_y);
	temp += ",radius:";
	temp += std::to_string(this->_radius);
	temp += "]";

	return temp;
}