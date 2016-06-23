var ctrl = {};

ctrl.init = function (vm) {
    ctrl.vm = vm;

    ko.applyBindings(ctrl.vm);
};

// ready function
$(function () {
    var test = "test";
});

//productController.init();

