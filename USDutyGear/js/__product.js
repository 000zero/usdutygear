var ctrl = {};
ctrl.selectedImage = null;
ctrl.price = null;

ctrl.setSelectedImage = function (imageName) {
    ctrl.vm.SelectedImage(imageName);
};

ctrl.calculatePrice = function () {
    // get product price

    // get selected finish

    // get selected size
};

ctrl.init = function (vm) {
    ctrl.vm = vm;

    ctrl.vm.SelectedImage = ko.observable(ctrl.vm.SelectedImage);
    ko.applyBindings(ctrl.vm);
};

