#pragma once


#include "Object.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;


namespace HV {

	namespace V1 {
		public ref class CircleROI : public HV::V1::Object
		{

		internal:

		public:
			CircleROI(std::shared_ptr<hv::v1::object>& object);
			CircleROI(System::String^ Name, double x, double y, double radius);
			CircleROI(HV::V1::Object^ object);
			CircleROI(hv::v1::object*);
			~CircleROI();
			!CircleROI();


			property double X {
				double get();
			};
			property double Y {
				double get();
			}

			property double Radius {
				double get();
			}

		};
	};
};
