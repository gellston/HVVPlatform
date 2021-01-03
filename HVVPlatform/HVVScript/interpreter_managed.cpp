
#include "interpreter_managed.h"

#include "interpreter.h"

namespace hv::v1 {
	class pimpl_interpreter {
	public:
		pimpl_interpreter() : ptr(std::make_shared<hv::v1::interpreter>()) {

		}

		~pimpl_interpreter() {

		}

		std::shared_ptr<hv::v1::interpreter> ptr;
	};
}



hv::v1::interpreter_managed::interpreter_managed() : _pimpl(std::make_shared<hv::v1::pimpl_interpreter>()){

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

bool hv::v1::interpreter_managed::register_external_data(std::string key, std::shared_ptr<hv::v1::object> data) {
	return this->_pimpl->ptr->register_external_data(key, data);
}
std::shared_ptr<hv::v1::object> hv::v1::interpreter_managed::external_data(std::string key) {
	return this->_pimpl->ptr->external_data(key);
}
bool hv::v1::interpreter_managed::check_external_data(std::string key) {
	return this->_pimpl->ptr->check_external_data(key);
}
void hv::v1::interpreter_managed::clear_external_data() {
	return this->_pimpl->ptr->clear_external_data();
}

std::list<std::string> hv::v1::interpreter_managed::global_names() {
	return this->_pimpl->ptr->global_names();
}
std::map<std::string, std::shared_ptr<hv::v1::object>>* hv::v1::interpreter_managed::global_objects() {
	return this->_pimpl->ptr->global_objects();
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