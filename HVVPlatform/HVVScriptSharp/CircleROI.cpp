
#include <circleROI.h>
#include <memory>


// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "CircleROI.h"


HV::V1::CircleROI::CircleROI(std::shared_ptr<hv::v1::object>& object) : HV::V1::Object(object) {

}

HV::V1::CircleROI::CircleROI(System::String^ Name, double x, double y, double radius)
	: HV::V1::Object(new hv::v1::circleROI(msclr::interop::marshal_as<std::string>(Name), x, y, radius)) {

}

HV::V1::CircleROI::CircleROI(HV::V1::Object^ object) : HV::V1::Object(object) {

}

HV::V1::CircleROI::CircleROI(hv::v1::object* object) : HV::V1::Object(object) {

}

HV::V1::CircleROI::~CircleROI() {
	this->!CircleROI();
}

HV::V1::CircleROI::!CircleROI() {

}


double HV::V1::CircleROI::X::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::circleROI>(this->_instance.get());
	return pointer->x();
}


double HV::V1::CircleROI::Y::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::circleROI>(this->_instance.get());
	return pointer->y();
}

double HV::V1::CircleROI::Radius::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::circleROI>(this->_instance.get());
	return pointer->radius();
}
