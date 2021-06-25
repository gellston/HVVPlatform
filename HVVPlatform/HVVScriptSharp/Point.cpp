
#include <point.h>
#include <memory>


// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Point.h"


HV::V1::Point::Point(std::shared_ptr<hv::v1::object>& object) : HV::V1::Object(object) {

}

HV::V1::Point::Point(String^ name,double x, double y) : HV::V1::Object(new hv::v1::point(msclr::interop::marshal_as<std::string>(name), x, y)){

}

HV::V1::Point::Point(HV::V1::Object^ object) : HV::V1::Object(object) {

}

HV::V1::Point::Point(hv::v1::object* object) : HV::V1::Object(object) {

}

HV::V1::Point::~Point() {
	this->!Point();
}

HV::V1::Point::!Point() {

}


double HV::V1::Point::X::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::point>(this->_instance.get());
	return pointer->x();
}


double HV::V1::Point::Y::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::point>(this->_instance.get());
	return pointer->y();
}