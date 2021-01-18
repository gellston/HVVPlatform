#pragma once

#include <cstdlib> 

#include <ctime>
#include <random>
#include <random>

namespace hv::v1 {


	int rand(int _min, int _max) {
		
		std::random_device rd; 
		std::mt19937 mersenne(rd()); 
		std::uniform_int_distribution dist(_min, _max);

		return dist(mersenne);
	}
}