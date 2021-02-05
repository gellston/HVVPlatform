var core = require("core"); // dll 로드 !!

var image = new core.image("test", 2020,2020, core.u8_image, 1);

var rand_value = core.rand(0,255);
image.fill(rand_value);


for(let index =0; index < 100; index++){
	var rand_x = core.rand(0,2020);
	var rand_y = core.rand(0,2020);
	var point = new core.point("point!!", rand_x, rand_y);
	image.register_draw_object(point);
}