// console_test.cpp : 이 파일에는 'main' 함수가 포함됩니다. 거기서 프로그램 실행이 시작되고 종료됩니다.
//
//
#include <Windows.h>


#include <iostream>
#include <filesystem>
#include <chrono>
#include <execution>
#include <memory>
#include <array>
#include <any>

#include <interpreter.h>
#include <exception.h>
#include <primitive_object.h>

#include <image.h>


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
    hv::v1::interpreter::set_v8_flag("--max_old_space_size=8192");
    hv::v1::interpreter::set_v8_flag("--expose_gc");

    hv::v1::interpreter interpreter1;
    //hv::v1::interpreter interpreter2;

    interpreter1.set_module_path(current_path);
 
    while (true) {
        try {
            interpreter1.run_script("var test = 2");
            auto names = interpreter1.global_names();
        }
        catch (hv::v1::script_error e) {
            std::cout << e.what() << std::endl;
        }
        

      
    }

}
