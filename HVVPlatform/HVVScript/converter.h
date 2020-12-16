#pragma once


#ifndef HV_CONVERTER
#define HV_CONVERTER


#include <functional>
#include <string>
#include <variant>

#include "pimpl.h"


namespace hv::v1 {

	using ret_array_type = std::variant<std::monostate, std::vector<std::string>, std::vector<double>>;

	HVAPI_EXPORT bool convert_to_boolean(std::shared_ptr<pimpl_local_var> local_var);
	HVAPI_EXPORT bool is_boolean(std::shared_ptr<pimpl_local_var> local_var);

	HVAPI_EXPORT double convert_to_number(std::shared_ptr<pimpl_local_var> local_var);
	HVAPI_EXPORT bool is_number(std::shared_ptr<pimpl_local_var> local_var);

	HVAPI_EXPORT std::string convert_to_string(std::shared_ptr<pimpl_local_var> local_var);
	HVAPI_EXPORT bool is_string(std::shared_ptr<pimpl_local_var> local_var);

	HVAPI_EXPORT bool is_array(std::shared_ptr<pimpl_local_var> local_var);
	HVAPI_EXPORT ret_array_type convert_to_array(std::shared_ptr<pimpl_local_var> local_var);

}


namespace hv::v1 {

	class HVAPI_EXPORT converter {

	private:

		std::function<object* (std::shared_ptr<pimpl_local_var>)> _function;


	public:
		converter(std::function<object* (std::shared_ptr<pimpl_local_var>)> callback);
		object* operator()(std::shared_ptr<pimpl_local_var> local_variable);
	};

};

#endif