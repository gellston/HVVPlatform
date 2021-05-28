#pragma once


#include "Object.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;


namespace HV {

	namespace V1 {
		public ref class Rect : public HV::V1::Object
		{

		internal:

		public:
			Rect(std::shared_ptr<hv::v1::object>& object);
			Rect(System::String^ Name, double x, double y, double width, double height);
			Rect(HV::V1::Object^ object);
			Rect(hv::v1::object*);
			~Rect();
			!Rect();


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
