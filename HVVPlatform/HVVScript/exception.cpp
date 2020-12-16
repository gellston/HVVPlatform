#include "exception.h"

#include <string>
#include <stdexcept>

using namespace hv::v1;



script_error::script_error(const std::string message, int start_column, int end_column, int line) : std::runtime_error(message),
																									_start_column(start_column),
																									_end_column(end_column),
																									_line(line){

}

int script_error::line() {

	return this->_line;
}
int script_error::start_column() {

	return this->_start_column;
}
int script_error::end_column() {

	return this->_end_column;
}
