#pragma once


namespace hv::v1 {


	enum image_data_type{
		u8c1_image=1,
		u8c3_image,
		u16c1_image,
		u16c3_image,
		u32c1_image,
		u32c3_image,
		u64c1_image,
		u64c3_image
	};



	namespace casting {
		enum casting_type {
			object = 0,
			number,
			boolean,
			string,
			image,
			point
		};
	};

}