#pragma once

#ifndef HV_IMAGE
#define HV_IMAGE

#include "object.h"
#include "data_type.h"

#include <string>
#include <variant>
#include <memory>
#include <vector>

namespace hv::v1 {
	class HVAPI_EXPORT image : object {
		using image_type = std::variant<std::shared_ptr<std::vector<unsigned char>>,
										std::shared_ptr<std::vector<unsigned long>>,
										std::shared_ptr<std::vector<unsigned short>>,
										std::shared_ptr<std::vector<unsigned long long>>>;

	private:

		unsigned int _width;
		unsigned int _height;
		unsigned int _size;
		unsigned int _count;
		unsigned int _stride;

		image_type __data;

		image_data_type _type;

	public:


		image(std::string name, unsigned int width, unsigned int height, unsigned int type);
		explicit image(image& copy);



		std::string to_string() override;
		unsigned int width();
		unsigned int height();
		unsigned int size();
		unsigned int stride();
		unsigned int count();
		void* ptr();


	};
}

#endif // !HV_IMAGE
