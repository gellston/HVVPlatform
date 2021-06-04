#pragma once

#ifndef HV_CIRCLE_FIT_ROI
#define HV_CIRCLE_FIT_ROI

#include "object.h"
#include "data_type.h"

#include <string>
#include <variant>
#include <memory>
#include <vector>
#include <list>


#include <iostream>

namespace hv::v1 {

	class circleFitROI : public object {


	private:

		double _x;
		double _y;
		double _radius;
		double _startRatio;
		double _endRatio;
		bool _is_black2white;

	public:


		circleFitROI(std::string name, double _x, double _y, double _radius, double _startRatio, double _endRatio, bool _is_black2white);
		~circleFitROI() override { }


		double x();
		double y();
		double radius();
		double startRatio();
		double endRatio();
		bool is_black2white();

		std::string to_string() override;

	};

	static std::shared_ptr<hv::v1::circleFitROI> to_circleFitROI(std::shared_ptr<hv::v1::object> _data) {

		return std::static_pointer_cast<hv::v1::circleFitROI>(_data);
	}


}

#endif // !HV_IMAGE
