
#include <primitive_object.h>
#include <memory>


// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Boolean.h"



HV::V1::Boolean::Boolean(bool data) {
	this->_instance = new hv::v1::boolean(data);
}

HV::V1::Boolean::Boolean(String^ name, bool data){
	this->_instance = new hv::v1::boolean(msclr::interop::marshal_as<std::string>(name), data);
}

HV::V1::Boolean::Boolean(HV::V1::Object^ object){
	this->_instance = object->_instance.get();
}

HV::V1::Boolean::~Boolean() {

}

HV::V1::Boolean::!Boolean() {

}


bool HV::V1::Boolean::Data() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::boolean>(this->_instance.get());
	return pointer->data();
}
void HV::V1::Boolean::Data(bool data) {
	auto pointer = std::dynamic_pointer_cast<hv::v1::boolean>(this->_instance.get());
	pointer->data(data);
}
