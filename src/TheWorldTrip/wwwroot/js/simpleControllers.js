// simpleControllers.js

(function () {
    "use strict";

    angular.module("simpleController", [])
        .directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
            templateUrl: "/views/waitCursor.html"
        };
    };

})(); 