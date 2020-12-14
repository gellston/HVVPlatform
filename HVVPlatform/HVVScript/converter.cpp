
#include "converter.h"
#include "pimpl.h"
#include "pimpl_define.h"


#include "string_cvt.h"

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
	v8::HandleScope scope(isolate);
	auto v8boolValue = (*(pimpl_solid_ptr->_local))->ToBoolean(isolate);
	return v8pp::from_v8<bool>(isolate, v8boolValue);
}
bool hv::v1::is_boolean(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);

	return (*(pimpl_solid_ptr->_local))->IsBoolean();
}



double hv::v1::convert_to_number(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto v8NumberValue = (*(pimpl_solid_ptr->_local))->ToNumber(isolate);
	return v8pp::from_v8<double>(isolate, v8NumberValue);
}
bool hv::v1::is_number(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);

	return (*(pimpl_solid_ptr->_local))->IsNumber();
}



std::string hv::v1::convert_to_string(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto v8StringValue = (*(pimpl_solid_ptr->_local))->ToString(isolate);
	auto u8string = v8pp::from_v8<std::string>(isolate, v8StringValue);
	auto std_string = u8string_to_string(u8string);
	return  std_string;
}
bool hv::v1::is_string(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);

	return (*(pimpl_solid_ptr->_local))->IsString();
}



std::vector<double> hv::v1::convert_to_array(std::shared_ptr<pimpl_local_var> local_var) {

	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;

	try {
		v8::HandleScope scope(isolate);
		auto v8ObjectValue = (*(pimpl_solid_ptr->_local))->ToObject(isolate);
		auto array = v8pp::from_v8<std::vector<double>>(isolate, v8ObjectValue);
		return array;
	}
	catch (std::exception e) {
		throw e;
	}

}
bool hv::v1::is_array(std::shared_ptr<pimpl_local_var> local_var) {

	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	bool check = (*(pimpl_solid_ptr->_local))->IsArray() && !(*(pimpl_solid_ptr->_local)).IsEmpty();
	return check;
}
