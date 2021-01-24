

// Native Header
#include "Exception.h"


HV::V1::ScriptError::ScriptError(String^ message, int start_column, int _end_column, int line) : System::Exception(message) {
	this->_start_column = start_column;
	this->_end_column = _end_column;
	this->_line = line;

}


int HV::V1::ScriptError::Line() {
	return this->_line;
}

int HV::V1::ScriptError::StartColumn() {
	return this->_start_column;
}

int HV::V1::ScriptError::EndColumn() {
	return this->_end_column;
}