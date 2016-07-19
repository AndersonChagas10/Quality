function lineGroupTask(groupTask) {

    var out = "<div class='panel panel-default side-item' id='group-task-"+groupTask.Id+"'>"+
					"<div class='panel-heading'>"+
						 "<h4 class='panel-title'>"+
							"<a data-toggle='collapse' data-target='#group-task-body-"+groupTask.Id+"' href='#"+groupTask.Id+" '>"+
								groupTask.Name+
							"</a>"+
						 "</h4>"+
					"</div>"+
					"<div id='group-task-body-"+groupTask.Id+"' class='panel-collapse collapse in'>"+
					"<div id='group-task-body-child-"+groupTask.Id+"' ></div>"+
                    "</div>"+
                "</div>";
    return out;
}
