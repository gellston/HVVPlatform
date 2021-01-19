#pragma once


#include "mananged_shared_ptr.h"
#include "Object.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;



namespace HV {

	namespace V1 {
		public ref class Boolean : public HV::V1::Object
		{

		internal:


		public:

			Boolean(System::String^ Name, bool data);
			Boolean(bool data);
			Boolean(HV::V1::Object^ object);
			~Boolean();
			!Boolean();

		
			bool Data();
			void Data(bool data);

		};
	}

}
