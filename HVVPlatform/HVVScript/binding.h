#pragma once


#ifndef HV_BINDING
#define HV_BINDING



#include <v8.h>
#include <v8pp/config.hpp>
#include <v8pp/class.hpp>
#include <v8pp/module.hpp>
#include <v8pp/class.hpp>
#include <v8pp/property.hpp>


namespace hv::v1 {
	using EscapeHandleScope = v8::EscapableHandleScope;
	using raw_ptr_trait = v8pp::raw_ptr_traits;


	template<typename T, typename Traits = raw_ptr_trait> using class_ = v8pp::class_<T, Traits>;
	using module = v8pp::module;


	template<typename Get, typename Set> using property_ = v8pp::property_<Get, Set>;
	template<typename Get> using property__ = v8pp::property_<Get, Get>;

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

};

#endif