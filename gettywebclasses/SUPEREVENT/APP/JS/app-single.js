'use strict';

// Declare app level module which depends on views, and components
angular.module('myApp', ['ngRoute', 'ngMaterial', 'ngAnimate', 'flow']).
config(['$routeProvider', 'flowFactoryProvider', function ($routeProvider, flowFactoryProvider) {
    //$mdIconProvider.iconSet('check', 'checked.svg', 36);
    $routeProvider
          .when('/dash', {
              templateUrl: 'app/template/home-single.html',
              controller: 'homeCtrl'
          })
          .when('/gettysearch', {
              templateUrl: 'app/template/search-single.html',
              controller: 'searchCtrl'
          })          
    $routeProvider.otherwise({ redirectTo: '/dash' });
    flowFactoryProvider.on('catchAll', function (event) {
    });
} ])
.run(function ($rootScope, $location, globalData) {
    //$httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';

    //$rootScope.exitsImage = globalData.exists;
    $rootScope.$on("$routeChangeStart", function (event, next, current, $window) {        
        if (current && (current.controller == "cropCtrl" || current.controller == "searchCtrl")) {

            $rootScope.fromCrop = true;
        } else {
            $rootScope.fromCrop = false
        }
    });
})
.controller('homeCtrl', function ($scope, storeData, $window, $location, $rootScope, globalData) {
    var exists = null;
    $scope.limit = globalData.limit;
    if (exists) {
        $scope.existsImg = exists.items;
    }
    $scope.imgPath = globalData.path;

    function save_new_image(file, back) {
        var oFReader = new FileReader();
        oFReader.readAsDataURL(file);
        oFReader.onload = function (data) {
            var d = data.target.result;
            console.log(d)
            back(d);
        };
    }

    $scope.baseUrl = globalData.baseURL;
    $scope.uploadSingle = function () { var submit = document.getElementById('btnSave').click(); }

    $scope.cropSelection = [];
    function toggleSelection(site) {
        save_new_image(site.file, function (_img) {
            storeData.updateImage(0, _img);
        })

    };
    $scope.fileAdded = function ($file, $message, $flow) {
        toggleSelection($file)

    }
    $scope.remove = function (flow, file) {
        toggleSelection(file)
        file.cancel();

    }

   
    $scope.goGetty = function () {
        $location.path('/gettysearch')
    }
    $scope.removeImg = function (index, id) {
        storeData.removeImage(index, id, function (res) {
            if (res.status == 'ok') {
                $scope.existsImg.splice(res.index, 1);
                globalData.limit = 5 - $scope.existsImg.length;
            }
        });
    }
})
.controller('searchCtrl', function ($scope, $location, getty, storeData, $rootScope, $mdDialog, $window, globalData) {

    $scope.val = '';
    $scope.data = false;
    $scope.loading = false;
    $scope.isImages = false;
    var prime = false;

    $scope.serachGetty = function () { $window.sessionStorage.setItem('key', $scope.val); $scope.loading = true; prime = false; getty.getGetty($scope.val); }


    $scope.cropSelection = [];


    function showAlert($event) {
        $mdDialog.show({
            targetEvent: $event,
            template: '<md-dialog>' +
                '  <md-content>You can add maximum 5 images for Crop</md-content>' +
                '  <div class="md-actions">' +
                '    <md-button class="md-raised md-primary" ng-click="closeDialog()">' +
                '      Ok' +
                '    </md-button>' +
                '  </div>' +
                '</md-dialog>',
            controller: 'searchCtrl',
            onComplete: ''
        });
    }
    $scope.closeDialog = function () {
        $mdDialog.hide();
    };
    $scope.toggleSelection = function toggleSelection(site, event) {
        var idx = $scope.cropSelection.indexOf(site);
        console.log(idx);

        if (idx > -1) {
            $scope.cropSelection.splice(idx, 1);
        }
        else {
            if ($scope.cropSelection.length >= globalData.limit) {
                alert('Limit exceeded!');
                return false;
            }
            $scope.cropSelection.push(site);

        }

        $scope.isImages = $scope.cropSelection.length > 0 ? true : false;
        storeData.setImage($scope.cropSelection);


    };
    $scope.uploadSingle = function () { document.getElementById('btnSave').click(); }
    window.jsonCallback = function (res) {
        console.log(res)
        if (res !== '') {
            if (!prime) {
                $scope.items = res.menu.items;
                $scope.data = true;
            } else {
                angular.forEach(res.menu.items, function (i) {
                    $scope.items.push(i)
                });
                //$scope.items.push(res.menu.items);
            }

        }
        $scope.loading = false;

    }
    $scope.loadmore = function () {
        prime = true;
        $scope.loading = true;
        getty.paginate($scope.val);
    }
    

    $scope.val = globalData.sport;
    //$scope.serachGetty();
})

.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
})
.directive('checkLoading', function () {
    return {
        link: function (scope, element, attrs) {
            element.bind('error', function () {
                scope.error = true;
            });
            element.bind('load', function () {
                scope.error = false;
            });
        }
    }
})
.factory('getty', function ($http) {
    var gettyList = null, index = 1;
    return {
        setList: function (list) {
            gettyList = list;
        },
        getGetty: function (term, page) {
            if (!page) { index = 1 }
            return $http.jsonp('http://admin.shoptweets.com/storify/gettyimages.aspx?searchterm=' + term + '&searchmode=horizontal&startindex=' + index + '&callback=jsonCallback');
        },
        paginate: function (term) {
            index++;
            this.getGetty(term, 1);
        }
    }
})
.factory('storeData', function ($window, $http) {
    var pages = null, init = true, images = null, imgObj = null;
    return {
        isListingSet: function (set) {
            init = set;
        },
        setImage: function (p) {
            var self = this;
            /*var img=new Image();
            img.onload=function(){
            var canvas = document.createElement("canvas");
            canvas.width = img.width;
            canvas.height = img.height;
            var ctx = canvas.getContext("2d");
            ctx.drawImage(img, 0, 0);
            var dataURL = canvas.toDataURL();
            self.updateImage(0, dataURL);
            }
            img.src=p[0] ; */

            var xhr = new XMLHttpRequest();
            xhr.responseType = 'blob';
            xhr.onload = function () {
                var reader = new FileReader();
                reader.onloadend = function () {
                    self.updateImage(0, reader.result);
                }
                reader.readAsDataURL(xhr.response);
            };
            xhr.open('GET', p[0]);
            xhr.send();

        },
        getImage: function () {
            var isSet = $window.sessionStorage.getItem('selected');
            //console.log(isSet)
            if (!isSet) { $window.sessionStorage.setItem('selected', JSON.stringify(imgObj)) }
            var cropList = imgObj || JSON.parse(isSet);
            !imgObj && (imgObj = cropList);
            return cropList;
        },
        updateImage: function (index, value) {

            var hiddens = document.querySelectorAll('[id^="hdnimage"]')[index];
            hiddens.value = value;
        },
        _page: function (p) {
            if (p) {
                pages = p
            }
            return pages;
        },
        isListing: function (set) {

            return init;
        },
        removeImage: function (index, id, back) {
            var param = { 'type': 'supereventimages', 'supereventids': id }
            Object.toparams = function ObjecttoParams(obj) {
                var p = [];
                for (var key in obj) {
                    p.push(key + '=' + encodeURIComponent(obj[key]));
                }
                return p.join('&');
            };

            return $http({
                method: 'POST',
                url: baseurl + 'supereventajax.aspx',
                data: Object.toparams(param),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).then(function (res) { back({ status: 'ok', index: index, id: id }) });

        }
    };
})
    .factory('imageCart', function ($window) {
        var max = 1, init = true, images = null, imgObj = null;
        return {

    };
})


.directive('imgData', [function ($compile) {
    return {
        restrict: "EA",
        'scope': true,
        'require': '^flowInit',
        'link': function (scope, element, attrs) {
            var file = attrs.imgData;
            console.log(attrs)
            scope.$watch(file, function (file) {
                console.log(file)
                if (!file) {
                    return;
                }
                var fileReader = new FileReader();
                fileReader.readAsDataURL(file.file);
                fileReader.onload = function (event) {
                    scope.$apply(function () {
                        attrs.$set('value', event.target.result);
                        //attrs.$set('ng-checked', "cropSelection.indexOf('"+event.target.result+"') > -1");
                        //attrs.$set('ng-click', "toggleSelection('"+event.target.result+"')");
                    });
                };

            });

        }
    };
} ]).constant('globalData', { 'sport': sport, 'baseURL': baseurl, 'limit': 1, 'exists': jsondata, 'path': 'http://anil/gettyclasses/superevent/' })