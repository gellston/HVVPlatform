
#include <primitive_object.h>
#include <memory>


// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Boolean.h"


HV::V1::Boolean::Boolean(std::shared_ptr<hv::v1::object>& object) : HV::V1::Object(object) {

}

HV::V1::Boolean::Boolean(bool data) : HV::V1::Object(new hv::v1::boolean(data)) {
}

HV::V1::Boolean::Boolean(String^ name, bool data) : HV::V1::Object(new hv::v1::boolean(msclr::interop::marshal_as<std::string>(name), data)){
	
}

HV::V1::Boolean::Boolean(HV::V1::Object^ object) : HV::V1::Object(object){

}

HV::V1::Boolean::Boolean(hv::v1::object* object) : HV::V1::Object(object) {

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
