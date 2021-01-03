
// Managed Header
#include <msclr/marshal_cppstd.h>




// Native Header
#include "Object.h"



void HV::V1::Object::reset() {

}



HV::V1::Object::Object(System::String^ Name, System::String^ Type) {

}


HV::V1::Object::~Object() {

}

HV::V1::Object::!Object() {

}

String^ HV::V1::Object::Name::get() {


	return gcnew String("test");
}

String^ HV::V1::Object::Type::get() {


	return gcnew String("test");
}

String^ HV::V1::Object::ToString() {
	return gcnew String("test");
}
