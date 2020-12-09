#pragma once



#ifndef HV_PIMPL
#define HV_PIMPL

#include "macro.h"
#include "object.h"

namespace hv::v1 {
	class HVAPI_EXPORT pimpl{

	public:
		pimpl(){}
		~pimpl() {}
	};


	class HVAPI_EXPORT pimpl_local_var {
	public:
		pimpl_local_var() {}
		~pimpl_local_var() {}


		/*virtual std::shared_ptr<object> data() = 0;*/
		virtual std::string key() = 0;
	};
}


#endif // !HV_PIMPL
