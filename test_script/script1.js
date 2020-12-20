
var calculation = 5 * 2;
var calculation2 = calculation + "";
var hash_map = new Map();
hash_map.set("test1", 1);
hash_map.set("test2", 2);
hash_map.set("test3", calculation);

var hash_map2 = new Map([
	["test1", "test!!"],
	["한글키", "test!!"],
	["한글키2", calculation2],
]);