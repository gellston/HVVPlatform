#pragma once


#include <image.h>
#include <opencv2/opencv.hpp>
#include <binding.h>
#include <exception.h>

HV_CREATE_SHARED_CONVERTER(hv::v1::image);


class opencv4 {

public :


	static std::shared_ptr<hv::v1::image> imread(std::string _path, bool _is_color) {

		try {
			cv::Mat cv_image = cv::imread(_path, _is_color);

			hv::v1::image_data_type data_type;
			unsigned int long demension = 0;

			switch (cv_image.type())
			{
			case CV_8UC1:
				data_type = hv::v1::image_data_type::u8c1_image;
				break;
			case CV_16UC1:
				data_type = hv::v1::image_data_type::u16c1_image;
				break;
			case CV_32FC1:
				data_type = hv::v1::image_data_type::u32c1_image;
				break;
			case CV_64FC1:
				data_type = hv::v1::image_data_type::u64c1_image;
				break;
			default:
				throw std::runtime_error("Not supported image type!!!");
				break;
			}

			std::shared_ptr<hv::v1::image> native_image(new hv::v1::image("", cv_image.cols, cv_image.rows, data_type, 1));
			std::memcpy(native_image->ptr(), cv_image.data, cv_image.cols * cv_image.rows * cv_image.channels());

			return native_image;
		}
		catch (std::exception e) {
			throw e;
		}

		
	}


};