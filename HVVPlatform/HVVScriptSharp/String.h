#pragma once






#include "Object.h"



using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;



namespace HV {

	namespace V1 {
		public ref class String : public HV::V1::Object
		{

		internal:

		public:
			String(std::shared_ptr<hv::v1::object>& object);
			String(System::String^ Name, System::String^ data);
			String(System::String^ data);
			String(HV::V1::Object^ object);
			~String();
			!String();


			System::String^ Data();
			void Data(System::String^ data);

		};
	}

}
