
#include "converter.h"
#include "pimpl.h"
#include "pimpl_define.h"
#include "string_cvt.h"
#include "binding.h"


using namespace hv;
using namespace v1;


HV_CREATE_SHARED_CONVERTER(object);

converter::converter(std::function<object* (std::shared_ptr<pimpl_local_var>)> _callback) : _function(_callback)
{
	//_function = _callback;
	// _function will initialized statically
}


object* hv::v1::converter::operator()(std::shared_ptr<pimpl_local_var> local_variable)
{
	return _function(local_variable);
}


bool hv::v1::convert_to_boolean(std::shared_ptr<pimpl_local_var> _local_var)
{

	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	auto v8boolValue = _local->ToBoolean(isolate);
	return v8pp::from_v8<bool>(isolate, v8boolValue);
}
bool hv::v1::is_boolean(std::shared_ptr<pimpl_local_var> _local_var) {

	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	return _local->IsBoolean();
}



double hv::v1::convert_to_number(std::shared_ptr<pimpl_local_var> _local_var) {

	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	auto v8NumberValue = _local->ToNumber(isolate);
	return v8pp::from_v8<double>(isolate, v8NumberValue);
}
bool hv::v1::is_number(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);


	return _local->IsNumber();
}



std::string hv::v1::convert_to_string(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);


	auto u8string = v8pp::from_v8<std::string>(isolate, _local);
	auto std_string = u8string_to_string(u8string);
	return  std_string;
}
bool hv::v1::is_string(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	return _local->IsString();
}




// array to native array converter template defition
bool hv::v1::is_array(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	bool check = _local->IsArray() && !_local.IsEmpty();
	return check;
}

// double
template<> std::vector<double> hv::v1::convert_to_array(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	try {
		auto v8ObjectValue = _local->ToObject(isolate);
		auto data = v8pp::from_v8<std::vector<double>>(isolate, v8ObjectValue);
		if (data.size() == 0) throw std::runtime_error("array is empty");
		return data;
	}
	catch (std::exception e) {
		throw e;
	}
}
// std::string
template<> std::vector<std::string> hv::v1::convert_to_array(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	try {
		auto v8ObjectValue = _local->ToObject(isolate);
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
template<> std::vector<bool> hv::v1::convert_to_array(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	try {
		auto v8ObjectValue = _local->ToObject(isolate);
		auto data = v8pp::from_v8<std::vector<bool>>(isolate, v8ObjectValue);
		if (data.size() == 0) throw std::runtime_error("array is empty");
		return data;
	}
	catch (std::exception e) {
		throw e;
	}
}



// map 
bool hv::v1::is_map(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	bool check = _local->IsMap() && !_local.IsEmpty();
	return check;
}

template<> std::map<std::string, double> hv::v1::convert_to_map(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	std::map<std::string, double> native_map;

	try {
		auto v8ObjectValue = _local->ToObject(isolate);

		//v8::HandleScope scope(isolate);
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

template<> std::map<std::string, bool> hv::v1::convert_to_map(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);


	std::map<std::string, bool> native_map;

	try {
		auto v8ObjectValue = _local->ToObject(isolate);

		///v8::HandleScope scope(isolate);
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

template<> std::map<std::string, std::string> hv::v1::convert_to_map(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	std::map<std::string, std::string> native_map;

	try {
		auto v8ObjectValue = _local->ToObject(isolate);

		///v8::HandleScope scope(isolate);
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


// object
template<> std::shared_ptr<object> hv::v1::convert_to_object(std::shared_ptr<pimpl_local_var> _local_var) {
	auto pimpl_solid_ptr = std::static_pointer_cast<pimpl_local_var_solid>(_local_var);
	auto isolate = pimpl_solid_ptr->_isolate;
	v8::HandleScope scope(isolate);
	auto _local = v8pp::to_local(isolate, pimpl_solid_ptr->_global);

	try {
		//auto v8ObjectValue = _local->ToObject(isolate);
		auto data = v8pp::from_v8<std::shared_ptr<object>>(isolate, _local);
		
		return data;
	}
	catch (std::exception e) {
		throw e;
	}
}