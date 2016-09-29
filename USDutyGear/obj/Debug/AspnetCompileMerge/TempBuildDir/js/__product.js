var ctrl = {};

ctrl.getModel = function () {
    var model = ctrl.vm.Model;

    if (ctrl.vm.selectedFinish())
        model += "-" + ctrl.vm.selectedFinish().Model;

    if (ctrl.vm.selectedSnap())
        model += "-" + ctrl.vm.selectedSnap().Model;

    if (ctrl.vm.selectedBuckle())
        model += "-" + ctrl.vm.selectedBuckle().Model;

    if (ctrl.vm.selectedInnerLiner())
        model += "-" + ctrl.vm.selectedInnerLiner().Model;

    if (ctrl.vm.selectedSize())
        model += "-" + ctrl.vm.selectedSize().Model;

    if (ctrl.vm.selectedPackage() && ctrl.vm.selectedPackage().Model)
        model += "-" + ctrl.vm.selectedPackage().Model;

    if (model.slice(-1) === '-')
        model = model.slice(0, -1);

    ctrl.vm.model(model);
    return model;
};

ctrl.calculatePrice = function () {
    var model = ctrl.getModel();

    price = ctrl.vm.Prices[model]
        ? ctrl.vm.Prices[model]
        : 0;

    return '$' + price.toFixed(2).toString();
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
            ctrl.vm.selectedImage(list[0]);
            ctrl.selectImage(list[0]);
            break;
        }

        // find the last - and remove that part of the string
        model = model.substring(0, model.lastIndexOf("-"));
    }
};

ctrl.selectImage = function (imageName) {
    
    ctrl.vm.selectedImage(imageName);
    setTimeout(function() {
        $(".main-image").trigger("zoom.destroy");
        $(".main-image").zoom();
    }, 100);
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
    ctrl.vm.Buckles = ko.observableArray(ctrl.vm.Buckles);
    ctrl.vm.InnerLiners = ko.observableArray(ctrl.vm.InnerLiners);
    ctrl.vm.Packages = ko.observableArray(ctrl.vm.Packages);
    // client side only
    ctrl.vm.imageList = ko.observableArray();
    ctrl.vm.selectedFinish = ko.observable(ctrl.vm.Finishes()[0]);
    ctrl.vm.selectedSize = ko.observable(ctrl.vm.Sizes()[0]);
    ctrl.vm.selectedSnap = ko.observable(ctrl.vm.Snaps()[0]);
    ctrl.vm.selectedBuckle = ko.observable(ctrl.vm.Buckles()[0]);
    ctrl.vm.selectedInnerLiner = ko.observable(ctrl.vm.InnerLiners()[0]);
    ctrl.vm.selectedPackage = ko.observable(ctrl.vm.Packages()[0]);
    ctrl.vm.selectedImage = ko.observable();
    ctrl.vm.quantity = ko.observable("1");
    ctrl.vm.model = ko.observable();
    // functions
    ctrl.vm.calculatePrice = ko.computed(ctrl.calculatePrice, ctrl.vm);
    ctrl.vm.setImageList = ko.computed(ctrl.setImageList, ctrl.vm);
    ko.applyBindings(ctrl.vm, $("#product-section")[0]);

    ctrl.setImageList();
    ctrl.calculatePrice();
    ctrl.getModel();

    $(".main-image").zoom();
};

