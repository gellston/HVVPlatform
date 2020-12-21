
#include "image.h"

#include <string>

hv::v1::image::image(std::string name, unsigned char * data, unsigned int size) : object(name, "image"){

}

hv::v1::image::image(unsigned char* data, unsigned int size) : image("anonymous", data, size){

}