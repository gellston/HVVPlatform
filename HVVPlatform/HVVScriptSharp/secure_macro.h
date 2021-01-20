#pragma once

#ifndef HV_MANAGED_SECURE_MACRO
#define HV_MANAGED_SECURE_MACRO


#define register_casting_type(x) this->_converter[#x]= hv::v1::casting::casting_type::##x; 


#endif