
#include "converter.h"
#include "pimpl.h"
#include "pimpl_define.h"

using namespace hv;
using namespace v1;



converter::converter(std::function<object* (std::shared_ptr<pimpl_local_var>)> _callback) : _function(_callback)
{
	//_function = _callback;
	// _function will initialized statically
}


object* hv::v1::converter::operator()(std::shared_ptr<pimpl_local_var> local_variable)
{
	return _function(local_variable);
}


bool hv::v1::convert_to_boolean(std::shared_ptr<pimpl_local_var> local_var)
{
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	
	return v8pp::from_v8<bool>(isolate, pimpl_solid_ptr->_local->ToBoolean(isolate));
}
bool hv::v1::is_boolean(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);

	return pimpl_solid_ptr->_local->IsBoolean();
}



double hv::v1::convert_to_number(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;

	return v8pp::from_v8<double>(isolate, pimpl_solid_ptr->_local->ToNumber(isolate));
}
bool hv::v1::is_number(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);

	return pimpl_solid_ptr->_local->IsNumber();
}



std::string hv::v1::convert_to_string(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;

	return v8pp::from_v8<std::string>(isolate, pimpl_solid_ptr->_local->ToString(isolate));
}
bool hv::v1::is_string(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);

	return pimpl_solid_ptr->_local->IsString();
}



std::vector<double> hv::v1::convert_to_array(std::shared_ptr<pimpl_local_var> local_var) {

	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;

	try {
		auto array = v8pp::from_v8<std::vector<double>>(isolate, pimpl_solid_ptr->_local->ToObject(isolate));
		return array;
	}
	catch (std::exception e) {
		throw e;
	}

}
bool hv::v1::is_array(std::shared_ptr<pimpl_local_var> local_var) {

	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	bool check = pimpl_solid_ptr->_local->IsArray() && !pimpl_solid_ptr->_local.IsEmpty();
	return check;
}
