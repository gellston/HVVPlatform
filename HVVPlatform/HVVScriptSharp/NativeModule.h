#pragma once



#include "native_module.hpp"


#include "managed_shared_ptr.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;



namespace HV {

	namespace V1 {
		public ref class NativeModule
		{
		private:

			System::String^ _file_path;
			IntPtr _handle;

		public:

			NativeModule(System::String^ fileName, IntPtr intPtr);

			property System::String^ FilePath{
				System::String^ get();
			}

			property IntPtr Handle {
				IntPtr get();
			}
		};
	};

};
