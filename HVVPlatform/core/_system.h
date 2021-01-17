#pragma once

#include <cstdlib> 

#include <ctime>

namespace hv::v1 {

	int rand() {
		std::srand(static_cast<unsigned int>(std::time(0)));
		return std::rand();
	}
}