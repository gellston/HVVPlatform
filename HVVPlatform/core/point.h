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

namespace hv::v1 {

	class point : public object {


	private:

		double _x;
		double _y;


	public:


		point(std::string name, double x, double y);
		explicit point(point& copy);
		~point() override { }


		double x();
		double y();
		std::string to_string() override;

	};
}

#endif // !HV_IMAGE
