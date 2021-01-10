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
    //interpreter2.set_module_path(current_path);

    while (true) {

        system("cls");

        std::shared_ptr<hv::v1::image> ptr_test(new hv::v1::image("test", 2020, 2020, hv::v1::image_data_type::u8_image));

        auto casted_object = std::static_pointer_cast<hv::v1::object>(ptr_test);


        interpreter1.register_external_object("test", ptr_test);

        try {
            auto start_time = std::chrono::high_resolution_clock::now();
            std::string current_module_path = current_path;
            std::string current_script_path = current_path;
            current_script_path += "\\";
            current_script_path += "script1.js";

            current_module_path += "\\module";


            interpreter1.run_file(current_script_path);

            auto global_objects = interpreter1.global_objects();

            auto end_time = std::chrono::high_resolution_clock::now();
            auto time = end_time - start_time;

            auto duration = time / std::chrono::milliseconds(1);

            std::cout << "takt time : " << duration << std::endl;
            std::cout << "=============================================" << std::endl;
            for (auto& element : global_objects)
            {
                std::cout << "Key : " << element.first << std::endl;
                std::cout << "Type : " << element.second->type() << std::endl;
                std::cout << "Data : " << element.second->to_string() << std::endl;
                std::cout << "Is Array check : " << (element.second->type().compare("array") == 0) << std::endl;

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
        //Sleep(300);
    }
   
    //while (true) {

    //    system("cls");
    //    
    //    std::shared_ptr<hv::v1::image> ptr_test(new hv::v1::image("test", 2020, 2020, hv::v1::image_data_type::u8_image));

    //    auto casted_object = std::static_pointer_cast<hv::v1::object>(ptr_test);


    //    interpreter2.register_external_object("test", ptr_test);

    //    try {
    //        auto start_time = std::chrono::high_resolution_clock::now();
    //        std::string current_module_path = current_path;
    //        std::string current_script_path = current_path;
    //        current_script_path += "\\";
    //        current_script_path += "script1.js";

    //        current_module_path += "\\module";

    //        
    //        interpreter2.run_file(current_script_path);

    //        auto global_objects = interpreter2.global_objects();

    //        auto end_time = std::chrono::high_resolution_clock::now();
    //        auto time = end_time - start_time;

    //        auto duration = time / std::chrono::milliseconds(1);

    //        //std::cout << "takt time : " << duration << std::endl;
    //        //std::cout << "=============================================" << std::endl;
    //        for (auto & element : global_objects)
    //        {
    //            std::cout << "Key : " << element.first << std::endl;
    //            std::cout << "Type : " << element.second->type() << std::endl;
    //            std::cout << "Data : " << element.second->to_string() << std::endl;
    //            std::cout << "Is Array check : " << (element.second->type().compare("array") == 0) << std::endl;

    //            std::cout << "---------------------------------------------" << std::endl;
    //        }
    //        std::cout << "=============================================" << std::endl;

    //    }
    //    catch (hv::v1::script_error error) {
    //        std::cout << "=============================================" << std::endl;
    //        std::cout << "error message :" << std::endl;
    //        std::cout << error.what() << std::endl;
    //        std::cout << "error line : " << error.line() << std::endl;
    //        std::cout << "error start column : " << error.start_column() << std::endl;
    //        std::cout << "error end column : " << error.end_column() << std::endl;
    //        std::cout << "=============================================" << std::endl;
    //    }
    //    Sleep(300);
    //}

}
