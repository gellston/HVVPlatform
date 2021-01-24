#pragma once

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;



namespace HV {

	namespace V1 {
		public ref class ScriptError :  System::Exception
		{

		internal:
			int _start_column;
			int _end_column;
			int _line;

		public:

			
			ScriptError(System::String^ context, int start_column, int end_column, int line);


			int StartColumn();
			int Line();
			int EndColumn();

		};
	}

}
