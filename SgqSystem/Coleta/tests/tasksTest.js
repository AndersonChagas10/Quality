var tasks = new Array();

//constructor(Id, Name, Monitoring, Type, Group, Indicator) 

//begin carcass

tasks.push(new Task(1, "Specks",5, 0, null, 3));
tasks.push(new Task(2, "Dressing",5, 0, null, 3));
tasks.push(new Task(3, "Single Hairs",5, 0, null, 3));
tasks.push(new Task(4, "Clusters", 5, 0, null, 3));
tasks.push(new Task(5, "Hide", 5, 0, null, 3));

//begin cff

tasks.push(new Task(6, "Cuts",40, 1, 1, 6));
tasks.push(new Task(7, "Folds and Flaps",40, 1, 1, 6));
tasks.push(new Task(8, "Puncture",40, 1, 1, 6));

tasks.push(new Task(1002, "Cuts", 41, 1, 2, 6));
tasks.push(new Task(1003, "Folds and Flaps", 41, 1, 2, 6));
tasks.push(new Task(1004, "Puncture", 41, 1, 2, 6));

tasks.push(new Task(1005, "Cuts", 42, 1, 3, 6));
tasks.push(new Task(1006, "Folds and Flaps", 42, 1, 3, 6));
tasks.push(new Task(1007, "Puncture", 42, 1, 3, 6));

tasks.push(new Task(1008, "Cuts", 43, 1, 4, 6));
tasks.push(new Task(1009, "Folds and Flaps", 43, 1, 4, 6));
tasks.push(new Task(1010, "Puncture", 43, 1, 4, 6));

tasks.push(new Task(1011, "Cuts", 44, 1, 9, 6));
tasks.push(new Task(1012, "Folds and Flaps", 44, 1, 9, 6));
tasks.push(new Task(1013, "Puncture", 44, 1, 9, 6));

tasks.push(new Task(1014, "Cuts", 45, 1, 10, 6));
tasks.push(new Task(1015, "Folds and Flaps", 45, 1, 10, 6));
tasks.push(new Task(1016, "Puncture", 45, 1,10, 6));

tasks.push(new Task(1017, "Cuts", 46, 1, 11, 6));
tasks.push(new Task(1018, "Folds and Flaps", 46, 1, 11, 6));
tasks.push(new Task(1019, "Puncture", 46, 1, 11, 6));

tasks.push(new Task(1020, "Cuts", 1002, 1, 12, 6));
tasks.push(new Task(1021, "Folds and Flaps", 1002, 1, 12, 6));
tasks.push(new Task(1022, "Puncture", 1002, 1, 12, 6));




//begin htp

tasks.push(new Task(13, "Rinse hands with the chlorine spray between each carcass.",26, 2, 5, 2));
tasks.push(new Task(14, "Knives are to be steeled prior to placing in the sterilizer.",26, 2, 5, 2));

tasks.push(new Task(15, "Two knives are to be used in a knife rotation system.",26, 2, 6, 2));
tasks.push(new Task(16, "Knives are to be changed (rotated) between each individual carcass.",26, 2, 6, 2));
tasks.push(new Task(17, "All hide pattern openings done with one continuous cut.",26, 2, 6, 2));
tasks.push(new Task(18, "All hide pattern openings trimmed as close to the initial job of opening. The piece of carcass removed as a continuous portion.",26, 2, 6, 2));

tasks.push(new Task(19, "Sanitized by dipping in the 180°F sterilizer while running the blade for one (1) second between each carcass.",26, 2, 7,2));
tasks.push(new Task(20, "Hoses for the air knives secured in a manner that does not contact the exposed carcass surface.",26, 2, 7, 2));

tasks.push(new Task(21, "Knives are to be changed (rotated) between each individual carcass.",26, 2, 8, 2));

tasks.push(new Task(1024, "Other.",26, 2, 13, 2));


showTasks(tasks, taskgroups);