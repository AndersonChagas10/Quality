(function () {
    //'use strict';

    /*
    START ANGULAR MODULE
    */

    app.controller('MainCtrl', ['$scope', '$http',
        function ($scope, $http) {
            //Defining the $http service for getting the Level1 2 and 3 by default
            $http({
                method: 'POST',
                url: GetListLevel1
            }).
            then(function (r) {
                $scope.level1 = r.data;
            });
            if (exibeTodosLevels) {
                $http({
                    method: 'POST',
                    url: GetListLevel2
                }).
                then(function (r) {
                    $scope.level2 = r.data;
                });

                $http({
                    method: 'POST',
                    url: GetListLevel3
                }).
                then(function (r) {
                    $scope.level3 = r.data;
                });
            }

            //Defining the $http service for getting Level2 By Level1
            $scope.GetListLevel2VinculadoLevel1 = function () {
                if ($scope.level1Value) {
                    $http({
                        method: 'POST',
                        url: GetListLevel2VinculadoLevel1 + "/" + $scope.level1Value,
                        //data: JSON.stringify({ Id: $scope.level1Value })
                    }).
                    then(function (r) {
                        $scope.level2 = r.data;
                        $scope.level3 = [];
                    });

                }
                else {
                    if (exibeTodosLevels) {
                        $http({
                            method: 'POST',
                            url: GetListLevel2
                        }).
                       then(function (r) {
                           $scope.level2 = r.data;
                       });
                    } else {
                        $scope.level2 = [];
                    }
                }
            }

            $scope.GetListLevel3VinculadoLevel2 = function () {
                //Defining the $http service for getting Level3 By Level2
                if ($scope.level2Value && !$scope.level1Value) {
                    $http({
                        method: 'POST',
                        url: GetListLevel3VinculadoLevel2 + "/" + $scope.level2Value,
                        //data: JSON.stringify({ Id: $scope.level2Value })
                    }).
                    then(function (r) {
                        $scope.level3 = r.data;
                    });
                }
                    //Defining the $http service for getting Level3 By Level2 and Level1
                else if ($scope.level2Value && $scope.level1Value) {
                    $http({
                        method: 'POST',
                        url: GetListLevel3VinculadoLevel2Level1 + "/" + $scope.level1Value + "/" + $scope.level2Value,
                        //data: JSON.stringify({ Id:  } + { Id:  })
                    }).
                    then(function (r) {
                        $scope.level3 = r.data;
                    });
                }
                else {
                    if (exibeTodosLevels) {
                        $http({
                            method: 'POST',
                            url: GetListLevel3
                        }).
                       then(function (r) {
                           $scope.level3 = r.data;
                       });
                    } else {
                        $scope.level3 = [];
                    }
                }
            }
        }]);

    /*
        END ANGULAR MODULE
    */

})();
