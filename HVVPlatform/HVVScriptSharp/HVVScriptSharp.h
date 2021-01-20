#pragma once


#include <interpreter_managed.h>

#include "Object.h"

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;


namespace hv::v1 {
	class pimpl_hvvscript_casting_container;
};

namespace HV {
	namespace V1 {
		

		public delegate void DelegateTrace(System::String^ str);
		

		public ref class Interpreter
		{

		internal:
			
			HV::V1::mananged_shared_ptr<hv::v1::interpreter_managed> _instance;
			HV::V1::mananged_shared_ptr<hv::v1::pimpl_hvvscript_casting_container> _casting_pimpl;

		private:

			DelegateTrace^ EventTraceCallback;
			DelegateTrace^ EventTraceTriggerCallback;

			GCHandle Handle;
			IntPtr HandlePtr;


			void Trace(System::String^ data);

		

		public:

			Interpreter();
			~Interpreter();
			!Interpreter();



			bool SetModulePath(System::String^ path);
			bool RunScript(System::String^ content);
			bool RunFile(System::String^ path);
			bool Terminate();
			

			bool RegisterExternalData(System::String^ key, HV::V1::Object^ data);
			System::Object^ ExternalData(System::String^ key);
			bool CheckExternalData(System::String^ key);
			void ClearExternalData();

			
			property List<System::String^>^ GlobalNames {
				List<System::String^>^ get();
			}
			property Dictionary<System::String^, HV::V1::Object^>^ GlobalObjects {
				Dictionary<System::String^, HV::V1::Object^>^ get();
			}

			property List<System::String^>^ ExternalNames{
				List<System::String^> ^ get();
			}
			property Dictionary<System::String^, HV::V1::Object^>^ ExternalObjects {
				Dictionary<System::String^, HV::V1::Object^>^ get();
			}

			event DelegateTrace^ TraceEvent {
				void add(DelegateTrace^ _delegate) {
					EventTraceCallback = static_cast<DelegateTrace^> (Delegate::Combine(EventTraceCallback, _delegate));
					this->HandlePtr = Marshal::GetFunctionPointerForDelegate(EventTraceCallback);
					auto native_pointer =  static_cast<void(*)(char*)>(HandlePtr.ToPointer());
					this->_instance->set_trace_callback(native_pointer);
				}

				void remove(DelegateTrace^ _delegate) {
					EventTraceCallback = static_cast<DelegateTrace^> (Delegate::Remove(EventTraceCallback, _delegate));
					this->HandlePtr = Marshal::GetFunctionPointerForDelegate(EventTraceCallback);
					auto native_pointer = static_cast<void(*)(char*)>(HandlePtr.ToPointer());
					this->_instance->set_trace_callback(native_pointer);
				}

				void raise(System::String^ data) {
					if (EventTraceCallback != nullptr) {
						return EventTraceCallback->Invoke(data);
					}
				}
			}

			/// <summary>
			/// script static functions
			/// </summary>
			static bool InitV8StartupData(System::String^ path);
			static void InitV8Platform();
			static bool InitV8Engine();
			static void SetV8Flag(System::String^ flag);
		};
	}

}
