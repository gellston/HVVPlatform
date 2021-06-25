
#include <primitive_object.h>
#include <memory>


// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "String.h"


HV::V1::String::String(std::shared_ptr<hv::v1::object>& object) : HV::V1::Object(object) {

}

HV::V1::String::String(System::String^ data) : HV::V1::Object(new hv::v1::string(msclr::interop::marshal_as<std::string>(data))) {

}

HV::V1::String::String(System::String^ name, System::String^ data) : HV::V1::Object(new hv::v1::string(msclr::interop::marshal_as<std::string>(name), msclr::interop::marshal_as<std::string>(data))) {

}

HV::V1::String::String(HV::V1::Object^ object) : HV::V1::Object(object) {

}

HV::V1::String::String(hv::v1::object* object) : HV::V1::Object(object) {

}

HV::V1::String::~String() {
	this->!String();
}

HV::V1::String::!String() {

}


System::String^ HV::V1::String::Data() {
	auto pointer = std::dynamic_pointer_cast<hv::v1::string>(this->_instance.get());
	return gcnew System::String(pointer->data().c_str());
}
void HV::V1::String::Data(System::String^ data) {
	auto pointer = std::dynamic_pointer_cast<hv::v1::string>(this->_instance.get());
	pointer->data(msclr::interop::marshal_as<std::string>(data));
}
