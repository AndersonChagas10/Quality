var curretParCompany_Id;
var currentParFrequency_Id;
var parametrization = null;
var currentParDepartment_Id;
var currentParDepartmentParent_Id;
var currentParCargo_Id;
var globalColetasRealizadas = [];

var currentTotalEvaluationValue = 0;
var currentTotalSampleValue = 0;

function getAppParametrization() {
	
	openMensagem('Por favor, aguarde até que seja feito o download do planejamento selecionado','blue','white');
   $.ajax({
      data: {},
      url: urlPreffix + '/api/AppColeta/GetAppParametrization/' + curretParCompany_Id + '/' + currentParFrequency_Id,
      type: 'GET',
      success: function (data) {

         _writeFile("appParametrization.txt", JSON.stringify(data), function () {
            parametrization = data;
            listarParDepartment(0);
         });
		closeMensagem();
      },
      timeout: 600000,
      error: function () {
        $(this).html($(this).attr('data-initial-text'));
		closeMensagem();
      }
   });
}

function showAllGlobalVar() {
   console.log("ParCompany:" + curretParCompany_Id);
   console.log("Frequencia: " + currentParFrequency_Id);
   console.log("Departamento: " + currentParDepartment_Id);
   console.log("Cargo: " + currentParCargo_Id);
}