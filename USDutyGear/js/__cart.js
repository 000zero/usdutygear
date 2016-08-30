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
        if (!response.data) {
            ctrl.vm.items = ko.observableArray();
            ctrl.vm.total = ko.observable(0);
        } else {// set the cart view model here
            ctrl.vm.items = ko.observableArray(response.data.Items);
            ctrl.vm.total = ko.observable(response.data.CartTotal);
        }

        // apply view model binding
        ko.applyBindings(ctrl.vm, $("#shopping-cart-list-form")[0]);
    });
};

ctrl.init();