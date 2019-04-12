var resultColeta = [];
/*
{
	ParDepartment_Id,
	ParCargo_Id,
	Evaluation,
	Sample
}
*/

function getResultEvaluationSample(parDepartment_Id, parCargo_Id){
	
	var obj = {
		ParDepartment_Id : parDepartment_Id,
		ParCargo_Id : parCargo_Id,
		Evaluation : 1,
		Sample : 1
	};
	
	$(resultColeta).each(function (i, o) {
		if(o.ParDepartment_Id == parDepartment_Id && o.ParCargo_Id == parCargo_Id){
			obj = o;
		}
	});
	
	return obj;
	
}