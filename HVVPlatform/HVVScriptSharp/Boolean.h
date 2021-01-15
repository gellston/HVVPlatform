#pragma once


#include <primitive_object.h>
#include <memory>


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

			HV::V1::mananged_shared_ptr<hv::v1::boolean> _instance;

		public:
			Boolean(std::shared_ptr<hv::v1::object>& object);
			Boolean(System::String^ Name, bool data);
			Boolean(bool data);
			~Boolean();
			!Boolean();

			bool Data();
			void Data(bool data);

		};
	}

}
