
// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Object.h"




void HV::V1::Object::reset() {
	delete this->_instance;
}

HV::V1::Object::Object(std::shared_ptr<hv::v1::object> & object) : _instance(nullptr){
	this->_instance = new std::shared_ptr<hv::v1::object>();
	*this->_instance = object;
}


HV::V1::Object::Object(System::String^ Name, System::String^ Type) {
	auto native_name = msclr::interop::marshal_as<std::string>(Name);
	auto native_type = msclr::interop::marshal_as<std::string>(Type);
	this->_instance = new std::shared_ptr<hv::v1::object>();

	std::shared_ptr<hv::v1::object> shared_object(new hv::v1::object(native_name, native_type));

	*(this->_instance) = shared_object;
}


HV::V1::Object::~Object() {
	this->reset();
}

HV::V1::Object::!Object() {
	this->reset();
}

String^ HV::V1::Object::Name::get() {
	
	auto native_name = (*(this->_instance))->name();

	return gcnew String(native_name.c_str());
}

String^ HV::V1::Object::Type::get() {
	auto native_type = (*(this->_instance))->type();

	return gcnew String(native_type.c_str());
}

String^ HV::V1::Object::ToString() {
	auto native_to_string = (*(this->_instance))->to_string();

	return gcnew String(native_to_string.c_str());
}
