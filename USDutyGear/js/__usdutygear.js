// cart is an object with parameters named of the sku's with the value being the quantity
var rootCtrl = (function () {
    var scope = this;

    // get the cart from local storage if present
    scope.cart = JSON.parse(localStorage.getItem('usdutygear-cart'));

    if (!scope.cart)
        scope.cart = {};

    // public methods
    return {
        addToCart: function (sku, quantity) {
            if (scope.cart[sku])
                scope.cart[sku] += quantity;
            else
                scope.cart[sku] = quantity;
        },
        emptyCart: function () {
            scope.cart = {};
        },
        getCart: function () {
            return _.clone(scope.cart);
        },

    };
})();
