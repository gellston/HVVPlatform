#pragma once

#ifndef HV_EXCEPTION
#define HV_EXCEPTION

#include <stdexcept>
#include <string>
#include "macro.h"



namespace hv::v1 {

    

    class HVAPI_EXPORT script_error : public std::runtime_error {

    private:
        int _line;
        int _start_column;
        int _end_column;

    public:

        script_error(const std::string message, int start_column, int end_column, int line);
  
        int line();
        int start_column();
        int end_column();
    };

}


#endif
