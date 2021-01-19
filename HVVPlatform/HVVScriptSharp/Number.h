#pragma once



#include "Object.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;



namespace HV {

	namespace V1 {
		public ref class Number : public HV::V1::Object
		{

		internal:

		public:
			Number(System::String^ Name, double data);
			Number(double data);
			Number(HV::V1::Object^ object);
			~Number();
			!Number();


			double Data();
			void Data(double data);

		};
	}

}
