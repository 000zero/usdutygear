var ctrl = {};
ctrl.selectedImage = null;

ctrl.setSelectedImage = function (imageName) {
    ctrl.vm.SelectedImage(imageName);
};

ctrl.init = function (vm) {
    ctrl.vm = vm;

    ctrl.vm.SelectedImage = ko.observable(ctrl.vm.SelectedImage);
    ko.applyBindings(ctrl.vm);
};

