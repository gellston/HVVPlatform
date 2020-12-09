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

namespace hv::v1{


	/// <summary>
	/// pimpl section
	/// </summary>



	class HVAPI_EXPORT interpreter {

	private:

		//friend class hv::v1::converter;
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
		std::shared_ptr<pimpl> _global_object_hash;


		/// <summary>
		/// pimpl converter lambda
		/// </summary>
		//std::shared_ptr<pimpl> _converter_lambda;
		std::map<std::string, std::shared_ptr<converter>> _converter_lambda;

			

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



		/// <summary>
		/// script run event control
		/// </summary>
		std::condition_variable _event_start_script;
		std::condition_variable _event_end_script;
		/////std::condition_variable _event_globals_hash;

		std::mutex _mtx_event_start_script;
		std::mutex _mtx_event_end_script;
		std::mutex _mtx_event_global_hash;

		bool _is_script_running;

		std::string _script_file_path;
		std::string _script_content;
		bool _is_content;


		std::list<std::string> _global_names;

		
		void trace(std::string input);
		

	public:
		interpreter();
		~interpreter();


		/// <summary>
		/// interpreter member functions
		/// </summary>
		bool run_script(std::string content);
		bool run_file(std::string path);


		std::list<std::string> global_names();
		std::map<std::string, std::shared_ptr<object>> * global_objects();

		bool register_converter(std::string type , converter* converter);


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
