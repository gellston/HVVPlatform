#pragma once

#ifndef HV_IMAGE
#define HV_IMAGE

#include <v8.h>
#include <v8pp/config.hpp>
#include <v8pp/class.hpp>
#include <v8pp/module.hpp>

#include "object.h"
#include <string>
#include <memory>
#include <iostream>

using namespace v8;

namespace hv::v1 {
	class HVAPI_EXPORT image : object {
	private:

		double _width;
		double _height;
		double _depth;
		double _size;
		std::shared_ptr<unsigned char> __data;


	public:


		image(std::string name, double width, double height, double depth) : object(name, "image"),
																								_width(width),
																								_height(height),
																								_depth(depth){

			if (width == 0 || height == 0 || width < 0 || height < 0 || depth < 0 || depth == 0)
				throw std::runtime_error("size is not correct");

			double size = width * height * depth;
			__data = std::shared_ptr<unsigned char>(new unsigned char[(unsigned int)size], [](unsigned char * pointer)
			{
				delete[] pointer;
			});

			this->_width = width;
			this->_height = height;
			this->_depth = depth;
			this->_size = size;
		}


		std::string to_string() override {

			std::string temp = "";
			temp += "image:";
			temp += this->name();
			temp += ":[x:";
			temp += std::to_string(this->_width);
			temp += ",y:";
			temp += std::to_string(this->_height);
			temp += "]";
			
			return temp;
		}

		double width() {
			return this->_width;
		}

		double height() {
			return this->_height;
		}

		double size() {
			return this->_size;
		}

		void show(image& image) {
		
			std::cout << "test width : " << image.width() << std::endl;
			std::cout << "test height : " << image.height() << std::endl;
		}

	};
}


V8PP_PLUGIN_INIT(v8::Isolate* isolate)
{
	v8::EscapableHandleScope scope(isolate);

	v8pp::class_<hv::v1::image> image_class(isolate);
	image_class.ctor<std::string, double, double, double>()
				.set("to_string", &hv::v1::image::to_string)
				.set("width", v8pp::property(&hv::v1::image::height))
				.set("height", v8pp::property(&hv::v1::image::width))
				.set("show", &hv::v1::image::show);

	v8pp::module m(isolate);
		m.set("image", image_class);
	

	return scope.Escape(m.new_instance());
}

#endif // !HV_IMAGE
