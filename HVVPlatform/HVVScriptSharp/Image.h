#pragma once


#include "Object.h"
#include "DataType.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;


namespace hv::v1 {
	class pimpl_image_casting_container;
};

namespace HV {

	namespace V1 {
		public ref class Image : public HV::V1::Object
		{

		internal:

			HV::V1::mananged_shared_ptr<hv::v1::pimpl_image_casting_container> _casting_pimpl;

		public:

			Image(std::shared_ptr<hv::v1::object>& object);
			Image(HV::V1::Object^ object);
			~Image();
			!Image();

			Image(String^ name, unsigned int width, unsigned int height, unsigned int type, double resolution);
			Image(Image^ copy);


			void RegisterDrawObject(HV::V1::Object^ _object);
			property List<HV::V1::Object^>^ DrawObjects{
				List<HV::V1::Object^>^ get();
			};


	
			property int Width {
				int get();
			};
			property int Height {
				int get();
			};;
			property int Size {
				int get();
			};;
			property int Stride {
				int get();
			};;
			property int Count {
				int get();
			};;
			IntPtr Ptr();
			bool Copy(Image^ data);
			bool Fill(double value);
			double Reduce();
			bool Multiply(double value);
			bool Divide(double value);
			bool Add(double value);
			bool Minus(double value);
			property double Resolution {
				double get();
			};;
			property ImageDataType PixelType {
				ImageDataType get();
			};
		};
	}

}
