#pragma once


#include <object.h>
#include <memory>


#include "managed_shared_ptr.h"



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
			Object(hv::v1::object*);
			Object(std::shared_ptr<hv::v1::object>& object);
			Object(System::String^ Name, System::String^ Type);
			Object(HV::V1::Object^ object);
			virtual ~Object();
			!Object();


			property System::String^ Name {
				System::String^ get();
			}
			property System::String^ Type {
				System::String^ get();
			}

			property System::String^ StackName {
				System::String^ get();
			}

			void SetStackName(System::String^ name);

			property virtual System::String^ ToString {
				System::String^ get();
			}

		};
	};

};
