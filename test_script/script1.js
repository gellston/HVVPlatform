
var core = require("core");

var image1 = new core.image("image1",2048, 2048, 1);
var image2 = new core.image("image2", 2048, 2048,1);

script.trace(image1.to_string());
script.trace(image2.to_string());

script.trace("image width = " + image1.width );
script.trace("image height = " + image1.height );
image1.show(image2);