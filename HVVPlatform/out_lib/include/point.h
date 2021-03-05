#pragma once

#ifndef HV_POINT
#define HV_POINT

#include "object.h"
#include "data_type.h"

#include <string>
#include <variant>
#include <memory>
#include <vector>
#include <list>


#include <iostream>

namespace hv::v1 {

	class point : public object {


	private:

		double _x;
		double _y;


	public:


		point(std::string name, double _x, double _y);
		~point() override { }


		double x();
		double y();
		std::string to_string() override;

	};

	static std::shared_ptr<hv::v1::point> to_point(std::shared_ptr<hv::v1::object> _data) {

		return std::static_pointer_cast<hv::v1::point>(_data);
	}
}

#endif // !HV_IMAGE
