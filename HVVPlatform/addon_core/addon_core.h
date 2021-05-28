#pragma once
#include <binding.h>

#include "image.h"
#include "point.h"
#include "rect.h"
#include "_math.h"
#include "_system.h"

HV_CREATE_SHARED_CONVERTER(hv::v1::image);
HV_CREATE_SHARED_CONVERTER(hv::v1::point);
HV_CREATE_SHARED_CONVERTER(hv::v1::rect);

HV_PLUGIN_INIT(hv::v1::isolate* _isolate)
{

	

	hv::v1::EscapeHandleScope scope(_isolate);



	hv::v1::class_<hv::v1::image, hv::v1::shared_ptr_traits> image_class(_isolate);
	image_class.ctor<std::string, unsigned int, unsigned int, hv::v1::image_data_type, double>()
		.auto_wrap_objects(true)
		.inherit<hv::v1::object>()
		//.set("create", &hv::v1::image::create)
		.set("stride", &hv::v1::image::stride)
		.set("count", &hv::v1::image::count)
		.set("fill", &hv::v1::image::fill)
		.set("copy", &hv::v1::image::copy)
		.set("reduce", &hv::v1::image::reduce)
		.set("multiply", &hv::v1::image::multiply)
		.set("divide", &hv::v1::image::divide)
		.set("add", &hv::v1::image::add)
		.set("minus", &hv::v1::image::minus)
		.set("resolution", &hv::v1::image::resolution)
		.set("register_draw_object", &hv::v1::image::register_draw_object);
		


	hv::v1::class_<hv::v1::point, hv::v1::shared_ptr_traits> point_class(_isolate);
	point_class.ctor<std::string, double, double>()
		.auto_wrap_objects(true)
		.inherit<hv::v1::object>()
		//.set("create", &hv::v1::point::create)
		.set("x", &hv::v1::point::x)
		.set("y", &hv::v1::point::y);


	hv::v1::class_<hv::v1::rect, hv::v1::shared_ptr_traits> rect_class(_isolate);
	rect_class.ctor<std::string, double, double, double, double>()
		.auto_wrap_objects(true)
		.inherit<hv::v1::object>()
		.set("x", &hv::v1::rect::x)
		.set("y", &hv::v1::rect::y)
		.set("width", &hv::v1::rect::width)
		.set("height", &hv::v1::rect::height);



	hv::v1::module m(_isolate);
	m.set_const("u8c1_image", hv::v1::image_data_type::u8c1_image)
		.set_const("u16c1_image", hv::v1::image_data_type::u16c1_image)
		.set_const("u32c1_image", hv::v1::image_data_type::u32c1_image)
		.set_const("u64c1_image", hv::v1::image_data_type::u64c1_image)
		.set("image", image_class)
		.set("point", point_class)
		.set("rect", rect_class)
		.set("to_image", &hv::v1::to_image)
		.set("to_point", &hv::v1::to_point)
		.set("to_rect", &hv::v1::to_rectt)
		.set("sleep", _sleep)
		.set("round", hv::v1::round)
		.set("rand", hv::v1::rand);
		

	return scope.Escape(m.new_instance());
}