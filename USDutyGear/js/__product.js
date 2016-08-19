var ctrl = {};

ctrl.getModel = function () {
    var model = ctrl.vm.Model;

    if (ctrl.vm.selectedFinish())
        model += "-" + ctrl.vm.selectedFinish().Model;

    if (ctrl.vm.selectedSize())
        model += "-" + ctrl.vm.selectedSize().Model;

    if (ctrl.vm.selectedSnap())
        model += "-" + ctrl.vm.selectedSnap().Model;

    if (ctrl.vm.selectedPackage())
        model += "-" + ctrl.vm.selectedPackage().Model;

    return model;
};

ctrl.calculatePrice = function () {
    // base price
    var price = ctrl.vm.Price;

    // get selected finish
    if (_.isFunction(this.selectedFinish) && this.selectedFinish())
        price += this.selectedFinish().PriceAdjustment;

    // get selected size
    if (_.isFunction(this.selectedSize) && this.selectedSize())
        price += this.selectedSize().PriceAdjustment;

    // get selected snap
    if (_.isFunction(this.selectedSnap) && this.selectedSnap())
        price += this.selectedSnap().PriceAdjustment;

    // get selected package
    if (_.isFunction(this.selectedPackage) && this.selectedPackage())
        price += this.selectedPackage().PriceAdjustment;

    return '$' + price.toString();
};

ctrl.setImageList = function () {
    // construct the selected model
    var model = ctrl.getModel();

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

ctrl.addToCart = function() {
    // get current model number
    var model = ctrl.getModel();

    // get the quantity
    var quantity = ctrl.vm.quantity();

    if (quantity < 1)
        return;

    // add the model to the global shopping cart
    rootCtrl.addToCart(model, quantity);
};

ctrl.init = function (vm) {
    ctrl.vm = vm;

    // server side stuff
    ctrl.vm.Finishes = ko.observableArray(ctrl.vm.Finishes);
    ctrl.vm.Sizes = ko.observableArray(ctrl.vm.Sizes);
    ctrl.vm.Snaps = ko.observableArray(ctrl.vm.Snaps);
    ctrl.vm.Packages = ko.observableArray(ctrl.vm.Packages);
    // client side only
    ctrl.vm.imageList = ko.observableArray();
    ctrl.vm.selectedFinish = ko.observable(ctrl.vm.Finishes()[0]);
    ctrl.vm.selectedSize = ko.observable(ctrl.vm.Sizes()[0]);
    ctrl.vm.selectedSnap = ko.observable(ctrl.vm.Snaps()[0]);
    ctrl.vm.selectedPackage = ko.observable(ctrl.vm.Packages()[0]);
    ctrl.vm.selectedImage = ko.observable();
    ctrl.vm.quantity = ko.observable(1);
    // functions
    ctrl.vm.calculatePrice = ko.computed(ctrl.calculatePrice, ctrl.vm);
    ctrl.vm.setImageList = ko.computed(ctrl.setImageList, ctrl.vm);
    ko.applyBindings(ctrl.vm, $("#product-section")[0]);

    ctrl.setImageList();
    ctrl.calculatePrice();
};

