#pragma once

#ifndef HV_STRING_CVT
#define HV_STRING_CVT

#include <iostream>
#include <string>
#include <locale>
#include <codecvt>

//--------------------------------
// text_conv_traits
//---------------------------------
//UTF8 타입 정의하기.

struct u8string :public std::string {
	using base_type = std::string;
	using base_type::base_type;

	u8string() = default;

	template<typename T>
	u8string(T&& t) :base_type(std::forward<T>(t)) {
	}
};

template<typename T, typename Y>
class text_conv_traits {
};

template<>
struct text_conv_traits<std::wstring, std::string> {
	using cvt_from_t = wchar_t;
	using cv_to_t = char;
	using from_t = std::wstring;
	using to_t = std::string;

	static to_t apply(cvt_from_t const* input, std::size_t size, std::locale loc)
	{
		using state_type = std::mbstate_t;
		using codecvt_t = std::codecvt<wchar_t, char, state_type>;
		using int_type = typename to_t::traits_type::int_type;
		enum { _BUF_INC = 8, _BUF_MAX = 16 };

		codecvt_t const& cvt = std::use_facet<codecvt_t>(loc);

		cvt_from_t const* _beg = input;
		cvt_from_t const* _end = input + size;
		cvt_from_t const* _beg_sav = _beg;

		state_type _state{};
		to_t temp_buf, result_buf;
		temp_buf.append((std::size_t)_BUF_INC, (cv_to_t)'\0');

		for (std::size_t _Nconv = 0; _beg != _end; _Nconv = _beg - _beg_sav) {
			char* _Dest = &*temp_buf.begin();
			char* _Dnext = nullptr;

			switch (cvt.out(_state, _beg, _end, _beg, _Dest, _Dest + temp_buf.size(), _Dnext))
			{
			case codecvt_t::partial:
			case codecvt_t::ok:
				if (_Dest < _Dnext)
					result_buf.append(_Dest, (std::size_t)(_Dnext - _Dest));
				else if (temp_buf.size() < _BUF_MAX)
					temp_buf.append((std::size_t)_BUF_INC, (cv_to_t)'\0');

				break;

			case codecvt_t::noconv:
				for (; _beg != _end; ++_beg)
					result_buf.append((std::size_t)1, (cv_to_t)(int_type)*_beg);
				break;

			default:
				result_buf.push_back('?');
				_state = state_type{};
				_beg++;
			}
		}
		return result_buf;
	}
};

template<>
struct text_conv_traits<std::string, std::wstring> {
	using cvt_from_t = char;
	using cvt_to_t = wchar_t;
	using from_t = std::string;
	using to_t = std::wstring;

	static to_t apply(cvt_from_t const* input, std::size_t size, std::locale loc)
	{
		using state_type = std::mbstate_t;
		using int_type = typename to_t::traits_type::int_type;
		using codecvt_t = std::codecvt<wchar_t, char, state_type>;
		enum { _BUF_INC = 8, _BUF_MAX = 16 };

		codecvt_t const& cvt = std::use_facet<codecvt_t>(loc);

		state_type _state{};
		to_t temp_buf, result_buf;

		cvt_from_t const* _beg = input;
		cvt_from_t const* _end = input + size;
		cvt_from_t const* _beg_sav = _beg;

		temp_buf.append((std::size_t)_BUF_INC, (cvt_to_t)'\0');
		for (std::size_t _Nconv = 0; _beg != _end; _Nconv = _beg - _beg_sav)
		{
			wchar_t* _Dest = &*temp_buf.begin();
			wchar_t* _Dnext;

			switch (cvt.in(_state, _beg, _end, _beg, _Dest, _Dest + temp_buf.size(), _Dnext))
			{
			case codecvt_t::partial:
			case codecvt_t::ok:
				if (_Dest < _Dnext)
					result_buf.append(_Dest, (std::size_t)(_Dnext - _Dest));
				else if (temp_buf.size() < _BUF_MAX)
					temp_buf.append((std::size_t)_BUF_INC, (cvt_to_t)'\0');

				break;

			case codecvt_t::noconv:
				for (; _beg != _end; ++_beg)
					result_buf.append((std::size_t)1, (cvt_to_t)(int_type)*_beg);
				break;

			default:
				result_buf.push_back(L'?');
				_state = state_type{};
				_beg++;
			}
		}
		return result_buf;
	}
};

template<>
struct text_conv_traits<u8string, std::wstring> {
	using cvt_from_t = char;
	using cvt_to_t = wchar_t;
	using from_t = u8string;
	using to_t = std::wstring;

	static to_t apply(cvt_from_t const* input, std::size_t size, std::locale loc) {
		using codecvt_t = std::codecvt_utf8<wchar_t>;
		using state_type = codecvt_t::state_type;
		using int_type = typename to_t::traits_type::int_type;
		enum { _BUF_INC = 8, _BUF_MAX = 16 };

		codecvt_t const cvt;

		state_type _state{};
		to_t temp_buf, result_buf;
		cvt_from_t const* _beg = input;
		cvt_from_t const* _end = input + size;
		cvt_from_t const* _beg_sav = _beg;

		temp_buf.append((std::size_t)_BUF_INC, (cvt_to_t)'\0');
		for (std::size_t _Nconv = 0; _beg != _end; _Nconv = _beg - _beg_sav)
		{
			wchar_t* _Dest = &*temp_buf.begin();
			wchar_t* _Dnext;

			switch (cvt.in(_state, _beg, _end, _beg, _Dest, _Dest + temp_buf.size(), _Dnext))
			{
			case codecvt_t::partial:
			case codecvt_t::ok:
				if (_Dest < _Dnext)
					result_buf.append(_Dest, (std::size_t)(_Dnext - _Dest));
				else if (temp_buf.size() < _BUF_MAX)
					temp_buf.append((std::size_t)_BUF_INC, (cvt_to_t)'\0');

				break;

			case codecvt_t::noconv:
				for (; _beg != _end; ++_beg)
					result_buf.append((std::size_t)1, (cvt_to_t)(int_type)*_beg);
				break;

			default:
				result_buf.push_back(L'?');
				_state = state_type{};
				_beg++;
			}
		}
		return result_buf;
	}
};

template<>
struct text_conv_traits<std::wstring, u8string> {
	using cvt_from_t = wchar_t;
	using cvt_to_t = char;
	using from_t = std::wstring;
	using to_t = u8string;

	static to_t apply(cvt_from_t const* input, std::size_t size, std::locale loc) {
		using codecvt_t = std::codecvt_utf8<wchar_t>;
		using state_type = codecvt_t::state_type;
		using int_type = to_t::traits_type::int_type;
		enum { _BUF_INC = 8, _BUF_MAX = 16 };

		codecvt_t const cvt;

		state_type _state{};
		to_t temp_buf, result_buf;
		cvt_from_t const* _beg = input;
		cvt_from_t const* _end = input + size;
		cvt_from_t const* _beg_sav = _beg;

		temp_buf.append((std::size_t)_BUF_INC, (cvt_to_t)'\0');
		for (std::size_t _Nconv = 0; _beg != _end; _Nconv = _beg - _beg_sav)
		{
			char* _Dest = &*temp_buf.begin();
			char* _Dnext;

			switch (cvt.out(_state, _beg, _end, _beg, _Dest, _Dest + temp_buf.size(), _Dnext))
			{
			case codecvt_t::partial:
			case codecvt_t::ok:
				if (_Dest < _Dnext)
					result_buf.append(_Dest, (std::size_t)(_Dnext - _Dest));
				else if (temp_buf.size() < _BUF_MAX)
					temp_buf.append((std::size_t)_BUF_INC, (cvt_to_t)'\0');

				break;

			case codecvt_t::noconv:
				for (; _beg != _end; ++_beg)
					result_buf.append((std::size_t)1, (cvt_to_t)(int_type)*_beg);
				break;

			default:
				result_buf.push_back('?');
				_state = state_type{};
				_beg++;
			}
		}
		return result_buf;
	}
};

template<>
struct text_conv_traits<std::string, u8string> {
	using cvt_from_t = char;
	using to_t = u8string;

	static to_t apply(cvt_from_t const* input, std::size_t size, std::locale loc) {
		auto mvalue = text_conv_traits<std::string, std::wstring>::apply(input, size, loc);
		return text_conv_traits<std::wstring, u8string>::apply(mvalue.data(), mvalue.size(), loc);
	}
};

template<>
struct text_conv_traits<u8string, std::string> {
	using cvt_from_t = char;
	using to_t = std::string;

	static to_t apply(cvt_from_t const* input, std::size_t size, std::locale loc) {
		auto mvalue = text_conv_traits<u8string, std::wstring>::apply(input, size, loc);
		return text_conv_traits<std::wstring, std::string>::apply(mvalue.data(), mvalue.size(), loc);
	}
};

//--------------------------------
// text_conv
//---------------------------------
template<typename FROM, typename TO, typename _Traits = text_conv_traits<FROM, TO> >
class text_conv {
	using self_trait = _Traits;
	using from_t = FROM;
	using to_t = TO;
	using self_type = text_conv<from_t, to_t, self_trait>;
	using cvt_from_t = typename self_trait::cvt_from_t;

public:
	to_t operator()(cvt_from_t const* input, std::size_t size, std::locale loc) const {
		return self_trait::apply(input, size, loc);
	}

	to_t operator()(from_t const& input, std::locale loc) const {
		return (*this)(input.data(), input.size(), loc);
	}

	template<std::size_t N>
	to_t operator()(cvt_from_t const(&input)[N], std::locale loc) const {
		return (*this)(input, N, loc);
	}
};


//--------------------------------
// text sugar coding
//---------------------------------
template<typename T>
inline std::string wstring_to_string(T& datas, std::locale loc = std::locale("")) {
	text_conv<std::wstring, std::string> cvt;
	return cvt(datas, loc);
}

template<typename T>
inline std::string u8string_to_string(T& datas, std::locale loc = std::locale("")) {
	text_conv<u8string, std::string> conv1;
	return conv1(datas, loc);
}
//-------------------------------
template<typename T>
inline std::wstring string_to_wstring(T& datas, std::locale loc = std::locale("")) {
	text_conv<std::string, std::wstring> cvt;
	return cvt(datas, loc);
}

template<typename T>
inline std::wstring u8string_to_wstring(T& datas, std::locale loc = std::locale("")) {
	text_conv<u8string, std::wstring> cvt;
	return cvt(datas, loc);
}
//-------------------------------
template<typename T>
inline std::string wstring_to_u8string(T& datas, std::locale loc = std::locale("")) {
	text_conv<std::wstring, u8string> cvt;
	return cvt(datas, loc);
}

template<typename T>
inline std::string string_to_u8string(T& datas, std::locale loc = std::locale("")) {
	text_conv<std::string, u8string> cvt;
	return cvt(datas, loc);
}


#endif // !HV_STRING_CVT

