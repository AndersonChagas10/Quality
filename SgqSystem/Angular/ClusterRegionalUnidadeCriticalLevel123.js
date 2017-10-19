(function () {
    app.controller('CtrlClstRegUnitCriticalLevel123', ['$scope', '$http',
        function ($scope, $http) {

            $http({
                method: 'POST',
                url: GetListCluster,
                data: UnitList
            }).
            then(function (r) {
                $scope.cluster = r.data;
            });

            $http({
                method: 'POST',
                url: GetListStructure,
                data: JSON.stringify({ "UnitList": UnitList, "Cluster": $scope.clusterValue })
            }).
            then(function (r) {
                $scope.structure = r.data;
            });

            $scope.unit = UnitList;

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

            $scope.GetListStructureVinculadoCluster = function () {              

                if ($scope.clusterValue) {

                    $http({
                        method: 'POST',
                        url: GetListStructure,
                        data: JSON.stringify({ "UnitList": UnitList, "Cluster": $scope.clusterValue })
                    }).
                    then(function (r) {
                        $scope.structure = r.data;
                    });


                    if (!$scope.structureValue) {
                        $http({
                            method: 'POST',
                            url: GetListUnitVinculado,
                            data: JSON.stringify({ "UnitList": UnitList, "Cluster": $scope.clusterValue, "Structure": $scope.structureValue })
                        }).
                        then(function (r) {
                            $scope.unit = r.data;
                        });
                    }


                    $http({
                        method: 'POST',
                        url: GetListCriticalLevelVinculadoCluster,
                        data: JSON.stringify({ "UnitList": 0, "Cluster": $scope.clusterValue })
                    }).
                    then(function (r) {
                        $scope.criticalLevel = r.data;
                    });

                }
                else {
                    $http({
                        method: 'POST',
                        url: GetListStructure,
                        data: JSON.stringify({ "UnitList": UnitList, "Cluster": $scope.clusterValue })
                    }).
                   then(function (r) {
                       $scope.structure = r.data;

                       if (!$scope.structureValue) {
                           $scope.unit = UnitList;
                       }
                   });

                    $scope.criticalLevel = [];
                }
            }

            $scope.GetListUnitVinculadoStructure = function () {

                enviar['structureId'] = document.getElementById('structureId').value;

                if ($scope.clusterValue && !$scope.structureValue) {
                    $http({
                        method: 'POST',
                        url: GetListUnitVinculado,
                        data: JSON.stringify({ "UnitList": UnitList, "Cluster": $scope.clusterValue })
                    }).
                    then(function (r) {
                        $scope.unit = r.data;
                    });
                }

                else if ($scope.structureValue && $scope.clusterValue) {

                    $http({
                        method: 'POST',
                        url: GetListUnitVinculado,
                        data: JSON.stringify({ "UnitList": UnitList, "Cluster": $scope.clusterValue, "Structure": $scope.structureValue })
                    }).
                    then(function (r) {
                        $scope.unit = r.data;
                    });
                }
                else if ($scope.structureValue) {
                    $http({
                        method: 'POST',
                        url: GetListUnitVinculado,
                        data: JSON.stringify({ "UnitList": UnitList, "Cluster": $scope.clusterValue, "Structure": $scope.structureValue })
                    }).
                then(function (r) {
                    $scope.unit = r.data;
                });

                } else {
                    $scope.unit = UnitList;
                }
            }

            $scope.AtribuiObject = function () {
                enviar['unitId'] = document.getElementById('unitIdV').value;
            }

            $scope.GetLevel1ByCriticalLevel = function () {

                enviar['criticalLevelId'] = document.getElementById('criticalLevelId').value;
                enviar['level1Id'] = parseInt(document.getElementById('level1Idv').value);

                if ($scope.clusterValue) {
                    $http({
                        method: 'POST',
                        url: GetListLevel1VinculadoCriticalLevel,
                        data: JSON.stringify({ "Cluster": $scope.clusterValue, "CriticalLevel": $scope.criticalLevelValue })
                    }).
                    then(function (r) {
                        $scope.level1 = r.data;
                    });
                }
            }

            $scope.GetListLevel2VinculadoLevel1 = function () {
                if ($scope.level1Value) {
                    $http({
                        method: 'POST',
                        url: GetListLevel2VinculadoLevel1 + "/" + $scope.level1Value,
                        //data: JSON.stringify({ Id: $scope.level1Value })
                    }).
                    then(function (r) {
                        $scope.level2 = r.data;
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
})();
