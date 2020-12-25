
#include "image.h"


#include <stdexcept>

hv::v1::image::image(std::string name, unsigned int width, unsigned int height, unsigned int type) : object(name, "image"),
																					_width(width),
																					_height(height),
																					_type((hv::v1::image_data_type)type){

	if (width == 0 || height == 0 || width < 0 || height < 0)
		throw std::runtime_error("size is not correct");

	unsigned int demension = width * height;

	switch (type) {
		case hv::v1::image_data_type::u8_image:{
			auto data = std::make_shared<std::vector<unsigned char>>();
			data->resize(demension);
			this->__data = data;
			break;
		}
		case hv::v1::image_data_type::u16_image: {
			auto data = std::make_shared<std::vector<unsigned short>>();
			data->resize(demension);
			this->__data = data;
			break;
		}
		case hv::v1::image_data_type::u32_image: {
			auto data = std::make_shared<std::vector<unsigned long>>();
			data->resize(demension);
			this->__data = data;
			break;
		}
		case hv::v1::image_data_type::u64_image: {
			auto data = std::make_shared<std::vector<unsigned long long>>();
			data->resize(demension);
			this->__data = data;
			break;
		}
		default:
			throw std::runtime_error("invaid type error");
			break;
	}

	this->_size = width * height * (unsigned int)type;
	this->_count = width * height;
	this->_stride = width* (unsigned int)type;
}


hv::v1::image::image(image& instance) : object(instance.name(), "image"),
									_width(instance._width),
									_height(instance._height),
									_type(instance._type),
									__data(instance.__data),
									_size(instance._size),
									_stride(instance._stride),
									_count(instance._count){
	
}


std::string hv::v1::image::to_string() {

	std::string temp = "";
	temp += "image:";
	temp += this->name();
	temp += ":[width:";
	temp += std::to_string(this->_width);
	temp += ",height:";
	temp += std::to_string(this->_height);
	temp += "]";

	return temp;
}

unsigned int hv::v1::image::width() {
	return this->_width;
}

unsigned int hv::v1::image::height() {
	return this->_height;
}

unsigned int hv::v1::image::size() {
	return this->_size;
}

void* hv::v1::image::ptr() {
	switch (this->_type) {
		case hv::v1::image_data_type::u8_image:{
			auto data = std::get<std::shared_ptr<std::vector<unsigned char>>>(this->__data);
			return data->data();
			break;
		}
		case hv::v1::image_data_type::u16_image: {
			auto data = std::get<std::shared_ptr<std::vector<unsigned short>>>(this->__data);
			return data->data();
			break;
		}
		case hv::v1::image_data_type::u32_image: {
			auto data = std::get<std::shared_ptr<std::vector<unsigned long>>>(this->__data);
			return data->data();
			break;
		}
		case hv::v1::image_data_type::u64_image: {
			auto data = std::get<std::shared_ptr<std::vector<unsigned long long>>>(this->__data);
			return data->data();
			break;
		}
	}

	return nullptr;
}


unsigned int hv::v1::image::stride() {
	return this->_stride;
}

unsigned int hv::v1::image::count() {
	return this->_count;
}