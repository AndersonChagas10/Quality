var curretParCompany_Id = 1;
var currentParFrequency_Id;
var parametrization = null;
var currentParDepartment_Id;
var currentParCargo_Id;

function getAppParametrization() {

   $.ajax({
      data: {},
      url: urlPreffix + '/api/AppColeta/GetAppParametrization/' + curretParCompany_Id + '/' + currentParFrequency_Id,
      type: 'GET',
      success: function (data) {
         
         _writeFile("appParametrization.txt", JSON.stringify(data), function () {
            parametrization = data;
            listarParDepartment(0);
         });

      },
      timeout: 600000,
      error: function () {
         $(this).html($(this).attr('data-initial-text'));
      }
   });
}