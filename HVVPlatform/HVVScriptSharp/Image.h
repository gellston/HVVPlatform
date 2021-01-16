#pragma once


#include <image.h>
#include <memory>


#include "mananged_shared_ptr.h"
#include "Object.h"
#include "DataType.h"

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;



namespace HV {

	namespace V1 {
		public ref class Image : public HV::V1::Object
		{

		internal:

		public:

			Image(HV::V1::Object^ object);
			~Image();
			!Image();

			Image(String^ name, unsigned int width, unsigned int height, unsigned int type, double resolution);
			Image(Image^ copy);


			void RegisterDrawObject(HV::V1::Object^ _object);
			List<HV::V1::Object^>^ DrawObjects();


	
			int Width();
			int Height();
			int Size();
			int Stride();
			int Count();
			IntPtr Ptr();
			bool Copy(Image^ data);
			bool Fill(double value);
			double Reduce();
			bool Multiply(double value);
			bool Divide(double value);
			bool Add(double value);
			bool Minus(double value);
			double Resolution();
			ImageDataType PixelType();
		};
	}

}
