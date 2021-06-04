#pragma once

#ifndef HV_RECT_ROI
#define HV_RECT_ROI

#include "object.h"
#include "data_type.h"

#include <string>
#include <variant>
#include <memory>
#include <vector>
#include <list>


#include <iostream>

namespace hv::v1 {

	class rectROI : public object {


	private:

		double _x;
		double _y;
		double _width;
		double _height;

	public:


		rectROI(std::string name, double _x, double _y, double _width, double _height);
		~rectROI() override { }


		double x();
		double y();
		double width();
		double height();
		std::string to_string() override;

	};

	static std::shared_ptr<hv::v1::rectROI> to_rectROI(std::shared_ptr<hv::v1::object> _data) {

		return std::static_pointer_cast<hv::v1::rectROI>(_data);
	}


}

#endif // !HV_IMAGE
