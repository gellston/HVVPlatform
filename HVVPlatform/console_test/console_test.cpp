// console_test.cpp : 이 파일에는 'main' 함수가 포함됩니다. 거기서 프로그램 실행이 시작되고 종료됩니다.
//
//


#include <Windows.h>

#include <iostream>
#include <filesystem>

#include <interpreter.h>
#include <exception.h>


std::string current_directory()
{
    char buffer[MAX_PATH];
    GetModuleFileNameA(NULL, buffer, MAX_PATH);
    std::string::size_type pos = std::string(buffer).find_last_of("\\/");

    return std::string(buffer).substr(0, pos);
}


int main()
{
    auto current_path = current_directory();
    
    hv::v1::interpreter::init_v8_startup_data(current_path + "\\");
    hv::v1::interpreter::init_v8_platform();
    hv::v1::interpreter::init_v8_engine();
    hv::v1::interpreter::set_v8_flag("--use_strict");
    
    
    
    hv::v1::interpreter interpreter;


    while (true) {
        try {
            interpreter.run_file("C:\\Github\\HVVPlatform\\test_script\\script.js");
        }
        catch (hv::v1::script_error error) {
            std::cout << "=============================================" << std::endl;
            std::cout << "error message :" << std::endl;
            std::cout << error.what() << std::endl;
            std::cout << "error line : " << error.line() << std::endl;
            std::cout << "error start column : " << error.start_column() << std::endl;
            std::cout << "error end column : " << error.end_column() << std::endl;
            std::cout << "=============================================" << std::endl;
        }
    }


    
}
