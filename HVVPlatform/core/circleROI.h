#pragma once

#ifndef HV_CIRCLE_ROI
#define HV_CIRCLE_ROI

#include "object.h"
#include "data_type.h"

#include <string>
#include <variant>
#include <memory>
#include <vector>
#include <list>


#include <iostream>

namespace hv::v1 {

	class circleROI : public object {


	private:

		double _x;
		double _y;
		double _radius;

	public:


		circleROI(std::string name, double _x, double _y, double _radius);
		~circleROI() override { }


		double x();
		double y();
		double radius();

		std::string to_string() override;

	};

	static std::shared_ptr<hv::v1::circleROI> to_circleROI(std::shared_ptr<hv::v1::object> _data) {

		return std::static_pointer_cast<hv::v1::circleROI>(_data);
	}


}

#endif // !HV_IMAGE
