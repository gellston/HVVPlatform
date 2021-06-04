#pragma once


#include "Object.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;


namespace HV {

	namespace V1 {
		public ref class CircleFitROI : public HV::V1::Object
		{

		internal:

		public:
			CircleFitROI(std::shared_ptr<hv::v1::object>& object);
			CircleFitROI(System::String^ Name, double x, double y, double radius, double startRatio, double endRatio,  bool is_black2white);
			CircleFitROI(HV::V1::Object^ object);
			CircleFitROI(hv::v1::object*);
			~CircleFitROI();
			!CircleFitROI();


			property double X {
				double get();
			};
			property double Y {
				double get();
			}

			property double Radius {
				double get();
			}

			property double StartRatio {
				double get();
			};
			property double EndRatio {
				double get();
			}

			property bool IsBlack2White {
				bool get();
			}
		};
	};
};
