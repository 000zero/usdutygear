var ctrl = {};

ctrl.init = function (vm) {
    ctrl.vm = vm;

    ko.applyBindings(ctrl.vm);
};

// ready function
$(function () {
    // parse product name from URL
    //var tokens = window.location.split('/');
    //var productName = tokens[tokens.length - 1];

    //ctrl.init(productName);
});

//productController.init();

