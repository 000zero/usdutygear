// UPS access key 0D1529428864B0D8

var ctrl = {};

ctrl.doUpsStuff = function () {
    var validation = ctrl.validateShippingAddress();
    if (!validation.valid) {
        // set error message notification
        ctrl.vm.shipping.ShowError(true);
        ctrl.vm.shipping.ErrorMessage(validation.error);
        return;
    }

    ctrl.vm.shipping.ShowError(false);

    var request = {
        "Name": ctrl.vm.cart.Name(),
        "Address": {
            "AddressLine": [ctrl.vm.cart.Street()],
            "City": ctrl.vm.cart.City(),
            "StateProvinceCode": ctrl.vm.cart.State(),
            "PostalCode": ctrl.vm.cart.Zip(),
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
    var validation = ctrl.validateShippingAddress();
    if (!validation.valid) {
        // set error message notification
        ctrl.vm.shipping.ShowError(true);
        ctrl.vm.shipping.ErrorMessage(validation.error);
        return;
    }

    ctrl.vm.shipping.ShowError(false);
    // all good post trigger the submit
    alert("i would submit");
};

ctrl.removeCartItem = function (model, index) {
    rootCtrl.removeItem(model);
    ctrl.vm.cart.Items.splice(index, 1);
};

ctrl.validateShippingAddress = function () {
    var isValid = true;
    var errorMsg;

    if (!ctrl.vm.cart.Name() || ctrl.vm.cart.Name().trim() == '') {
        isValid = false;
        errorMsg = 'Invalid Name';
    }

    if (!ctrl.vm.cart.Street() || ctrl.vm.cart.Street().trim() == '') {
        isValid = false;
        if (!errorMsg)
            errorMsg = 'Invalid Street';
        else
            errorMsg += ', Street';
    }
        
    if (!ctrl.vm.cart.City() || ctrl.vm.cart.City().trim() == '') {
        isValid = false;
        if (!errorMsg)
            errorMsg = 'Invalid City';
        else
            errorMsg += ', City';
    }

    if (!ctrl.vm.cart.State() || ctrl.vm.cart.State().trim() == '') {
        isValid = false;
        if (!errorMsg)
            errorMsg = 'Invalid State';
        else
            errorMsg += ', State';
    }

    if (!ctrl.vm.cart.Zip() || ctrl.vm.cart.Zip().trim() == '') {
        isValid = false;
        if (!errorMsg)
            errorMsg = 'Invalid Zip';
        else
            errorMsg += ', Zip';
    }

    return { valid: isValid, error: '*' + errorMsg };
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
                    return (this.Quantity() * this.Price).toFixed(2);
                }, item);

                return item;
            }));
        }

        // track the shipping address info
        ctrl.vm.cart.Name = ko.observable(ctrl.vm.cart.Name);
        ctrl.vm.cart.Street = ko.observable(ctrl.vm.cart.Street);
        ctrl.vm.cart.City = ko.observable(ctrl.vm.cart.City);
        ctrl.vm.cart.State = ko.observable(ctrl.vm.cart.State);
        ctrl.vm.cart.Zip = ko.observable(ctrl.vm.cart.Zip);

        // only the total of the items
        ctrl.vm.cart.SubTotal = ko.pureComputed(function() {
            var subTotal = _.reduce(this.cart.Items(), function (memo, item) {
                return memo + parseFloat(item.TotalFn());
            }, 0);

            return '$' + subTotal.toFixed(2);
        }, ctrl.vm);

        // total of the items plus shipping and tax
        ctrl.vm.cart.GrandTotal = ko.pureComputed(function () {
            var grandTotal = _.reduce(this.cart.Items(), function (memo, item) {
                return memo + parseFloat(item.TotalFn());
            }, 0);

            if (this.shipping.SelectedRate())
                grandTotal += this.shipping.SelectedRate().Charge;

            return '$' + grandTotal.toFixed(2);
        }, ctrl.vm);

        // setup shipping view model
        ctrl.vm.shipping = {
            Options: ko.observableArray(),
            SelectedRate: ko.observable(),
            ShowError: ko.observable(false),
            ErrorMessage: ko.observable()
        };
        ko.applyBindings(ctrl.vm);
    });
};

ctrl.init();