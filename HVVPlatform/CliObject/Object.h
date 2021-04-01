#pragma once


#include <object.h>
#include <memory>


#include "mananged_shared_ptr.h"



using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;



namespace HV {

	namespace V1 {
		public ref class Object
		{
		private:


		public:
			HV::V1::mananged_shared_ptr<hv::v1::object> _instance;

			Object();
			Object(std::shared_ptr<hv::v1::object>& object);
			Object(System::String^ Name, System::String^ Type);
			Object(HV::V1::Object^ object);
		/*	~Object();
			!Object();*/

			property String^ Name {
				String^ get();
			}
			property String^ Type {
				String^ get();
			}

			property virtual String^ ToString {
				String^ get();
			}

		};
	};

};
