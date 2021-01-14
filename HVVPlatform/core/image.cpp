
#include "image.h"


#include <stdexcept>
#include <execution>
#include <limits>
#include <valarray>

hv::v1::image::image(std::string name, unsigned int width, unsigned int height, unsigned int type, double resolution) : object(name, "image"),
																														_width(width),
																														_height(height),
																														_type((hv::v1::image_data_type)type),
																														_pixel_resolution(resolution){

	if (width == 0 || height == 0 || width < 0 || height < 0)
		throw std::runtime_error("size is not correct");

	unsigned int demension = width * height;

	switch (type) {
		case hv::v1::image_data_type::u8_image:{
			this->__data = std::make_shared<std::vector<unsigned char>>(std::vector<unsigned char>(demension));
			break;
		}
		case hv::v1::image_data_type::u16_image: {
			this->__data = std::make_shared<std::vector<unsigned short>>(std::vector<unsigned short>(demension));
			break;
		}
		case hv::v1::image_data_type::u32_image: {
			this->__data = std::make_shared<std::vector<unsigned long>>(std::vector<unsigned long>(demension));
			break;
		}
		case hv::v1::image_data_type::u64_image: {
			this->__data = std::make_shared<std::vector<unsigned  long long>>(std::vector<unsigned long long>(demension));
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
									_count(instance._count),
								    _pixel_resolution(instance._pixel_resolution) {
	
}


std::string hv::v1::image::to_string() {

	std::string temp = "";
	temp += this->type();
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

bool hv::v1::image::copy(hv::v1::image& data) {

	

	if (this->_width != data._width ||
		this->_height != data._height ||
		this->_count != data._count ||
		this->_type != data._type ||
		this->_size != data._size ||
		this->_stride != data._stride) return false;


	switch (this->_type) {
	case hv::v1::image_data_type::u8_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned char>>>(this->__data);
		auto target = std::get<std::shared_ptr<std::vector<unsigned char>>>(data.__data);
		std::copy(std::execution::par_unseq, source->begin(), source->end(), target->begin());
		break;
	}
	case hv::v1::image_data_type::u16_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned short>>>(this->__data);
		auto target = std::get<std::shared_ptr<std::vector<unsigned short>>>(data.__data);
		std::copy(std::execution::par_unseq, source->begin(), source->end(), target->begin());
		break;
	}
	case hv::v1::image_data_type::u32_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long>>>(this->__data);
		auto target = std::get<std::shared_ptr<std::vector<unsigned long>>>(data.__data);
		std::copy(std::execution::par_unseq, source->begin(), source->end(), target->begin());
		break;
	}
	case hv::v1::image_data_type::u64_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long long>>>(this->__data);
		auto target = std::get<std::shared_ptr<std::vector<unsigned long long>>>(data.__data);
		std::copy(std::execution::par_unseq, source->begin(), source->end(), target->begin());
		break;
	}
	default: return false; break;
	}

	

	return true;
}


bool hv::v1::image::fill(double value) {
	switch (this->_type) {
	case hv::v1::image_data_type::u8_image: {
		if (UCHAR_MAX < value || value < 0) return false;
		auto source = std::get<std::shared_ptr<std::vector<unsigned char>>>(this->__data);
		std::fill(std::execution::par_unseq, source->begin(), source->end(), static_cast<unsigned char>(value));
		break;
	}
	case hv::v1::image_data_type::u16_image: {
		if (USHRT_MAX < value || value < 0) return false;
		auto source = std::get<std::shared_ptr<std::vector<unsigned short>>>(this->__data);
		std::fill(std::execution::par_unseq, source->begin(), source->end(), static_cast<unsigned short>(value));
		break;
	}
	case hv::v1::image_data_type::u32_image: {
		if (ULONG_MAX < value || value < 0) return false;
		auto source = std::get<std::shared_ptr<std::vector<unsigned long>>>(this->__data);
		std::fill(std::execution::par_unseq, source->begin(), source->end(), static_cast<unsigned long>(value));
		break;
	}
	case hv::v1::image_data_type::u64_image: {
		if (ULLONG_MAX < value || value < 0) return false;
		auto source = std::get<std::shared_ptr<std::vector<unsigned long long>>>(this->__data);
		std::fill(std::execution::par_unseq, source->begin(), source->end(), static_cast<unsigned long long>(value));
		break;
	}
	default: return false; break;
	}

	return true;
}

double hv::v1::image::reduce() {
	switch (this->_type) {
	case hv::v1::image_data_type::u8_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned char>>>(this->__data);
		double sum = std::reduce(std::execution::par, source->begin(), source->end(), (double)0);
		return sum;
		break;
	}
	case hv::v1::image_data_type::u16_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned short>>>(this->__data);
		double sum = std::reduce(std::execution::par, source->begin(), source->end(), (double)0);
		return sum;
		break;
	}
	case hv::v1::image_data_type::u32_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long>>>(this->__data);
		double sum = std::reduce(std::execution::par, source->begin(), source->end(), (double)0);
		return sum;
		break;
	}
	case hv::v1::image_data_type::u64_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long long>>>(this->__data);
		double sum = std::reduce(std::execution::par, source->begin(), source->end(), (double)0);
		return sum;
		break;
	}
	default: return 0; break;
	}

	return 0;
}


bool hv::v1::image::multiply(double value) {
	switch (this->_type) {
	case hv::v1::image_data_type::u8_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned char>>>(this->__data);
		unsigned char* pointer = source->data();
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned char& element) {
			element = static_cast<unsigned char>(((double)element * value));
		});
		break;
	}
	case hv::v1::image_data_type::u16_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned short>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned short& element) {
			element = static_cast<unsigned short>(((double)element * value));
		});
		break;
	}
	case hv::v1::image_data_type::u32_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned long& element) {
			element = static_cast<unsigned long>(((double)element * value));
		});
		break;
	}
	case hv::v1::image_data_type::u64_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long long>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned long long& element) {
			element = static_cast<unsigned long long>(((double)element * value));
		});
		break;
	}
	default: return false; break;
	}

	return true;
}
bool hv::v1::image::divide(double value) {
	switch (this->_type) {
	case hv::v1::image_data_type::u8_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned char>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned char& element) {
			if (element == 0 || value == 0) return;
			element = static_cast<unsigned char>(((double)element / value));
		});
		break;
	}
	case hv::v1::image_data_type::u16_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned short>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned short& element) {
			if (element == 0 || value == 0) return;
			element = static_cast<unsigned short>(((double)element / value));
		});
		break;
	}
	case hv::v1::image_data_type::u32_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned long& element) {
			if (element == 0 || value == 0) return;
			element = static_cast<unsigned long>(((double)element / value));
		});
		break;
	}
	case hv::v1::image_data_type::u64_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long long>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned long long& element) {
			if (element == 0 || value == 0) return;
			element = static_cast<unsigned long long>(((double)element / value));
		});
		break;
	}
	default: return false; break;
	}

	return true;
}


bool hv::v1::image::minus(double value) {
	switch (this->_type) {
	case hv::v1::image_data_type::u8_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned char>>>(this->__data);
		unsigned char* pointer = source->data();
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned char& element) {
			element = static_cast<unsigned char>(((double)element - value));
			});
		break;
	}
	case hv::v1::image_data_type::u16_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned short>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned short& element) {
			element = static_cast<unsigned short>(((double)element - value));
			});
		break;
	}
	case hv::v1::image_data_type::u32_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned long& element) {
			element = static_cast<unsigned long>(((double)element - value));
			});
		break;
	}
	case hv::v1::image_data_type::u64_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long long>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned long long& element) {
			element = static_cast<unsigned long long>(((double)element - value));
			});
		break;
	}
	default: return false; break;
	}

	return true;
}

bool hv::v1::image::add(double value) {
	switch (this->_type) {
	case hv::v1::image_data_type::u8_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned char>>>(this->__data);
		unsigned char* pointer = source->data();
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned char& element) {
			element = static_cast<unsigned char>(((double)element + value));
			});
		break;
	}
	case hv::v1::image_data_type::u16_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned short>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned short& element) {
			element = static_cast<unsigned short>(((double)element + value));
			});
		break;
	}
	case hv::v1::image_data_type::u32_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned long& element) {
			element = static_cast<unsigned long>(((double)element + value));
			});
		break;
	}
	case hv::v1::image_data_type::u64_image: {
		auto source = std::get<std::shared_ptr<std::vector<unsigned long long>>>(this->__data);
		std::for_each(std::execution::par_unseq, source->begin(), source->end(), [&](unsigned long long& element) {
			element = static_cast<unsigned long long>(((double)element + value));
			});
		break;
	}
	default: return false; break;
	}

	return true;
}


double hv::v1::image::resolution() {

	return this->_pixel_resolution;
}

void hv::v1::image::register_draw_object(std::shared_ptr<hv::v1::object> _object) {
	this->_draw_objects.push_back(_object);
}

std::list<std::shared_ptr<hv::v1::object>> hv::v1::image::drarw_objects() {
	return this->_draw_objects;
}