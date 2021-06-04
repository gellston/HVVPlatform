
#include <rectROI.h>
#include <memory>


// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "RectROI.h"


HV::V1::RectROI::RectROI(std::shared_ptr<hv::v1::object>& object) : HV::V1::Object(object) {

}

HV::V1::RectROI::RectROI(String^ name, double x, double y, double width, double height) : HV::V1::Object(new hv::v1::rectROI(msclr::interop::marshal_as<std::string>(name), x, y, width, height)) {

}

HV::V1::RectROI::RectROI(HV::V1::Object^ object) : HV::V1::Object(object) {

}

HV::V1::RectROI::RectROI(hv::v1::object* object) : HV::V1::Object(object) {

}

HV::V1::RectROI::~RectROI() {

}

HV::V1::RectROI::!RectROI() {

}


double HV::V1::RectROI::X::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::rectROI>(this->_instance.get());
	return pointer->x();
}


double HV::V1::RectROI::Y::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::rectROI>(this->_instance.get());
	return pointer->y();
}

double HV::V1::RectROI::Width::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::rectROI>(this->_instance.get());
	return pointer->width();
}


double HV::V1::RectROI::Height::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::rectROI>(this->_instance.get());
	return pointer->height();
}