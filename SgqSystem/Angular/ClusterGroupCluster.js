(function () {
    /*
    START ANGULAR MODULE
    */

    app.controller('Renan', ['$scope', '$http',
        function ($scope, $http) {

            $scope.clusterGroup = listaClusterGroup;
            //$scope.cluster = listaCluster;

            $scope.GetListClusterVinculadoClusterGroup = function () {
                if ($scope.clusterGroupValue) {
                    $http({
                        method: 'GET',
                        url: urlGetListClusterVinculadoClusterGroup,
                        params: { ClusterGroupId: $scope.clusterGroupValue }
                    }).
                    then(function (r) {
                        $scope.cluster = r.data;
                    });
                }
                else {

                    //$scope.cluster = listaCluster;
                    $scope.cluster = [];
                }
            }

        }]);

    /*
        END ANGULAR MODULE
    */

})();
