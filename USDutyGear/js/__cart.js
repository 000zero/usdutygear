// UPS access key 0D1529428864B0D8

var ctrl = {};

ctrl.doUpsStuff = function () {
    var request = {
        "Name": "Steven Garcia",
        "Address": {
            "AddressLine": ["6 Andalusia "],
            "City": "Rancho Santa Margarita",
            "StateProvinceCode": "CA",
            "PostalCode": "92688",
            "CountryCode": "US",
            "ResidentialAddressIndicator": "True"
        }
    };

    $.ajax({
        url: "/api/shipping/rates",
        type: "POST",
        data: JSON.stringify(request),
        contentType: "application/json",
        dataType: "json"
    }).then(function (response) {
        if (response) {
            // build the shipping options view model
            ctrl.vm.shipping.Options.removeAll();
            _.each(response.RatedShipment, function (shippingOption) {
                ctrl.vm.shipping.Options.push({
                    ServiceCode: shippingOption.Service.Code,
                    Service: consts.UPSServiceCodes[shippingOption.Service.Code],
                    Charge: shippingOption.TotalCharges.MonetaryValue
                });
            });
        }
    });
};

ctrl.checkout = function () {
    // TODO: convert observable computed values back to their value before sending to the server
};

ctrl.removeCartItem = function (model, index) {
    rootCtrl.removeItem(model);
    ctrl.vm.cart.Items.splice(index, 1);
};

ctrl.init = function () {
    ctrl.vm = {
        cart: {},
        shipping: {}
    };

    // post the cart to get the prices populated from the server
    $.ajax({
        url: "/api/cart",
        type: "POST",
        data: JSON.stringify({ Items: rootCtrl.getCartItems() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).then(function (response) {
        ctrl.vm.cart = response;

        if (!ctrl.vm.cart.Items) {
            ctrl.vm.cart.Items = ko.observableArray();
        } else {// set the cart view model here
            ctrl.vm.cart.Items = ko.observableArray(_.map(ctrl.vm.cart.Items, function (item) {
                item.Quantity = ko.observable(item.Quantity);
                item.TotalFn = ko.pureComputed(function () {
                    return this.Quantity() * this.Price;
                }, item);

                return item;
            }));
        }

        // only the total of the items
        ctrl.vm.cart.SubTotal = ko.pureComputed(function() {
            var subTotal = _.reduce(this.cart.Items(), function (memo, item) {
                return memo + item.TotalFn();
            }, 0);

            return '$' + subTotal.toFixed(2);
        }, ctrl.vm);

        // total of the items plus shipping and tax
        ctrl.vm.cart.GrandTotal = ko.pureComputed(function () {
            var grandTotal = _.reduce(this.cart.Items(), function (memo, item) {
                return memo + item.TotalFn();
            }, 0);

            if (this.shipping.SelectedRate())
                grandTotal += this.shipping.SelectedRate().Charge;

            return '$' + grandTotal.toFixed(2);
        }, ctrl.vm);

        // setup shipping view model
        ctrl.vm.shipping = {
            Options: ko.observableArray(),
            SelectedRate: ko.observable()
        };
        ko.applyBindings(ctrl.vm);
    });
};

ctrl.init();