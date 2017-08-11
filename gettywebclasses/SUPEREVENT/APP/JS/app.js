'use strict';

// Declare app level module which depends on views, and components
angular.module('myApp', ['ngRoute', 'ngMaterial', 'ngAnimate', 'angular-darkroom', 'flow']).
config(['$routeProvider', 'flowFactoryProvider', function ($routeProvider, flowFactoryProvider) {
    //$mdIconProvider.iconSet('check', 'checked.svg', 36);
    $routeProvider
          .when('/dash', {
              templateUrl: 'app/template/home.html',
              controller: 'homeCtrl'
          })
          .when('/gettysearch', {
              templateUrl: 'app/template/search.html',
              controller: 'searchCtrl'
          })
          .when('/crop', {
              templateUrl: 'app/template/crop.html',
              controller: 'cropCtrl'
          })
    $routeProvider.otherwise({ redirectTo: '/dash' });
    flowFactoryProvider.on('catchAll', function (event) {
        console.log('catchAll', arguments);
    });
} ])
.run(function ($rootScope, $location, globalData) {
    //$httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';

    $rootScope.exitsImage = globalData.exists;
    $rootScope.$on("$routeChangeStart", function (event, next, current, $window) {
        console.log(current)
        if (current && (current.controller == "cropCtrl" || current.controller == "searchCtrl")) {

            $rootScope.fromCrop = true;
        } else {
            $rootScope.fromCrop = false
        }
    });
})
.controller('homeCtrl', function ($scope, storeData, $window, $location, $rootScope, globalData) {
    var exists = $rootScope.exitsImage;
    $scope.limit = globalData.limit;
    if (exists) {
        $scope.existsImg = exists.items;
    }
    $scope.imgPath = globalData.path;
    $window.sessionStorage.removeItem('selected')
    function save_new_image(file, back) {
        var oFReader = new FileReader();
        oFReader.readAsDataURL(file);
        oFReader.onload = function (data) {
            var d = data.target.result;
            back(d);
        };
    }

    $scope.baseUrl = globalData.baseURL;

    $scope.cropSelection = [];
    function toggleSelection(site) {
        save_new_image(site.file, function (_img) {
            var idx = $scope.cropSelection.indexOf(_img);

            if (idx > -1) {
                $scope.cropSelection.splice(idx, 1);
            }
            else {
                $scope.cropSelection.push(_img);
            }
            storeData.setImage($scope.cropSelection);
            $window.sessionStorage.setItem('selected', JSON.stringify(storeData.getImage()))
        })

    };
    $scope.fileAdded = function ($file, $message, $flow) {
        toggleSelection($file)

    }
    $scope.remove = function (flow, file) {
        toggleSelection(file)
        file.cancel();

    }

    $scope.goCrop = function () {
        $location.path('/crop')
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
    if ($rootScope.fromCrop) {
        var items = $window.sessionStorage.getItem('serachresult');
        $scope.items = JSON.parse(items);
        $scope.val = $window.sessionStorage.getItem('key');
        $scope.data = true;
    }
    $scope.serachGetty = function () { $window.sessionStorage.setItem('key', $scope.val); $scope.loading = true; prime = false; getty.getGetty($scope.val); }
    var prev = $window.sessionStorage.getItem('selected');

    $scope.cropSelection = [];
    if (prev) {
        var d = JSON.parse(prev);
        for (var i = 0, len = d.length; i < len; i++) {
            $scope.cropSelection.push(d[i].img);
        }
        $scope.isImages = $scope.cropSelection.length > 0 ? true : false;
    }

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
        $window.sessionStorage.setItem('selected', JSON.stringify(storeData.getImage()))

    };
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
            $window.sessionStorage.setItem('serachresult', JSON.stringify($scope.items));
        }
        $scope.loading = false;

    }
    $scope.loadmore = function () {
        prime = true;
        $scope.loading = true;
        getty.paginate($scope.val);
    }
    $scope.goCrop = function () {
        $location.path('/crop')
    }

    $scope.val = globalData.sport;
    $scope.serachGetty();
})
.controller('cropCtrl', function ($scope, storeData, $timeout, $window, globalData) {
    var imgs = storeData.getImage();
    $scope.imgPath = globalData.path;
    console.log(imgs)
    $scope.images = imgs;
    $scope.baseUrl = globalData.baseURL;
    var _me = this;
    //$scope.image = '';
    _me.preview_image = '';

    $scope.loading = true;
    $scope.count = 0;
    var cnt = $scope.images.length;

    $scope.image_changed = function (image) {
        console.log(image);
        console.log($scope.images)
        console.log($scope.active)
        var add = false;
        angular.forEach($scope.images, function (value, index) {

            if (index == $scope.active) {
                value.cropped = true;
                //value.img=image;
                storeData.updateImage(index, image);
                $scope.count++
                $scope.$apply();
            }
        })
        angular.forEach($scope.images, function (value, index) {
            if (!value.cropped && !add) {
                $scope.image = value.img;
                $scope.active = index;
                add = true;
                $scope.$apply();
            }
            if (cnt == $scope.count) {
                console.log('finish');
                $scope.image = '';
                $scope.$apply();
                return false;
            }
        })
    };
    var img = new Image();
    img.onload = function () {
        $scope.image = imgs[0].img;
        $scope.active = 0;
        $scope.loading = false;
        $scope.$apply();
    }
    img.onerror = function (err) {
        console.log(err)
    }
    img.crossOrigin = 'Anonymous';
    img.src = imgs[0].img;
    $scope.loadtoCrop = function (url, id) {
        $scope.active = id;
        $scope.image = url;

    }
    /*$scope.$on('flow::fileAdded', function(event, $flow, flowFile) {
    load_new_image(flowFile.file);
    });*/
    $scope.refreshImage = function (item) {
        var img = item.img;
        item.img = '';
        $timeout(function () { item.img = img; }, 100)
    }
    $scope.goSubmit = function () {
        console.log('click');
        var submit = document.getElementById('btnSave').click();

    }
    return _me;



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
            images = p;
            if (p.length > 0) {
                imgObj = [];
                for (var i = 0, len = p.length; i < len; i++) {
                    var obj = {};
                    obj.id = 'slide' + i;
                    obj.img = p[i];
                    obj.cropped = null;
                    obj.croppedimg = null
                    imgObj.push(obj)
                }
            }
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
            imgObj[index].cropped = true;
            imgObj[index].croppedimg = value;
            var hiddens = document.querySelectorAll('[id^="hdnimage"]');
            for (var i = 0, len = hiddens.length; i < len; i++) {
                if (i == index) {
                    hiddens[i].value = value;
                }
            }
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
        var max = 5, init = true, images = null, imgObj = null;
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
} ]).constant('globalData', { 'sport': sport, 'baseURL': baseurl, 'limit': (5 - (jsondata != null ? jsondata.items.length : 0)), 'exists': jsondata, 'path': 'http://anil/gettyclasses/superevent/' })