#pragma once


#include "Object.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;



namespace HV {

	namespace V1 {
		public ref class Point : public HV::V1::Object
		{

		internal:

		public:
			Point(System::String^ Name, double x, double y);
			Point(HV::V1::Object^ object);
			~Point();
			!Point();


			property double X{
				double get();
			};
			property double Y{
				double get();
			};
		};
	}

}
