
#include <circleFitROI.h>
#include <memory>


// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "CircleFitROI.h"


HV::V1::CircleFitROI::CircleFitROI(std::shared_ptr<hv::v1::object>& object) : HV::V1::Object(object) {

}

HV::V1::CircleFitROI::CircleFitROI(System::String^ Name, double x, double y, double radius, double startRatio, double endRatio, bool is_black2white)
	: HV::V1::Object(new hv::v1::circleFitROI(msclr::interop::marshal_as<std::string>(Name), x, y, radius, startRatio, endRatio, is_black2white)) {

}

HV::V1::CircleFitROI::CircleFitROI(HV::V1::Object^ object) : HV::V1::Object(object) {

}

HV::V1::CircleFitROI::CircleFitROI(hv::v1::object* object) : HV::V1::Object(object) {

}

HV::V1::CircleFitROI::~CircleFitROI() {
	this->!CircleFitROI();
}

HV::V1::CircleFitROI::!CircleFitROI() {

}


double HV::V1::CircleFitROI::X::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::circleFitROI>(this->_instance.get());
	return pointer->x();
}


double HV::V1::CircleFitROI::Y::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::circleFitROI>(this->_instance.get());
	return pointer->y();
}

double HV::V1::CircleFitROI::Radius::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::circleFitROI>(this->_instance.get());
	return pointer->radius();
}


double HV::V1::CircleFitROI::StartRatio::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::circleFitROI>(this->_instance.get());
	return pointer->startRatio();
}


double HV::V1::CircleFitROI::EndRatio::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::circleFitROI>(this->_instance.get());
	return pointer->endRatio();
}


bool HV::V1::CircleFitROI::IsBlack2White::get() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::circleFitROI>(this->_instance.get());
	return pointer->is_black2white();
}

