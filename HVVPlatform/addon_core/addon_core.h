#pragma once
#include <binding.h>

#include "image.h"
#include "point.h"
#include "rectROI.h"
#include "lineFitROI.h"
#include "circleFitROI.h"
#include "circleROI.h"

#include "_math.h"
#include "_system.h"

HV_CREATE_SHARED_CONVERTER(hv::v1::image);
HV_CREATE_SHARED_CONVERTER(hv::v1::point);
HV_CREATE_SHARED_CONVERTER(hv::v1::rectROI);
HV_CREATE_SHARED_CONVERTER(hv::v1::lineFitROI);
HV_CREATE_SHARED_CONVERTER(hv::v1::circleFitROI);
HV_CREATE_SHARED_CONVERTER(hv::v1::circleROI);

HV_PLUGIN_INIT(hv::v1::isolate* _isolate)
{

	hv::v1::EscapeHandleScope scope(_isolate);

	hv::v1::class_<hv::v1::image, hv::v1::shared_ptr_traits> image_class(_isolate);
	image_class.ctor<std::string, unsigned int, unsigned int, hv::v1::image_data_type, double>()
		.auto_wrap_objects(true)
		.inherit<hv::v1::object>()
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
		.set("x", &hv::v1::point::x)
		.set("y", &hv::v1::point::y);

	hv::v1::class_<hv::v1::rectROI, hv::v1::shared_ptr_traits> rect_roi_class(_isolate);
	rect_roi_class.ctor<std::string, double, double, double, double>()
		.auto_wrap_objects(true)
		.inherit<hv::v1::object>()
		.set("x", &hv::v1::rectROI::x)
		.set("y", &hv::v1::rectROI::y)
		.set("width", &hv::v1::rectROI::width)
		.set("height", &hv::v1::rectROI::height);

	hv::v1::class_<hv::v1::lineFitROI, hv::v1::shared_ptr_traits> line_roi_class(_isolate);
	line_roi_class.ctor<std::string, double, double, double, double, double, bool, bool>()
		.auto_wrap_objects()
		.inherit<hv::v1::object>()
		.set("x", &hv::v1::lineFitROI::x)
		.set("y", &hv::v1::lineFitROI::y)
		.set("angle", &hv::v1::lineFitROI::angle)
		.set("width", &hv::v1::lineFitROI::width)
		.set("height", &hv::v1::lineFitROI::height)
		.set("is_flip", &hv::v1::lineFitROI::is_flip)
		.set("is_black2white", &hv::v1::lineFitROI::is_black2white);

	hv::v1::class_<hv::v1::circleFitROI, hv::v1::shared_ptr_traits> circle_roi_class(_isolate);
	circle_roi_class.ctor<std::string, double, double, double, double, double, bool>()
		.auto_wrap_objects()
		.inherit<hv::v1::object>()
		.set("x", &hv::v1::circleFitROI::x)
		.set("y", &hv::v1::circleFitROI::y)
		.set("radius", &hv::v1::circleFitROI::radius)
		.set("startRatio", &hv::v1::circleFitROI::startRatio)
		.set("endRatio", &hv::v1::circleFitROI::endRatio)
		.set("is_black2white", &hv::v1::circleFitROI::is_black2white);


	hv::v1::class_<hv::v1::circleROI, hv::v1::shared_ptr_traits> circle_roi_class2(_isolate);
	circle_roi_class2.ctor<std::string, double, double, double>()
		.auto_wrap_objects()
		.inherit<hv::v1::object>()
		.set("x", &hv::v1::circleROI::x)
		.set("y", &hv::v1::circleROI::y)
		.set("radius", &hv::v1::circleROI::radius);

	hv::v1::module m(_isolate);
	m.set_const("u8c1_image", hv::v1::image_data_type::u8c1_image)
		.set_const("u16c1_image", hv::v1::image_data_type::u16c1_image)
		.set_const("u32c1_image", hv::v1::image_data_type::u32c1_image)
		.set_const("u64c1_image", hv::v1::image_data_type::u64c1_image)
		.set("image", image_class)
		.set("point", point_class)
		.set("rectROI", rect_roi_class)
		.set("lineFitROI", line_roi_class)
		.set("circleFitROI", circle_roi_class)
		.set("circleROI", circle_roi_class2)
		.set("to_image", &hv::v1::to_image)
		.set("to_point", &hv::v1::to_point)
		.set("to_rectROI", &hv::v1::to_rectROI)
		.set("to_lineFitROI", &hv::v1::to_lineFitROI)
		.set("to_circleFitROI", &hv::v1::to_circleFitROI)
		.set("to_circleROI", &hv::v1::to_circleROI)
		.set("sleep", _sleep)
		.set("round", hv::v1::round)
		.set("rand", hv::v1::rand);
		
	return scope.Escape(m.new_instance());
}