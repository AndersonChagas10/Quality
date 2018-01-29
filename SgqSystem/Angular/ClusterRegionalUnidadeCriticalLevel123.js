var clusterGroupInitialMock = "In-Natura";

(function () {
    app.controller('CtrlClstRegUnitCriticalLevel123', ['$scope', '$http',
        function ($scope, $http) {


            $http({
                method: 'POST',
                url: GetListClusterGroup,
                data: GetUsuarioId()
            }).
                then(function (r) {
                    $scope.clusterGroup = r.data;
                });

            $http({
                method: 'POST',
                url: GetListCluster,
                data: JSON.stringify({ "UserId": GetUsuarioId() })
            }).
                then(function (r) {
                    $scope.cluster = r.data;
                });

            $http({
                method: 'POST',
                url: GetListStructure,
                data: JSON.stringify({ "UserId": GetUsuarioId(), "ClusterArr": $scope.clusterValue })
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

            $scope.GetListClusterVinculadoClusterGroup = function () {
                //if ($scope.clusterGroupValue) {

                $http({
                    method: 'POST',
                    url: GetListCluster,
                    data: JSON.stringify({ "UserId": GetUsuarioId(), "ClusterGroup": $scope.clusterGroupValue })
                }).
                    then(function (r) {
                        $scope.cluster = r.data;
                        AtribuiCluster();

                    });
            }

            $scope.GetListStructureVinculadoCluster = function () {

                enviar['clusterIdArr'] = $('#clusterId').val();

                // Desabilita Nivel Critico se processo não selecionado
                if ($('#clusterId').val().length > 0) {
                    $('#criticalLevelId').prop("disabled", false);
                } else {
                    $('#criticalLevelId').prop("disabled", true);
                }

                if ($scope.clusterValue) {

                    $http({
                        method: 'POST',
                        url: GetListStructure,
                        data: JSON.stringify({ "UserId": GetUsuarioId(), "ClusterArr": $scope.clusterValue })
                    }).
                        then(function (r) {
                            $scope.structure = r.data;
                        });


                    if (!$scope.structureValue) {
                        $http({
                            method: 'POST',
                            url: GetListUnitVinculado,
                            data: JSON.stringify({
                                "UserId": GetUsuarioId(), "ClusterArr": $scope.clusterValue, "StructureArr": $scope.structureValue
                        })
                        }).
                            then(function (r) {
                                $scope.unit = r.data;
                            });
                    }


                    $http({
                        method: 'POST',
                        url: GetListCriticalLevelVinculadoCluster,
                        data: JSON.stringify({ "ClusterArr": $scope.clusterValue })
                    }).
                        then(function (r) {
                            $scope.criticalLevel = r.data;
                        });

                }
                else {
                    $http({
                        method: 'POST',
                        url: GetListStructure,
                        data: JSON.stringify({ "UserId": GetUsuarioId(), "ClusterArr": $scope.clusterValue })
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
                enviar['structureIdArr'] = $('#structureId').val();

                if ($scope.clusterValue && !$scope.structureValue) {
                    $http({
                        method: 'POST',
                        url: GetListUnitVinculado,
                        data: JSON.stringify({ "UserId": GetUsuarioId(), "ClusterArr": $scope.clusterValue })
                    }).
                        then(function (r) {
                            $scope.unit = r.data;
                        });
                }

                else if ($scope.structureValue && $scope.clusterValue) {

                    $http({
                        method: 'POST',
                        url: GetListUnitVinculado,
                        data: JSON.stringify({ "UserId": GetUsuarioId(), "ClusterArr": $scope.clusterValue, "StructureArr": $scope.structureValue })
                    }).
                        then(function (r) {
                            $scope.unit = r.data;
                        });
                }
                else if ($scope.structureValue) {
                    $http({
                        method: 'POST',
                        url: GetListUnitVinculado,
                        data: JSON.stringify({ "UserId": GetUsuarioId(), "ClusterArr": $scope.clusterValue, "StructureArr": $scope.structureValue })
                    }).
                        then(function (r) {
                            $scope.unit = r.data;
                        });

                } else {
                    $scope.unit = UnitList;
                }
            }

            $scope.AtribuiObject = function () {
                enviar['unitIdArr'] = $('#unitIdV').val();
                enviar['unitId'] = document.getElementById('unitIdV').value;
                //console.log($scope.unit);
                //console.log($scope.unitValue);
            }

            $scope.GetLevel1ByCriticalLevel = function () {

                enviar['criticalLevelId'] = document.getElementById('criticalLevelId').value;
                enviar['criticalLevelIdArr'] = $('#criticalLevelId').val();
                enviar['level1Id'] = parseInt(document.getElementById('level1Idv').value);
                enviar['level1IdArr'] = $('#level1Idv').val();

                if ($scope.clusterValue) {
                    $http({
                        method: 'POST',
                        url: GetListLevel1VinculadoCriticalLevel,
                        data: JSON.stringify({ "ClusterArr": $scope.clusterValue, "CriticalLevelArr": $scope.criticalLevelValue })
                    }).
                        then(function (r) {
                            $scope.level1 = r.data;
                        });
                }
            }

            $scope.GetListLevel2VinculadoLevel1 = function () {
                enviar['level1Id'] = parseInt(document.getElementById('level1Idv').value);
                enviar['level1IdArr'] = $('#level1Idv').val();
                //
                //Desabilita monitoramento e tarefa quando selecionado mais de um indicador
                if ($('#level1Idv').val().length > 1) {
                    $('#level2Idv').attr('disabled', true);
                    $('#level3Idv').attr('disabled', true);
                    $scope.level2Value = null;
                    $scope.level3Value = null;
                } else {
                    $('#level2Idv').attr('disabled', false);
                    $('#level3Idv').attr('disabled', false);
                }

                //Desabilita tipo de indicador quando há um unico selecionado, caso contrario habilita
                if ($('#level1Idv').val().length != 1) {
                    $('#statusIndicador').prop("disabled", false);
                } else {
                    $('#statusIndicador').prop("disabled", true);
                }


                if ($scope.level1Value) {
                    $http({
                        method: 'POST',
                        url: GetListLevel2VinculadoLevel1,// + "/" + $scope.level1Value,
                        data: JSON.stringify({ "Level1IdArr": $scope.level1Value })
                    }).
                        then(function (r) {
                            $scope.level2 = r.data;
                            AtribuiLevel2();
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
                                AtribuiLevel2();
                            });
                    } else {
                        $scope.level2 = [];
                        AtribuiLevel2();
                    }
                }
            }

            $scope.GetListLevel3VinculadoLevel2 = function () {
                enviar['level2Id'] = parseInt(document.getElementById('level2Idv').value);
                enviar['level2IdArr'] = $('#level2Idv').val();

                if ($('#level2Idv').val().length > 1 || !!$('#level2Idv').attr('disabled')) {
                    $('#level3Idv').attr('disabled', true);
                    $scope.level3Value = null;
                } else {
                    $('#level3Idv').attr('disabled', false);
                }

                //Defining the $http service for getting Level3 By Level2
                if ($scope.level2Value) {
                    $http({
                        method: 'POST',
                        url: GetListLevel3VinculadoLevel2,// + "/" + $scope.level2Value,
                        data: JSON.stringify({ "Level2IdArr": $scope.level2Value,  })
                        //data: JSON.stringify({ Id: $scope.level2Value })
                    }).
                        then(function (r) {
                            $scope.level3 = r.data;
                        });
                }
                //Defining the $http service for getting Level3 By Level2 and Level1
                /*else if ($scope.level2Value && $scope.level1Value) {
                    $http({
                        method: 'POST',
                        url: GetListLevel3VinculadoLevel2Level1,// + "/" + $scope.level1Value + "/" + $scope.level2Value,
                        //data: JSON.stringify({ Id:  } + { Id:  })
                    }).
                        then(function (r) {
                            $scope.level3 = r.data;
                        });
                }*/
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

function AtribuiCluster() {
    setTimeout(function () {
        enviar['clusterSelected_Id'] = document.getElementById('clusterId').value;
        var option = $('#clusterGroupId option').filter(function () { return $(this).html() == clusterGroupInitialMock; }).val();
        if (enviar["clusterGroupId"] == undefined || enviar["clusterGroupId"] <= 0) {
            $('#clusterGroupId').val(option).trigger("change");
            enviar["clusterGroupId"] = option;
            // Inicia o Nivel Critico, desabilitado, por conta de ser dependente da seleção do Processo
            $('#criticalLevelId').prop("disabled", true);
        }
    }, 1);
}

function AtribuiLevel2() {
    setTimeout(function () {
        enviar['level2Id'] = document.getElementById('level2Idv').value;
    }, 1);
}