#pragma once


#ifndef HV_BINDING
#define HV_BINDING



#include <v8.h>
#include <v8pp/config.hpp>
#include <v8pp/class.hpp>
#include <v8pp/module.hpp>
#include <v8pp/class.hpp>
#include <v8pp/property.hpp>
#include <v8pp/throw_ex.hpp>


#define HV_PLUGIN_INIT V8PP_PLUGIN_INIT 


#define HV_CREATE_SHARED_CONVERTER(klass) \
namespace v8pp { \
template<> \
struct convert<std::shared_ptr<##klass>, typename std::enable_if<is_wrapped_class<##klass>::value>::type> \
{ \
	using from_type = std::shared_ptr<##klass>; \
	using to_type = v8::Local<v8::Object>; \
	using class_type = typename std::remove_cv<##klass>::type; \
	static bool is_valid(v8::Isolate*, v8::Local<v8::Value> value) \
	{ \
		return !value.IsEmpty() && value->IsObject(); \
	} \
	static from_type from_v8(v8::Isolate* isolate, v8::Local<v8::Value> value) \
	{ \
		if (!is_valid(isolate, value)) \
		{ \
			return nullptr; \
		} \
		return class_<class_type, shared_ptr_traits>::unwrap_object(isolate, value); \
	} \
	static to_type to_v8(v8::Isolate* isolate, std::shared_ptr<##klass> const& value) \
	{ \
        auto& class_info = detail::classes::find<shared_ptr_traits>(isolate, detail::type_id<class_type>()); \
		auto wrapped_obj = class_<class_type, shared_ptr_traits>::find_object(isolate, value); \
        if (wrapped_obj.IsEmpty() && class_info.auto_wrap_objects()) { \
            wrapped_obj = class_<class_type, shared_ptr_traits>::reference_external(isolate, value); \
        } \
        return wrapped_obj; \
	} \
}; \
}\



namespace hv::v1 {

	

	using EscapeHandleScope = v8::EscapableHandleScope;

	using HandleScope = v8::HandleScope;

	using  Local = v8::Local<v8::Value>;

	using  Object = v8::Local<v8::Object>;

	using raw_ptr_trait = v8pp::raw_ptr_traits;

	using shared_ptr_traits = v8pp::shared_ptr_traits;


	template<typename T, typename Traits = raw_ptr_trait> using class_ = v8pp::class_<T, Traits>;
	using module = v8pp::module;

	template<typename Get, typename Set>
	v8pp::property_<Get, Set> property(Get get, Set set)
	{
		return v8pp::property_<Get, Set>(get, set);
	}


	template<typename Get>
	v8pp::property_<Get, Get> property(Get get)
	{
		return v8pp::property_<Get, Get>(get);
	}

	using isolate = v8::Isolate;

	static Local throw_ex(hv::v1::isolate* isolate, std::string const& str) {
		return hv::v1::throw_ex(isolate, str);
	}


};

#endif