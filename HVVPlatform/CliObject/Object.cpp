
// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Object.h"


HV::V1::Object::Object() : _instance(new hv::v1::object("", "")) {

}


HV::V1::Object::Object(std::shared_ptr<hv::v1::object>& object) : _instance(object) {

}


HV::V1::Object::Object(System::String^ Name, System::String^ Type) : _instance(new hv::v1::object(msclr::interop::marshal_as<std::string>(Name),
	msclr::interop::marshal_as<std::string>(Type))) {

}

HV::V1::Object::Object(HV::V1::Object^ object) : _instance(object->_instance.get()) {
	
}

//HV::V1::Object::~Object() {
//
//}
//
//HV::V1::Object::!Object() {
//
//}

String^ HV::V1::Object::Name::get() {

	return gcnew String(this->_instance->name().c_str());
}

String^ HV::V1::Object::Type::get() {

	return gcnew String(this->_instance->type().c_str());
}

String^ HV::V1::Object::ToString::get() {

	return gcnew String(this->_instance->to_string().c_str());
}

