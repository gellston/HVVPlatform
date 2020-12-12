
#define CATCH_CONFIG_MAIN  

#include <catch.hpp>


#include <object.h>
#include <primitive_object.h>


using namespace hv::v1;


TEST_CASE("boolean class test") {
	{
		hv::v1::boolean static_test1("test", false);
		REQUIRE(static_test1.data() == false);
		REQUIRE(static_test1.name() == "test");
		REQUIRE(static_test1.type() == "boolean");
		REQUIRE(static_test1.to_string() == "false");
	}
}

TEST_CASE("number class test") {
	{
		hv::v1::number static_test1("test", 1.5);
		REQUIRE(static_test1.data() == 1.5);
		REQUIRE(static_test1.name() == "test");
		REQUIRE(static_test1.type() == "number");
		REQUIRE(static_test1.to_string() == "1.500000");
	}
}

