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
            items: {},
            lastRead: null,
            lastWrite: moment()
        };

    scope.saveCart = function () {
        localStorage.setItem('usdutygear-cart', JSON.stringify(scope.cart));
    };

    scope.setCartViewModel = function () {
        scope.cartViewModel.items.removeAll();
        for (var prop in scope.cart.items) {
            if (scope.cart.items.hasOwnProperty(prop)) {
                scope.cartViewModel.items.push({
                    Model: prop,
                    Quantity: scope.cart.items[prop]
                });
            }
        }
    };

    // setup global viewModel TODO: figure out if we still need this
    scope.cartViewModel = {};
    scope.cartViewModel.items = ko.observableArray();
    scope.setCartViewModel();

    //ko.applyBindings(scope.cartViewModel, $("#shopping-cart-nav-form")[0]);

    // public methods
    return {
        addToCart: function (model, quantity) {
            if (!scope.cart.items)
                scope.cart.items = {};

            if (scope.cart.items[model]) {
                scope.cart.items[model] += parseInt(quantity);
            } else {
                scope.cart.items[model] = parseInt(quantity);
            }

            scope.cart.lastWrite = moment();
            scope.setCartViewModel();
            scope.saveCart();
        },
        emptyCart: function () {
            scope.cart = {
                items: {},
                lastRead: null,
                lastWrite: moment()
            };
            scope.saveCart();
        },
        updateQuantity: function (model, quantity) {
            if (scope.cart.items[model])
                scope.cart.items[model] = parseInt(quantity);

            scope.cart.lastWrite = moment();
            scope.setCartViewModel();
            scope.saveCart();
        },
        removeItem: function (model) {
            if (scope.cart.items[model])
                delete scope.cart.items[model];

            scope.cart.lastWrite = moment();
            scope.setCartViewModel();
            scope.saveCart();
        },
        getCart: function () {
            var copy = _.clone(scope.cart);
            scope.cart.lastRead = moment();

            return copy;
        },
        getCartItems: function () {
            var items = _.map(_.pairs(scope.cart.items), function (pair) {
                return {
                    Model: pair[0],
                    Quantity: parseInt(pair[1])
                };
            });
            scope.cart.lastRead = moment();

            return items;
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


var httpService = (function() {
    return {
        postJSON: function(url, data) {
            return $.ajax({
                url: url,
                type: "POST",
                data: JSON.stringify(data),
                contentType: "application/json",
                dataType: "json"
            });
        }
    };
})();