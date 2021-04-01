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
			Point(std::shared_ptr<hv::v1::object>& object);
			Point(System::String^ Name, double x, double y);
			Point(HV::V1::Object^ object);
			Point(hv::v1::object*);
			~Point();
			!Point();


			property double X {
				double get();
			};
			property double Y {
				double get();
			}
		};
	};
};
