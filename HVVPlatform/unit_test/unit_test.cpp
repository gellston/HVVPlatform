
#define CATCH_CONFIG_MAIN  

#include <catch.hpp>

// Primitive
#include <object.h>
#include <primitive_object.h>
// User object
#include <image.h>
#include <point.h>
// interpreter
#include <interpreter.h>

using namespace hv::v1;

std::string current_directory()
{
	char buffer[MAX_PATH];
	GetModuleFileNameA(NULL, buffer, MAX_PATH);
	std::string::size_type pos = std::string(buffer).find_last_of("\\/");
	return std::string(buffer).substr(0, pos);
}


// Object test
TEST_CASE("object class test") {
	{
		hv::v1::object test("test", "test");
		REQUIRE(test.name() == "test");
		REQUIRE(test.to_string() == "object");
		REQUIRE(test.type() == "test");

		hv::v1::object test1("test1", "test1");
		REQUIRE(test1.name() == "test1");
		REQUIRE(test1.to_string() == "object");
		REQUIRE(test1.type() == "test1");
	}
}


// Primitive Object test
TEST_CASE("boolean class test") {
	{
		hv::v1::boolean test("test", false);
		REQUIRE(test.data() == false);
		REQUIRE(test.name() == "test");
		REQUIRE(test.type() == "boolean");
		REQUIRE(test.to_string() == "false");

		hv::v1::object* object = &test;
		REQUIRE(object->name() == "test");
		REQUIRE(object->type() == "boolean");
		REQUIRE(object->to_string() == "false");
	}
}

TEST_CASE("number class test") {
	{
		hv::v1::number test("test", 1.5);
		REQUIRE(test.data() == 1.5);
		REQUIRE(test.name() == "test");
		REQUIRE(test.type() == "number");
		REQUIRE(test.to_string() == "1.500000");

		hv::v1::object* object = &test;
		REQUIRE(object->name() == "test");
		REQUIRE(object->type() == "number");
		REQUIRE(object->to_string() == "1.500000");
	}
}

TEST_CASE("string class test") {
	{
		hv::v1::string test("test", "test");
		REQUIRE(test.data() == "test");
		REQUIRE(test.name() == "test");
		REQUIRE(test.type() == "string");
		REQUIRE(test.to_string() == "test");

		hv::v1::object* object = &test;
		REQUIRE(object->name() == "test");
		REQUIRE(object->type() == "string");
		REQUIRE(object->to_string() == "test");
	}
}



// Interpreter test
TEST_CASE("interpreter class test") {
	{
		hv::v1::interpreter interpreter;

		auto current_path = current_directory();

		hv::v1::interpreter::init_v8_startup_data(current_path + "\\");
		hv::v1::interpreter::init_v8_platform();
		hv::v1::interpreter::init_v8_engine();
		hv::v1::interpreter::set_v8_flag("--use_strict");
		hv::v1::interpreter::set_v8_flag("--max_old_space_size=8192");
		hv::v1::interpreter::set_v8_flag("--expose_gc");

		hv::v1::interpreter interpreter1;
		


		REQUIRE(interpreter1.global_names().size() == 0);
		REQUIRE(interpreter1.global_objects()->size() == 0);
		REQUIRE(interpreter1.external_objects()->size() == 0);
		REQUIRE(interpreter1.external_names().size() == 0);
		REQUIRE(interpreter1.native_modules()->size() == 0);
		interpreter1.release_native_modules();
		interpreter1.reset_trace_callback();

		
		
	}
}
