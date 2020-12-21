#pragma once


#ifndef HV_IMAGE
#define HV_IMAGE

#include "object.h"

#include <string>

namespace hv::v1 {
	class HVAPI_EXPORT image : public object {

	public:
		image(std::string name, unsigned char * data, unsigned int size);
		image(unsigned char* data, unsigned int size);

	};
}

#endif // !HV_IMAGE
