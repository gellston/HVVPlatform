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
			hv::v1::interpreter_managed *_instance;
			void reset();
			
		private:
			Dictionary<String^, HV::V1::Object^>^ _GlobalObject;

		public:

			Interpreter();
			~Interpreter();
			!Interpreter();

			
			


			bool SetModulePath(String^ path);
			bool RunScript(String^ content);
			bool RunFile(String^ path);

			//bool register_external_data(std::string key, std::shared_ptr<object> data);
			//std::shared_ptr<object> external_data(std::string key);
			bool RegisterExternalData(String^ key, HV::V1::Object^ data);
			HV::V1::Object^ ExternalData(String^ key);
			bool CheckExternalData(String^ key);
			void ClearExternalData();


			List<String^>^ GlobalNames();
			property Dictionary<String^, HV::V1::Object^>^ GlobalObjects {
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
