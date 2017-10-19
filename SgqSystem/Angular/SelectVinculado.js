(function () {
    //'use strict';

    /*
    START ANGULAR MODULE
    */

    app.controller('MeuControle', ['$scope', '$http',
        function ($scope, $http) {

            //if (viewBag) {
            //$scope.ListaInicial = ListaInicial;
            //} else {

            $http({
                method: 'POST',
                url: UrlListaInicial
            }).
            then(function (r) {
                $scope.ListaInicial = r.data;
            });
            //}

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
