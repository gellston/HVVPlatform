var core = require("addon_core");

var image = new core.image("test", 2020,2020, core.u8_image, 1);
script.trace("image class");
script.trace("sum = " + image.to_string());





var point = new core.point("test", 100,100);
script.trace("point class");
script.trace("result = " + point.x());
script.trace("result = " + point.y());
script.trace("result = " + point.to_string());