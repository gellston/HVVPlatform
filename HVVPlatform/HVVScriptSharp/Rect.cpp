
#include <rect.h>
#include <memory>


// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Rect.h"


HV::V1::Rect::Rect(std::shared_ptr<hv::v1::object>& object) : HV::V1::Object(object) {

}

HV::V1::Rect::Rect(String^ name, double x, double y, double width, double height) : HV::V1::Object(new hv::v1::rect(msclr::interop::marshal_as<std::string>(name), x, y, width, height)) {

}

HV::V1::Rect::Rect(HV::V1::Object^ object) : HV::V1::Object(object) {

}

HV::V1::Rect::Rect(hv::v1::object* object) : HV::V1::Object(object) {

}

HV::V1::Rect::~Rect() {

}

HV::V1::Rect::!Rect() {

}


double HV::V1::Rect::X::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::rect>(this->_instance.get());
	return pointer->x();
}


double HV::V1::Rect::Y::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::rect>(this->_instance.get());
	return pointer->y();
}

double HV::V1::Rect::Width::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::rect>(this->_instance.get());
	return pointer->width();
}


double HV::V1::Rect::Height::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::rect>(this->_instance.get());
	return pointer->height();
}