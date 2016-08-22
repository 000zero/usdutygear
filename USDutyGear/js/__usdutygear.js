// cart is an object with parameters named of the sku's with the value being the quantity
var rootCtrl = (function () {
    var scope = this;

    // get the cart from local storage if present
    try {
        scope.cart = JSON.parse(localStorage.getItem('usdutygear-cart'));
    } catch (e) {
        console.log(e);
    }

    if (!scope.cart)
        scope.cart = {
            products: {},
            lastRead: null,
            lastWrite: moment()
        };

    scope.saveCart = function() {
        localStorage.setItem('usdutygear-cart', JSON.stringify(scope.cart));
    };

    scope.setCartViewModel = function () {
        scope.cartViewModel.products.removeAll();
        for (var prop in scope.cart.products) {
            if (scope.cart.products.hasOwnProperty(prop)) {
                scope.cartViewModel.products.push({
                    Model: prop,
                    Quantity: scope.cart.products[prop]
                });
            }
        }
    };

    // setup global viewModel
    scope.cartViewModel = {};
    scope.cartViewModel.products = ko.observableArray();
    scope.setCartViewModel();

    ko.applyBindings(scope.cartViewModel, $("#shopping-cart-nav-form")[0]);

    // public methods
    return {
        addToCart: function (model, quantity) {
            if (scope.cart.products[model]) {
                scope.cart.products[model] += quantity;
            } else {
                scope.cart.products[model] = quantity;
            }

            scope.cart.lastWrite = moment();
            scope.setCartViewModel();
            scope.saveCart();
        },
        emptyCart: function () {
            scope.cart = {
                products: {},
                lastRead: null,
                lastWrite: moment()
            };
            scope.saveCart();
        },
        getCart: function () {
            var copy = _.clone(scope.cart);
            scope.cart.lastRead = moment();

            return copy;
        },
        getCartViewModel: function () {
            return scope.cartViewModel;
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
