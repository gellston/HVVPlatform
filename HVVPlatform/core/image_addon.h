#pragma once


#include <binding.h>

#include "image.h"


HV_PLUGIN_INIT(hv::v1::isolate * isolate)
{
	hv::v1::EscapeHandleScope scope(isolate);



	//hv::v1::class_<hv::v1::object> object_class(isolate);
	//object_class.ctor<std::string, std::string>()
	//			.set("name", &hv::v1::object::name)
	//			.set("type",&hv::v1::object::type);
	
	hv::v1::class_<hv::v1::image> image_class(isolate);
	image_class.ctor<std::string, unsigned int, unsigned int, hv::v1::image_data_type>()
		.set("to_string", &hv::v1::image::to_string)
		.set("stride", &hv::v1::image::stride)
		.set("count", &hv::v1::image::count)
		.set("fill", &hv::v1::image::fill)
		.set("copy", &hv::v1::image::copy)
		.set("reduce", &hv::v1::image::reduce)
		.set("multiply", &hv::v1::image::multiply)
		.set("divide", &hv::v1::image::divide)
		.set("add", &hv::v1::image::add)
		.set("minus", &hv::v1::image::minus);





	hv::v1::module m(isolate);
	m.set_const("u8_image", hv::v1::image_data_type::u8_image)
	 .set_const("u16_image", hv::v1::image_data_type::u16_image)
	 .set_const("u32_image", hv::v1::image_data_type::u32_image)
	 .set_const("u64_image", hv::v1::image_data_type::u64_image)
	 .set("image", image_class);

	


	return scope.Escape(m.new_instance());
}

