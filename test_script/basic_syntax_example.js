var core = require("addon_core"); // dll 로드 !!


var image = new core.image("test", 2020,2020, core.u8_image, 1);


var rand = core.rand(0, 255);

image.fill(rand);

var number = 1;
var string = "test";
var bool = false;