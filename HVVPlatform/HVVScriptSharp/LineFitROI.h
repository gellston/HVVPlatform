#pragma once


#include "Object.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;


namespace HV {

	namespace V1 {
		public ref class LineFitROI : public HV::V1::Object
		{

		internal:

		public:
			LineFitROI(std::shared_ptr<hv::v1::object>& object);
			LineFitROI(System::String^ Name, double x, double y, double angle, double width, double height, bool is_flip, bool is_black2white);
			LineFitROI(HV::V1::Object^ object);
			LineFitROI(hv::v1::object*);
			~LineFitROI();
			!LineFitROI();


			property double X {
				double get();
			};
			property double Y {
				double get();
			}

			property double Angle {
				double get();
			}

			property double Width {
				double get();
			};
			property double Height {
				double get();
			}

			property bool IsFlip {
				bool get();
			};
			property bool IsBlack2White {
				bool get();
			}
		};
	};
};
