// console_test.cpp : 이 파일에는 'main' 함수가 포함됩니다. 거기서 프로그램 실행이 시작되고 종료됩니다.
//
//


#include <Windows.h>

#include <iostream>
#include <filesystem>
#include <chrono>



#include <interpreter.h>
#include <exception.h>
#include <primitive_object.h>


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
    
    hv::v1::interpreter interpreter;


    while (true) {
        system("cls");

        try {
            auto start_time = std::chrono::high_resolution_clock::now();
            
            std::string current_script_path = current_path;
            current_script_path += "\\";
            current_script_path += "script.js";
            interpreter.run_file(current_script_path);

            auto global_objects = interpreter.global_objects();

            auto end_time = std::chrono::high_resolution_clock::now();
            auto time = end_time - start_time;

            auto duration = time / std::chrono::milliseconds(1);

            std::cout << "takt time : " << duration << std::endl;
            std::cout << "=============================================" << std::endl;
            for (auto element : *global_objects)
            {
                std::cout << "Key : " << element.first << std::endl;
                std::cout << "Type : " << element.second->type() << std::endl;
                std::cout << "Data : " << element.second->to_string() << std::endl;
                std::cout << "Is Array check : " << (element.second->type().compare("array_number") == 0) << std::endl;
                if (element.second->type().compare("array_number") == 0) {
                    std::cout << "array data = [";
                    auto element_native = element.second;
                    auto casted_element = std::static_pointer_cast<hv::v1::array_number>(element_native);
                    auto pure_data = casted_element->data().get();
                    for (unsigned int index = 0; index < casted_element->size(); index++) {
                        std::cout << pure_data[index] << ",";
                    }
                    std::cout << "]" << std::endl;
                }
                std::cout << "---------------------------------------------" << std::endl;
            }
            std::cout << "=============================================" << std::endl;
            
            
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
        Sleep(1000);
    }

}
