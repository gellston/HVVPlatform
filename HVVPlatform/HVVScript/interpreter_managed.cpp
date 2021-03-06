
#include "interpreter_managed.h"

#include "interpreter.h"
#include "exception.h"

namespace hv::v1 {
	class pimpl_interpreter;
	class pimpl_interpreter {
	public:
		pimpl_interpreter() {
			std::shared_ptr<hv::v1::interpreter> shared_interpreter(new hv::v1::interpreter());
			ptr = shared_interpreter;
		}

		~pimpl_interpreter() {

		}

		std::shared_ptr<hv::v1::interpreter> ptr;
	};
}



hv::v1::interpreter_managed::interpreter_managed(){
	std::shared_ptr<hv::v1::pimpl_interpreter> pimpl_interpreter(new hv::v1::pimpl_interpreter());
	this->_pimpl = pimpl_interpreter;
}
hv::v1::interpreter_managed::~interpreter_managed() {

}

bool hv::v1::interpreter_managed::set_module_path(std::string _path) {
	return this->_pimpl->ptr->set_module_path(_path);
}
bool hv::v1::interpreter_managed::run_script(std::string _content) {
	try {
		return this->_pimpl->ptr->run_script(_content);
	}
	catch (hv::v1::script_error error) {
		throw error;
	}
}
bool hv::v1::interpreter_managed::run_file(std::string _path) {
	try {
		return this->_pimpl->ptr->run_file(_path);
	}
	catch (hv::v1::script_error error) {
		throw error;
	}
}
bool hv::v1::interpreter_managed::terminate() {
	return this->_pimpl->ptr->terminate();
}

bool hv::v1::interpreter_managed::register_external_data(std::string _key, std::shared_ptr<hv::v1::object> _data) {
	return this->_pimpl->ptr->register_external_object(_key, _data);
}
std::shared_ptr<hv::v1::object> hv::v1::interpreter_managed::external_data(std::string _key) {
	return this->_pimpl->ptr->external_object(_key);
}
bool hv::v1::interpreter_managed::check_external_data(std::string _key) {
	return this->_pimpl->ptr->check_external_object(_key);
}
void hv::v1::interpreter_managed::clear_external_data() {
	return this->_pimpl->ptr->clear_external_object();
}

std::list<std::string> hv::v1::interpreter_managed::global_names() {
	return this->_pimpl->ptr->global_names();
}
std::shared_ptr<std::map<std::string, std::shared_ptr<hv::v1::object>>> hv::v1::interpreter_managed::global_objects() {
	return this->_pimpl->ptr->global_objects();
}

std::list<std::string> hv::v1::interpreter_managed::external_names() {
	return this->_pimpl->ptr->external_names();
}
std::shared_ptr<std::map<std::string, std::shared_ptr<hv::v1::object>>> hv::v1::interpreter_managed::external_objects() {
	return this->_pimpl->ptr->external_objects();
}


void hv::v1::interpreter_managed::set_trace_callback(std::function<void(char*)> _callback) {
	return this->_pimpl->ptr->set_trace_callback(_callback);
}
void hv::v1::interpreter_managed::reset_trace_callback() {
	this->_pimpl->ptr->reset_trace_callback();
}


std::shared_ptr<std::map<std::string, hv::v1::native_module>> hv::v1::interpreter_managed::native_modules() {
	return this->_pimpl->ptr->native_modules();
}

void hv::v1::interpreter_managed::release_native_modules() {
	this->_pimpl->ptr->release_native_modules();
}



bool hv::v1::interpreter_managed::init_v8_startup_data(std::string _path) {
	return hv::v1::interpreter::init_v8_startup_data(_path);
}
void hv::v1::interpreter_managed::init_v8_platform() {
	hv::v1::interpreter::init_v8_platform();
}
bool hv::v1::interpreter_managed::init_v8_engine() {
	return hv::v1::interpreter::init_v8_engine();
}
void hv::v1::interpreter_managed::set_v8_flag(std::string _flag) {
	hv::v1::interpreter::set_v8_flag(_flag);
}

