
// Managed Header
#include <msclr/marshal_cppstd.h>


// Native Header
#include "Image.h"



HV::V1::Image::Image(HV::V1::Object^ data) {
	this->_instance = data->_instance.get();
}

HV::V1::Image::~Image() {

}

HV::V1::Image::!Image() {

}


HV::V1::Image::Image(String^ name, unsigned int width, unsigned int height, unsigned int type, double resolution) {
	this->_instance = new hv::v1::image(msclr::interop::marshal_as<std::string>(name), width, height, type, resolution);
}
HV::V1::Image::Image(Image^ copy) {
	this->_instance = copy->_instance.get();
}

void HV::V1::Image::RegisterDrawObject(HV::V1::Object^ _object) {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get())->register_draw_object(_object->_instance.get());
}
List<HV::V1::Object^>^ HV::V1::Image::DrawObjects() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	auto list = gcnew List<HV::V1::Object^>();
	auto native_list = target->drarw_objects();
	for (auto& element : native_list) {
		list->Add(gcnew HV::V1::Object(element));
	}
	return list;
}



int HV::V1::Image::Width() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->width();
}
int HV::V1::Image::Height() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->height();
}
int HV::V1::Image::Size() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->size();
}
int HV::V1::Image::Stride() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->stride();
}
int HV::V1::Image::Count() {
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
double HV::V1::Image::Resolution() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());
	return target->resolution();
}

HV::V1::ImageDataType HV::V1::Image::PixelType() {
	auto target = std::dynamic_pointer_cast<hv::v1::image>(this->_instance.get());

	return static_cast<HV::V1::ImageDataType>(target->pixel_type());
}