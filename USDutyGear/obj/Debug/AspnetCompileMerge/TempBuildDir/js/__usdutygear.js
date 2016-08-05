// cart is an object with parameters named of the sku's with the value being the quantity
var rootCtrl = (function () {
    var scope = this;

    // get the cart from local storage if present
    scope.cart = JSON.parse(localStorage.getItem('usdutygear-cart'));

    if (!scope.cart)
        scope.cart = {
            lastRead: null,
            lastWrite: moment()
        };

    // public methods
    return {
        addToCart: function (sku, quantity) {
            if (scope.cart[sku])
                scope.cart[sku] += quantity;
            else
                scope.cart[sku] = quantity;

            scope.cart.lastWrite = moment();
        },
        emptyCart: function () {
            scope.cart = {
                lastRead: null,
                lastWrite: moment()
            };
        },
        getCart: function () {
            var copy = _.clone(scope.cart);
            scope.cart.lastRead = moment();

            return copy;
        },
        getFinishImageName: function (finish) {
            switch (finish) {
                case 'Basketweave':
                    return 'basketweave_sample.png';
                case 'High Gloss':
                    return 'highgloss_sample.png';
                case 'Leather':
                    return 'leather_sample.png';
                default:
                    return '';
            }
        }
    };
})();
