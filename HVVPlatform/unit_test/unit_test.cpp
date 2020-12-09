
#define BOOST_TEST_MODULE hvv_script_test
#define BOOST_TEST_SHOW_PROGRESS

#include <boost/test/included/unit_test.hpp>

#include <object.h>
#include <primitive_object.h>


using namespace hv::v1;

BOOST_AUTO_TEST_SUITE(hvv_script_test)

BOOST_AUTO_TEST_CASE(native_boolean_test)
{
	{
		hv::v1::boolean static_test1("test", false);

		BOOST_CHECK(static_test1.data() == false);
		BOOST_CHECK(static_test1.name() == "test");
		BOOST_CHECK(static_test1.type() == "boolean");
		BOOST_CHECK(static_test1.to_string() == "false");
	}
}

BOOST_AUTO_TEST_CASE(native_number_test)
{
	{
		hv::v1::number static_test1("test", 1.5);

		BOOST_CHECK(static_test1.data() == 1.5);
		BOOST_CHECK(static_test1.name() == "test");
		BOOST_CHECK(static_test1.type() == "number");
		BOOST_CHECK(static_test1.to_string() == "1.500000");
	}
}

BOOST_AUTO_TEST_SUITE_END()