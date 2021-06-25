
#include <lineFitROI.h>
#include <memory>


// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "LineFitROI.h"


HV::V1::LineFitROI::LineFitROI(std::shared_ptr<hv::v1::object>& object) : HV::V1::Object(object) {

}

HV::V1::LineFitROI::LineFitROI(String^ name, double x, double y, double angle, double width, double height, bool is_flip, bool is_black_white)
	: HV::V1::Object(new hv::v1::lineFitROI(msclr::interop::marshal_as<std::string>(name), x, y, angle, width, height, is_flip, is_black_white)) {

}

HV::V1::LineFitROI::LineFitROI(HV::V1::Object^ object) : HV::V1::Object(object) {

}

HV::V1::LineFitROI::LineFitROI(hv::v1::object* object) : HV::V1::Object(object) {

}

HV::V1::LineFitROI::~LineFitROI() {
	this->!LineFitROI();
}

HV::V1::LineFitROI::!LineFitROI() {

}


double HV::V1::LineFitROI::X::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::lineFitROI>(this->_instance.get());
	return pointer->x();
}


double HV::V1::LineFitROI::Y::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::lineFitROI>(this->_instance.get());
	return pointer->y();
}

double HV::V1::LineFitROI::Angle::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::lineFitROI>(this->_instance.get());
	return pointer->angle();
}


double HV::V1::LineFitROI::Width::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::lineFitROI>(this->_instance.get());
	return pointer->width();
}


double HV::V1::LineFitROI::Height::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::lineFitROI>(this->_instance.get());
	return pointer->height();
}


bool HV::V1::LineFitROI::IsFlip::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::lineFitROI>(this->_instance.get());
	return pointer->is_flip();
}

bool HV::V1::LineFitROI::IsBlack2White::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::lineFitROI>(this->_instance.get());
	return pointer->is_black2white();
}