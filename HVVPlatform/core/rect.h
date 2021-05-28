#pragma once

#ifndef HV_RECT
#define HV_RECT

#include "object.h"
#include "data_type.h"

#include <string>
#include <variant>
#include <memory>
#include <vector>
#include <list>


#include <iostream>

namespace hv::v1 {

	class rect : public object {


	private:

		double _x;
		double _y;
		double _width;
		double _height;

	public:


		rect(std::string name, double _x, double _y, double _width, double _height);
		~rect() override { }


		double x();
		double y();
		double width();
		double height();
		std::string to_string() override;

	};

	static std::shared_ptr<hv::v1::rect> to_rectt(std::shared_ptr<hv::v1::object> _data) {

		return std::static_pointer_cast<hv::v1::rect>(_data);
	}


}

#endif // !HV_IMAGE
