
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
	auto local_variable = pimpl_solid_ptr->_local;
	//v8::HandleScope scope(isolate);
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
	auto local_variable = pimpl_solid_ptr->_local;
	//v8::HandleScope scope(isolate);
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
	auto local_variable = pimpl_solid_ptr->_local;
	//v8::HandleScope scope(isolate);
	auto u8string = v8pp::from_v8<std::string>(isolate, *local_variable);
	auto std_string = u8string_to_string(u8string);
	return  std_string;
}
bool hv::v1::is_string(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);

	return (*(pimpl_solid_ptr->_local))->IsString();
}




// array to native array converter template defition
bool hv::v1::is_array(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto local_variable = pimpl_solid_ptr->_local;

	bool check = (*(pimpl_solid_ptr->_local))->IsArray() && !(*local_variable).IsEmpty();
	return check;
}

// double
template<> std::vector<double> hv::v1::convert_to_array(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	auto local_variable = pimpl_solid_ptr->_local;

	try {
		auto v8ObjectValue = (*local_variable)->ToObject(isolate);
		auto data = v8pp::from_v8<std::vector<double>>(isolate, v8ObjectValue);
		if (data.size() == 0) throw std::runtime_error("array is empty");
		return data;
	}
	catch (std::exception e) {
		throw e;
	}
}
// std::string
template<> std::vector<std::string> hv::v1::convert_to_array(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	auto local_variable = pimpl_solid_ptr->_local;

	try {
		auto v8ObjectValue = (*local_variable)->ToObject(isolate);
		auto data = v8pp::from_v8<std::vector<std::string>>(isolate, v8ObjectValue);
		if (data.size() == 0) throw std::runtime_error("array is empty");
		for (int index = 0; index < data.size(); index++)
			data[index] = u8string_to_string(data[index]);
		return data;
	}
	catch (std::exception e) {
		throw e;
	}
}
// bool
template<> std::vector<bool> hv::v1::convert_to_array(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	auto local_variable = pimpl_solid_ptr->_local;

	try {
		auto v8ObjectValue = (*local_variable)->ToObject(isolate);
		auto data = v8pp::from_v8<std::vector<bool>>(isolate, v8ObjectValue);
		if (data.size() == 0) throw std::runtime_error("array is empty");
		return data;
	}
	catch (std::exception e) {
		throw e;
	}
}



// map 
bool hv::v1::is_map(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto local_variable = pimpl_solid_ptr->_local;
	auto isolate = pimpl_solid_ptr->_isolate;

	bool check = (*(pimpl_solid_ptr->_local))->IsMap() && !(*local_variable).IsEmpty();
	return check;
}

template<> std::map<std::string, double> hv::v1::convert_to_map(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	auto local_variable = pimpl_solid_ptr->_local;

	std::map<std::string, double> native_map;

	try {
		auto v8ObjectValue = (*local_variable)->ToObject(isolate);

		v8::HandleScope scope(isolate);
		v8::Local<v8::Context> context = isolate->GetCurrentContext();
		v8::Local<v8::Map>  v8Map = v8::Local<v8::Map>::Cast(v8ObjectValue);
		v8::Local<v8::Array> v8Array = v8Map->AsArray();

		unsigned int length = v8Array->Length();
		if (length == 0 || length % 2 == 1) throw std::runtime_error("Invalid map");
		
		for (unsigned int index = 0; index < length; index+=2) {
			auto key = v8Array->Get(context, index).ToLocalChecked();
			auto value = v8Array->Get(context, index + 1).ToLocalChecked();
			auto native_key = v8pp::from_v8<std::string>(isolate, key);
			auto native_value = v8pp::from_v8<double>(isolate, value);
			native_key = u8string_to_string(native_key);
			native_map[native_key] = native_value;
		}
		return native_map;
	}
	catch (std::exception e) {
		throw e;
	}
}

template<> std::map<std::string, bool> hv::v1::convert_to_map(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	auto local_variable = pimpl_solid_ptr->_local;

	std::map<std::string, bool> native_map;

	try {
		auto v8ObjectValue = (*local_variable)->ToObject(isolate);

		v8::HandleScope scope(isolate);
		v8::Local<v8::Context> context = isolate->GetCurrentContext();
		v8::Local<v8::Map>  v8Map = v8::Local<v8::Map>::Cast(v8ObjectValue);
		v8::Local<v8::Array> v8Array = v8Map->AsArray();

		unsigned int length = v8Array->Length();
		if (length == 0 || length % 2 == 1) throw std::runtime_error("Invalid map");

		for (unsigned int index = 0; index < length; index += 2) {
			auto key = v8Array->Get(context, index).ToLocalChecked();
			auto value = v8Array->Get(context, index + 1).ToLocalChecked();
			auto native_key = v8pp::from_v8<std::string>(isolate, key);
			auto native_value = v8pp::from_v8<bool>(isolate, value);
			native_key = u8string_to_string(native_key);
			native_map[native_key] = native_value;
		}
		return native_map;
	}
	catch (std::exception e) {
		throw e;
	}
}

template<> std::map<std::string, std::string> hv::v1::convert_to_map(std::shared_ptr<pimpl_local_var> local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	auto local_variable = pimpl_solid_ptr->_local;

	std::map<std::string, std::string> native_map;

	try {
		auto v8ObjectValue = (*local_variable)->ToObject(isolate);

		v8::HandleScope scope(isolate);
		v8::Local<v8::Context> context = isolate->GetCurrentContext();
		v8::Local<v8::Map>  v8Map = v8::Local<v8::Map>::Cast(v8ObjectValue);
		v8::Local<v8::Array> v8Array = v8Map->AsArray();

		unsigned int length = v8Array->Length();
		if (length == 0 || length % 2 == 1) throw std::runtime_error("Invalid map");

		for (unsigned int index = 0; index < length; index += 2) {
			auto key = v8Array->Get(context, index).ToLocalChecked();
			auto value = v8Array->Get(context, index + 1).ToLocalChecked();
			auto native_key = v8pp::from_v8<std::string>(isolate, key);
			auto native_value = v8pp::from_v8<std::string>(isolate, value);
			native_key = u8string_to_string(native_key);
			native_value = u8string_to_string(native_value);
			native_map[native_key] = native_value;
		}
		return native_map;
	}
	catch (std::exception e) {
		throw e;
	}
}