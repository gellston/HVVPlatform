#pragma once
#pragma warning(disable : 4251)
#pragma warning(disable : 4275)

#ifndef HV_MACRO
#define HV_MACRO


#if HVVAPI
#define HVAPI_EXPORT __declspec(dllexport)
#define HVAPI_TEMPLATE_EXPORT
#else
#define HVAPI_EXPORT __declspec(dllimport)
#define HVAPI_TEMPLATE_EXPORT extern
#endif


#endif