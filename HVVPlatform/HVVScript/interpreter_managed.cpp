
#include "interpreter_managed.h"

#include "interpreter.h"

namespace hv::v1 {
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
	std::shared_ptr<pimpl_interpreter> pimpl_interpreter(new pimpl_interpreter());
	this->_pimpl = pimpl_interpreter;
}
hv::v1::interpreter_managed::~interpreter_managed() {

}

bool hv::v1::interpreter_managed::set_module_path(std::string path) {
	return this->_pimpl->ptr->set_module_path(path);
}
bool hv::v1::interpreter_managed::run_script(std::string content) {
	return this->_pimpl->ptr->run_script(content);
}
bool hv::v1::interpreter_managed::run_file(std::string path) {
	return this->_pimpl->ptr->run_file(path);
}
bool hv::v1::interpreter_managed::terminate() {
	return this->_pimpl->ptr->terminate();
}

bool hv::v1::interpreter_managed::register_external_data(std::string key, std::shared_ptr<hv::v1::object> data) {
	return this->_pimpl->ptr->register_external_object(key, data);
}
std::shared_ptr<hv::v1::object> hv::v1::interpreter_managed::external_data(std::string key) {
	return this->_pimpl->ptr->external_object(key);
}
bool hv::v1::interpreter_managed::check_external_data(std::string key) {
	return this->_pimpl->ptr->check_external_object(key);
}
void hv::v1::interpreter_managed::clear_external_data() {
	return this->_pimpl->ptr->clear_external_object();
}

std::list<std::string> hv::v1::interpreter_managed::global_names() {
	return this->_pimpl->ptr->global_names();
}
std::map<std::string, std::shared_ptr<hv::v1::object>> hv::v1::interpreter_managed::global_objects() {
	return this->_pimpl->ptr->global_objects();
}

std::list<std::string> hv::v1::interpreter_managed::external_names() {
	return this->_pimpl->ptr->external_names();
}
std::map<std::string, std::shared_ptr<hv::v1::object>> hv::v1::interpreter_managed::external_objects() {
	return this->_pimpl->ptr->external_objects();
}


std::function<void(std::string)>& hv::v1::interpreter_managed::set_trace_callback() {
	return this->_pimpl->ptr->set_trace_callback();
}
void hv::v1::interpreter_managed::reset_trace_callback() {
	this->_pimpl->ptr->reset_trace_callback();
}



bool hv::v1::interpreter_managed::init_v8_startup_data(std::string path) {
	return hv::v1::interpreter::init_v8_startup_data(path);
}
void hv::v1::interpreter_managed::init_v8_platform() {
	hv::v1::interpreter::init_v8_platform();
}
bool hv::v1::interpreter_managed::init_v8_engine() {
	return hv::v1::interpreter::init_v8_engine();
}
void hv::v1::interpreter_managed::set_v8_flag(std::string flag) {
	hv::v1::interpreter::set_v8_flag(flag);
}