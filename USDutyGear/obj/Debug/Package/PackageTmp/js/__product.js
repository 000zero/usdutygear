var ctrl = {};

ctrl.calculatePrice = function () {
    // base price
    var price = ctrl.vm.Price;

    // get selected finish
    if (this.selectedFinish())
        price += this.selectedFinish().PriceAdjustment;

    // get selected size
    if (this.selectedSize())
        price += this.selectedSize().PriceAdjustment;

    // get selected snap
    if (this.selectedSnap())
        price += this.selectedSnap().PriceAdjustment;

    return '$' + price.toString();
};

ctrl.setImageList = function () {
    // construct the selected model
    var model = ctrl.vm.Model;

    if (ctrl.vm.selectedFinish())
        model += "-" + ctrl.vm.selectedFinish().Model;

    if (ctrl.vm.selectedSize())
        model += "-" + ctrl.vm.selectedSize().Model;

    if (ctrl.vm.selectedSnap())
        model += "-" + ctrl.vm.selectedSnap().Model;

    while (model.length > 0) {
        if (ctrl.vm.Images[model]) {
            var list = ctrl.vm.Images[model];

            ctrl.vm.imageList.removeAll();
            for (var i = 0; i < list.length; i++) {
                ctrl.vm.imageList.push(list[i]);
            }
            //ctrl.vm.imageList = ko.observableArray(ctrl.vm.Images[model]);
            ctrl.vm.selectedImage(list[0]);
            break;
        }

        // find the last - and remove that part of the string
        model = model.substring(0, model.lastIndexOf("-"));
    }
};

ctrl.selectImage = function (imageName) {

};

ctrl.init = function (vm) {
    ctrl.vm = vm;

    ctrl.vm.Finishes = ko.observableArray(ctrl.vm.Finishes);
    ctrl.vm.Sizes = ko.observableArray(ctrl.vm.Sizes);
    ctrl.vm.Snaps = ko.observableArray(ctrl.vm.Snaps);
    ctrl.vm.imageList = ko.observableArray();
    ctrl.vm.selectedFinish = ko.observable(ctrl.vm.Finishes()[0]);
    ctrl.vm.selectedSize = ko.observable(ctrl.vm.Sizes()[0]);
    ctrl.vm.selectedSnap = ko.observable(ctrl.vm.Snaps()[0]);
    ctrl.vm.selectedImage = ko.observable();
    ctrl.vm.totalPrice = ko.observable();
    ctrl.vm.calculatePrice = ko.computed(ctrl.calculatePrice, ctrl.vm);
    ko.applyBindings(ctrl.vm);

    ctrl.setImageList();
    //ctrl.calculatePrice();
};

