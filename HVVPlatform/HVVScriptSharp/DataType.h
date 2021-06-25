#pragma once


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;



namespace HV {

	namespace V1 {
		public enum class ImageDataType
		{
			u8c1_image = 1,
			u8c3_image,
			u16c1_image,
			u16c3_image,
			u32c1_image,
			u32c3_image,
			u64c1_image,
			u64c3_image
		};
	}

}
