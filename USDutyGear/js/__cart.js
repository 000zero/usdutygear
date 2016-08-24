var ctrl = {};

ctrl.init = function() {
    // get cart from the root controller
    $.post("/api/cart", { cart: rootCtrl.getCart() }).then(function(response) {
        // set the cart view model here

        // apply view model binding
    });

    // make api call
};