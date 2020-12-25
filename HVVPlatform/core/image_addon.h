#pragma once


#include <v8.h>
#include <v8pp/config.hpp>
#include <v8pp/class.hpp>
#include <v8pp/module.hpp>

#include "image.h"


V8PP_PLUGIN_INIT(v8::Isolate* isolate)
{
	v8::EscapableHandleScope scope(isolate);

	v8pp::class_<hv::v1::image> image_class(isolate);
	image_class.ctor<std::string, unsigned int, unsigned int, hv::v1::image_data_type>()
		.set("to_string", &hv::v1::image::to_string)
		.set("width", v8pp::property(&hv::v1::image::height))
		.set("height", v8pp::property(&hv::v1::image::width))
		.set("stride", &hv::v1::image::stride)
		.set("count", &hv::v1::image::count);



	v8pp::module m(isolate);
	m.set_const("u8_image", hv::v1::image_data_type::u8_image)
	 .set_const("u16_image", hv::v1::image_data_type::u16_image)
	 .set_const("u32_image", hv::v1::image_data_type::u32_image)
	 .set_const("u64_image", hv::v1::image_data_type::u64_image)
	 .set("image", image_class);

	


	return scope.Escape(m.new_instance());
}


