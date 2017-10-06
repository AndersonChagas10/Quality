(function () {
    //'use strict';

    /*
    START ANGULAR MODULE
    */

    app.controller('MainCtrl', ['$scope', '$http',
        function ($scope, $http) {

            if (viewBag) {
                $scope.ListaInicial = ListaInicial;
            } else {

                $http({
                    method: 'POST',
                    url: UrlListaInicial
                }).
                then(function (r) {
                    $scope.ListaInicial = r.data;
                });
            }

            //$http({
            //    method: 'POST',
            //    url: GetListLevel2
            //}).
            //then(function (r) {
            //    $scope.level2 = r.data;
            //});

            //$http({
            //    method: 'POST',
            //    url: GetListLevel3
            //}).
            //then(function (r) {
            //    $scope.level3 = r.data;
            //});


            //Defining the $http service for getting Level2 By Level1
            $scope.GetNextDDL = function (elementValue, url, elementChainedName, urlParaOptionSelecione) {
                if (elementValue) {
                    $http({
                        method: 'POST',
                        url: url + "/" + elementValue,
                    }).
                    then(function (r) {
                        $scope[elementChainedName] = r.data;
                    });
                }
                else {
                    if (urlParaOptionSelecione) {
                        $http({
                            method: 'POST',
                            url: urlParaOptionSelecione
                        }).
                       then(function (r) {
                           $scope[elementChainedName] = r.data;
                       });
                    }
                    $scope[elementChainedName] = [];
                }
            }
        }]);

    /*
        END ANGULAR MODULE
    */

})();
