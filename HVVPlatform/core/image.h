#pragma once

#ifndef HV_IMAGE
#define HV_IMAGE

#include "object.h"
#include "data_type.h"

#include <string>
#include <variant>
#include <memory>
#include <vector>
#include <list>

namespace hv::v1 {

	class image : public object {
		using image_type = std::variant<std::shared_ptr<std::vector<unsigned char>>,
										std::shared_ptr<std::vector<unsigned long>>,
										std::shared_ptr<std::vector<unsigned short>>,
										std::shared_ptr<std::vector<unsigned long long>>>;

	private:

		unsigned int _width;
		unsigned int _height;
		unsigned int _size;
		unsigned int _count;
		unsigned int _stride;
		double _pixel_resolution;

		image_type __data;

		image_data_type _type;


		std::vector<std::shared_ptr<hv::v1::object>> _draw_objects;

	public:


		image(std::string name, unsigned int width, unsigned int height, unsigned int type, double resolution = 1);
		explicit image(image& copy);
		~image() override { this->_draw_objects.clear(); }

		void register_draw_object(std::shared_ptr<hv::v1::object> _object);
		std::vector<std::shared_ptr<hv::v1::object>> drarw_objects();


		std::string to_string() override;
		unsigned int width();
		unsigned int height();
		unsigned int size();
		unsigned int stride();
		unsigned int count();
		void* ptr();
		bool copy(image& data);
		bool fill(double value);
		double reduce();
		bool multiply(double value);
		bool divide(double value);
		bool add(double value);
		bool minus(double value);
		double resolution();
		image_data_type pixel_type();

	};

	static std::shared_ptr<hv::v1::image> to_image(std::shared_ptr<hv::v1::object> data) {
		
		return std::static_pointer_cast<hv::v1::image>(data);
	}

}

#endif // !HV_IMAGE
