#pragma once

#ifndef HV_INTERPRETER
#define HV_INTERPRETER


#include <mutex>
#include <thread>
#include <string>
#include <map>
#include <functional>



#include "pimpl.h"
#include "converter.h"
#include "macro.h"
#include "native_module.hpp"

namespace hv::v1{

	class HVAPI_EXPORT interpreter {

		

	private:

		
		/// <summary>
		/// pimpl v8 isolate 
		/// </summary>
		std::shared_ptr<pimpl> _isolate; 


		/// <summary>
		/// pimpl v8 platform
		/// </summary>
		static std::shared_ptr<pimpl> _platform_instance;


		/// <summary>
		/// pimpl object hash
		/// </summary>
		//std::shared_ptr<pimpl> _global_object_hash;


		/// <summary>
		/// pimpl converter lambda
		/// </summary>
		//std::shared_ptr<pimpl> _converter_lambda;
		std::map<std::string, std::shared_ptr<converter>> _converter_lambda;

		std::shared_ptr <std::map<std::string, std::shared_ptr<object>>> _external_hash_map;

		std::shared_ptr <std::map<std::string, std::shared_ptr<object>>> _global_hash_map;
			

		/// <summary>
		/// properties and funtions for thread
		/// </summary>
		std::thread _interpreter_thread;
		bool _is_thread_running;
		void _loop();

		void _script_start_signal();
		void _script_start_wait();

		void _script_end_signal();
		void _script_end_wait();

		void _clear_error_message();
		void _set_error_info(std::string message, int start_col, int end_col, int row);



		/// <summary>
		/// script run event control
		/// </summary>
		std::condition_variable _event_start_script;
		std::condition_variable _event_end_script;
		/////std::condition_variable _event_globals_hash;

		std::mutex _mtx_event_start_script;
		std::mutex _mtx_event_end_script;
		std::mutex _mtx_event_global_hash;
		std::mutex _mtx_event_external_hash;
		std::mutex _mtx_event_trace_callback;

		bool _is_script_running;
		bool _is_terminating;

		std::string _script_file_path;
		std::string _script_content;
		std::string _script_module_path;
		bool _is_content;


		std::list<std::string> _global_names;

	
		bool _has_error;
		int _error_start_column;
		int _error_end_column;
		int _error_rows;
		std::string _error_message;


		// native module 
		std::shared_ptr<std::map<std::string, hv::v1::native_module>> _native_modules;

		void trace(std::string input);
		bool register_converter(std::string type, converter* converter);


		// lambda trace callback
		std::function<void(char *)> _trace_callback;


	public:
		interpreter();
		~interpreter();


		/// <summary>
		/// interpreter member functions
		/// </summary>
		/// 
		bool set_module_path(std::string path);
		bool run_script(std::string content);
		bool run_file(std::string path);

		bool register_external_object(std::string key, std::shared_ptr<object> data);
		std::shared_ptr<object> external_object(std::string key);
		bool check_external_object(std::string key);
		void clear_external_object();

		std::list<std::string> global_names();
		std::shared_ptr<std::map<std::string, std::shared_ptr<object>>> global_objects();


		std::list<std::string> external_names();
		std::shared_ptr<std::map<std::string, std::shared_ptr<object>>> external_objects();

		std::shared_ptr<std::map<std::string, hv::v1::native_module>> native_modules();

		
		bool terminate();
		void release_native_modules();

		void set_trace_callback(std::function<void(char *)> _callback);
		void reset_trace_callback();

		 
		/// <summary>
		/// script static functions
		/// </summary>
		static bool init_v8_startup_data(std::string path);
		static void init_v8_platform();
		static bool init_v8_engine();
		static void set_v8_flag(std::string flag);
	};
	
}

#endif
