
// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Boolean.h"
#include "Object.h"

HV::V1::Boolean::Boolean(std::shared_ptr<hv::v1::object>& object) : _instance(std::dynamic_pointer_cast<hv::v1::boolean>(object)),
																    HV::V1::Object(object){
	
}

HV::V1::Boolean::Boolean(bool data) : _instance(new hv::v1::boolean(data)),
									  HV::V1::Object(std::dynamic_pointer_cast<hv::v1::object>(_instance.get())){
	
}

HV::V1::Boolean::Boolean(String^ name, bool data) : _instance(new hv::v1::boolean(msclr::interop::marshal_as<std::string>(name), data)),
												    HV::V1::Object(std::dynamic_pointer_cast<hv::v1::object>(_instance.get())){

}

HV::V1::Boolean::~Boolean() {

}

HV::V1::Boolean::!Boolean() {

}

bool HV::V1::Boolean::Data() {
	return this->_instance->data();
}
void HV::V1::Boolean::Data(bool data) {
	this->_instance->data(data);
}