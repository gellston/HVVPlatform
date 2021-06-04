
#include "lineFitROI.h"

hv::v1::lineFitROI::lineFitROI(std::string name, double _x, double _y, double _angle, double _width, double _height, bool _is_flip, bool _is_black_white) : hv::v1::object(name, "lineFitROI") {
	this->_x = _x;
	this->_y = _y;
	this->_width = _width;
	this->_height = _height;
	this->_angle = _angle;
	this->_is_flip = _is_flip;
	this->_is_black_white = _is_black_white;


	if (this->_x < 0)
		this->_x = 0;

	if (this->_y < 0)
		this->_y = 0;

}


double hv::v1::lineFitROI::x() {
	return this->_x;
}

double hv::v1::lineFitROI::y() {
	return this->_y;
}

double hv::v1::lineFitROI::width() {
	return this->_width;
}

double hv::v1::lineFitROI::height() {
	return this->_height;
}

double hv::v1::lineFitROI::angle() {
	return this->_angle;
}

bool hv::v1::lineFitROI::is_flip() {
	return this->_is_flip;
}
bool hv::v1::lineFitROI::is_black2white() {
	return this->_is_black_white;
}

std::string hv::v1::lineFitROI::to_string() {
	std::string temp = "";
	temp += this->type();
	temp += ":";
	temp += this->name();
	temp += ":[x:";
	temp += std::to_string(this->_x);
	temp += ",y:";
	temp += std::to_string(this->_y);
	temp += ",angle:";
	temp += std::to_string(this->_angle);
	temp += ",width:";
	temp += std::to_string(this->_width);
	temp += ",height:";
	temp += std::to_string(this->_height);
	temp += ",is_flip:";
	temp += std::to_string(this->_is_flip);
	temp += ",is_black_white:";
	temp += std::to_string(this->_is_black_white);
	temp += "]";

	return temp;
}