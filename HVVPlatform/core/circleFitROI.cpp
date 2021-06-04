
#include "circleFitROI.h"

hv::v1::circleFitROI::circleFitROI(std::string name, double _x, double _y, double _radius, double _startRatio, double _endRatio, bool _is_black2white) : hv::v1::object(name, "circleFitROI") {
	this->_x = _x;
	this->_y = _y;
	this->_startRatio = _startRatio;
	this->_radius = _radius;
	this->_endRatio = _endRatio;
	this->_is_black2white = _is_black2white;

	if (this->_x < 0)
		this->_x = 0;

	if (this->_y < 0)
		this->_y = 0;

	if (this->_startRatio < 0)
		this->_startRatio = 0;

	if (this->_endRatio < 0)
		this->_endRatio = 0;

	if (this->_radius < 0)
		this->_radius = 0;

}


double hv::v1::circleFitROI::x() {
	return this->_x;
}

double hv::v1::circleFitROI::y() {
	return this->_y;
}

double hv::v1::circleFitROI::radius() {
	return this->_radius;
}

double hv::v1::circleFitROI::startRatio() {
	return this->_startRatio;
}

double hv::v1::circleFitROI::endRatio() {
	return this->_endRatio;
}

bool hv::v1::circleFitROI::is_black2white() {
	return this->_is_black2white;
}

std::string hv::v1::circleFitROI::to_string() {
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
	temp += ",startRatio:";
	temp += std::to_string(this->_startRatio);
	temp += ",endRatio:";
	temp += std::to_string(this->_endRatio);
	temp += ",is_black_white:";
	temp += std::to_string(this->_is_black2white);
	temp += "]";

	return temp;
}