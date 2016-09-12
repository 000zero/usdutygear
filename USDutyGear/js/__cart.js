// UPS access key 0D1529428864B0D8

var ctrl = {};

ctrl.init = function () {
    ctrl.vm = {};

    // get cart from the root controller
    $.ajax({
        url: "/api/cart",
        type: "POST",
        data: JSON.stringify({ Items: rootCtrl.getCartItems() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).then(function(response) {
        if (!response.Items) {
            ctrl.vm.items = ko.observableArray();
            ctrl.vm.total = ko.observable(0);
        } else {// set the cart view model here
            ctrl.vm.items = ko.observableArray(response.Items);
            ctrl.vm.total = ko.observable(response.CartTotal);
        }

        // apply view model binding
        ko.applyBindings(ctrl.vm, $("#shopping-cart-list-form")[0]);
    });
};

ctrl.init();