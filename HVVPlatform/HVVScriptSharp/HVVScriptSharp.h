#pragma once


#include <interpreter_managed.h>

#include "Object.h"


using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;


namespace HV {
	namespace V1 {
		public ref class Interpreter
		{

		internal:
			HV::V1::mananged_shared_ptr<hv::v1::interpreter_managed> _instance;
			
		private:


		public:

			Interpreter();
			~Interpreter();
			!Interpreter();



			bool SetModulePath(String^ path);
			bool RunScript(String^ content);
			bool RunFile(String^ path);
			bool Terminate();
			

			bool RegisterExternalData(String^ key, HV::V1::Object^ data);
			HV::V1::Object^ ExternalData(String^ key);
			bool CheckExternalData(String^ key);
			void ClearExternalData();

			
			property List<String^>^ GlobalNames {
				List<String^>^ get();
			}
			property Dictionary<String^, HV::V1::Object^>^ GlobalObjects {
				Dictionary<String^, HV::V1::Object^>^ get();
			}

			property List<String^>^ ExternalNames{
				List<String^> ^ get();
			}
			property Dictionary<String^, HV::V1::Object^>^ ExternalObjects {
				Dictionary<String^, HV::V1::Object^>^ get();
			}


			/// <summary>
			/// script static functions
			/// </summary>
			static bool InitV8StartupData(String^ path);
			static void InitV8Platform();
			static bool InitV8Engine();
			static void SetV8Flag(String^ flag);
		};
	}

}
