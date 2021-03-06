

#include <primitive_object.h>
#include <memory>


// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Number.h"

HV::V1::Number::Number(std::shared_ptr<hv::v1::object>& object) : HV::V1::Object(object) {

}

HV::V1::Number::Number(double data) : HV::V1::Object(new hv::v1::number(data)){

}

HV::V1::Number::Number(String^ name, double data) : HV::V1::Object(new hv::v1::number(msclr::interop::marshal_as<std::string>(name), data)) {

}

HV::V1::Number::Number(HV::V1::Object^ object) : HV::V1::Object(object) {

}

HV::V1::Number::Number(hv::v1::object* object) : HV::V1::Object(object){

}

HV::V1::Number::~Number() {
	this->!Number();
}

HV::V1::Number::!Number() {

}


double HV::V1::Number::Data() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::number>(this->_instance.get());
	return pointer->data();
}
void HV::V1::Number::Data(double data) {
	auto pointer = std::dynamic_pointer_cast<hv::v1::number>(this->_instance.get());
	pointer->data(data);
}
