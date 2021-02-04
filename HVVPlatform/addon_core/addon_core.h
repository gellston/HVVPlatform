#pragma once


#include <binding.h>

#include "image.h"
#include "point.h"
#include "_math.h"
#include "_system.h"

HV_CREATE_SHARED_CONVERTER(hv::v1::image);
HV_CREATE_SHARED_CONVERTER(hv::v1::point);

//static v8::Local<v8::Value> error_test(std::string test) {
//
//
//	auto isolate = v8::Isolate::GetCurrent();
//	hv::v1::EscapeHandleScope handle_scope(isolate);
//
//	auto exception = v8pp::throw_ex(isolate, "test");
//
//	return handle_scope.Escape(exception);
//}

HV_PLUGIN_INIT(hv::v1::isolate* isolate)
{

	

	hv::v1::EscapeHandleScope scope(isolate);



	hv::v1::class_<hv::v1::image, hv::v1::shared_ptr_traits> image_class(isolate);
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
		


	hv::v1::class_<hv::v1::point, hv::v1::shared_ptr_traits> point_class(isolate);
	point_class.ctor<std::string, double, double>()
		.auto_wrap_objects(true)
		.inherit<hv::v1::object>()
		//.set("create", &hv::v1::point::create)
		.set("x", &hv::v1::point::x)
		.set("y", &hv::v1::point::y);


	hv::v1::module m(isolate);
	m.set_const("u8_image", hv::v1::image_data_type::u8_image)
		.set_const("u16_image", hv::v1::image_data_type::u16_image)
		.set_const("u32_image", hv::v1::image_data_type::u32_image)
		.set_const("u64_image", hv::v1::image_data_type::u64_image)
		.set("image", image_class)
		.set("point", point_class)
		.set("to_image", &hv::v1::to_image)
		.set("to_point", &hv::v1::to_point)
		.set("sleep", _sleep)
		.set("round", hv::v1::round)
		.set("rand", hv::v1::rand);
		

	return scope.Escape(m.new_instance());
}