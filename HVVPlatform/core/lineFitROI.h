#pragma once

#ifndef HV_LINE_FIT_ROI
#define HV_LINE_FIT_ROI

#include "object.h"
#include "data_type.h"

#include <string>
#include <variant>
#include <memory>
#include <vector>
#include <list>


#include <iostream>

namespace hv::v1 {

	class lineFitROI : public object {


	private:

		double _x;
		double _y;
		double _angle;
		double _width; 
		double _height;
		bool _is_flip;
		bool _is_black_white;

	public:


		lineFitROI(std::string name, double _x, double _y, double _angle, double _width, double _height, bool _is_flip, bool _is_black_white);
		~lineFitROI() override { }


		double x();
		double y();
		double angle();
		double width();
		double height();
		bool is_flip();
		bool is_black2white();
		std::string to_string() override;

	};

	static std::shared_ptr<hv::v1::lineFitROI> to_lineFitROI(std::shared_ptr<hv::v1::object> _data) {

		return std::static_pointer_cast<hv::v1::lineFitROI>(_data);
	}


}

#endif // !HV_IMAGE
