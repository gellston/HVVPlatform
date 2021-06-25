#include <image.h>
#include <memory>
#include <map>

// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Image.h"
#include "Point.h"



using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace System::Diagnostics;


//namespace hv::v1 {
//	class pimpl_image_casting_container {
//	private:
//		std::map<std::string, int> _converter;
//	public:
//		pimpl_image_casting_container() {
//			register_casting_type(point);
//		}
//
//		std::map<std::string, int>& converter() {
//			return _converter;
//		}
//	};
//}

HV::V1::Image::Image(std::shared_ptr<hv::v1::object>& object) : HV::V1::Object(object){

}

HV::V1::Image::Image(HV::V1::Object^ data) : HV::V1::Object(data) {

}

HV::V1::Image::Image(hv::v1::object* object) : HV::V1::Object(object) {

}

HV::V1::Image::~Image() {
	this->!Image();
}

HV::V1::Image::!Image() {

}


HV::V1::Image::Image(String^ name, unsigned int width, unsigned int height, unsigned int type, double resolution) : HV::V1::Image(new hv::v1::image(msclr::interop::marshal_as<std::string>(name), width, height, type, resolution)) {

}
HV::V1::Image::Image(Image^ copy) : HV::V1::Object(copy) {

}

void HV::V1::Image::RegisterDrawObject(HV::V1::Object^ _object) {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get())->register_draw_object(_object->_instance.get());
}
List<HV::V1::Object^>^ HV::V1::Image::DrawObjects::get() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	auto list = gcnew List<HV::V1::Object^>();
	auto native_list = target->drarw_objects();

	for (auto& element : native_list) {

		if (element->type().length() <= 0) continue;

		std::string upperTypeName = element->type();
		upperTypeName[0] = toupper(upperTypeName[0]);

		std::string namespaceName = "HV.V1.";
		std::string fullNamespaceName = namespaceName + upperTypeName;

		try {
			System::String^ managedTypeName = gcnew System::String(fullNamespaceName.c_str());

			System::Type^ object_type = System::Type::GetType(managedTypeName);
			
			HV::V1::Object^ managedObject = gcnew HV::V1::Object(element);
			HV::V1::Object^ createdObject = (HV::V1::Object^)Activator::CreateInstance(object_type, managedObject);
			list->Add(createdObject);
		}
		catch (Exception^ e) {
			Debug::WriteLine(e->Message);
			HV::V1::Object^ managedObject = gcnew HV::V1::Object(element);
			list->Add(managedObject);
		}
	}
	return list;
}



int HV::V1::Image::Width::get() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->width();
}
int HV::V1::Image::Height::get() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->height();
}
int HV::V1::Image::Size::get() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->size();
}
int HV::V1::Image::Stride::get() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->stride();
}
int HV::V1::Image::Count::get() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->count();
}
IntPtr HV::V1::Image::Ptr() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	IntPtr pointer(target->ptr());
	return pointer;
}
bool HV::V1::Image::Copy(Image^ data) {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	auto native_pointer = std::dynamic_pointer_cast<hv::v1::image>(data->_instance.get());
	return target->copy(*native_pointer.get());
}
bool HV::V1::Image::Fill(double value) {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->fill(value);
}
double HV::V1::Image::Reduce() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->reduce();
}
bool HV::V1::Image::Multiply(double value) {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->multiply(value);
}
bool HV::V1::Image::Divide(double value) {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->divide(value);
}
bool HV::V1::Image::Add(double value) {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->add(value);
}
bool HV::V1::Image::Minus(double value) {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->minus(value);
}
double HV::V1::Image::Resolution::get() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->resolution();
}

HV::V1::ImageDataType HV::V1::Image::PixelType::get() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());

	return static_cast<HV::V1::ImageDataType>(target->pixel_type());
}