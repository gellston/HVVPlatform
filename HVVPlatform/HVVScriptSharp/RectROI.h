#pragma once


#include "Object.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;


namespace HV {

	namespace V1 {
		public ref class RectROI : public HV::V1::Object
		{

		internal:

		public:
			RectROI(std::shared_ptr<hv::v1::object>& object);
			RectROI(System::String^ Name, double x, double y, double width, double height);
			RectROI(HV::V1::Object^ object);
			RectROI(hv::v1::object*);
			~RectROI();
			!RectROI();


			property double X {
				double get();
			};
			property double Y {
				double get();
			}

			property double Width {
				double get();
			};
			property double Height {
				double get();
			}
		};
	};
};
