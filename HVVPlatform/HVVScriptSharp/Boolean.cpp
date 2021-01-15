
// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Boolean.h"
#include "Object.h"

HV::V1::Boolean::Boolean(std::shared_ptr<hv::v1::object>& object) : _instance(std::dynamic_pointer_cast<hv::v1::boolean>(object)){

}

HV::V1::Boolean::Boolean(bool data) : _instance(new hv::v1::boolean(data)){
	
}

HV::V1::Boolean::Boolean(String^ name, bool data) : _instance(new hv::v1::boolean(msclr::interop::marshal_as<std::string>(name), data)) {

}

HV::V1::Boolean::~Boolean() {

}