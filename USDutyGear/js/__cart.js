// UPS access key 0D1529428864B0D8

var ctrl = {};



ctrl.getUpsShippingRates = function() {
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

    httpService.postJSON("/api/shipping/rates", request).then(function (response) {
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

ctrl.verifyTaxAddress = function (address) {
    return httpService.postJSON("/api/taxes/address/verify", address);
};

ctrl.getSalesTax = function () {
    var destination = {
        "Address1": ctrl.vm.cart.Street(),
        "Address2": "",
        "City": ctrl.vm.cart.City(),
        "State": ctrl.vm.cart.State(),
        "Zip5": ctrl.vm.cart.Zip(),
        "Zip4": ""
    };

    ctrl.verifyTaxAddress(destination).then(function(response) {
        if (response.ErrNumber != "0") {
            // TODO: return error promise
        }

        // TODO: test if the address verified is good
        var verifiedTaxAddress = {
            "Address1": response.Address1,
            "Address2": response.Address2,
            "City": response.City,
            "State": response.State,
            "Zip5": response.Zip5,
            "Zip4": response.Zip4
        };

        var cartIndex = 0;
        var request = {
            destination: verifiedTaxAddress,
            cartItems: _.map(ctrl.vm.cart.Items(), function (item) {
                return {
                    "Qty": item.Quantity(),
                    "Price": item.Price,
                    "TIC": "00000",
                    "ItemID": item.Model,
                    "Index": cartIndex++
                };
            })
        };

        httpService.postJSON("/api/taxes/sales", request).then(function(response) {
            if (response.Success)
                ctrl.vm.cart.TaxAmount(response.TaxAmount);
        });
    });
};

ctrl.doUpsStuff = function () {
    var validation = ctrl.validateShippingAddress();
    if (!validation.valid) {
        // set error message notification
        ctrl.vm.shipping.ShowError(true);
        ctrl.vm.shipping.ErrorMessage(validation.error);
        return;
    }

    ctrl.getUpsShippingRates();
    ctrl.getSalesTax();
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
        ctrl.vm.cart.Email = ko.observable(ctrl.vm.cart.Email);
        ctrl.vm.cart.Name = ko.observable(ctrl.vm.cart.Name);
        ctrl.vm.cart.Street = ko.observable(ctrl.vm.cart.Street);
        ctrl.vm.cart.City = ko.observable(ctrl.vm.cart.City);
        ctrl.vm.cart.State = ko.observable(ctrl.vm.cart.State);
        ctrl.vm.cart.Zip = ko.observable(ctrl.vm.cart.Zip);

        ctrl.vm.cart.TaxAmount = ko.observable();

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

            if (this.cart.TaxAmount())
                grandTotal += this.cart.TaxAmount();

            if (this.shipping.SelectedRate())
                grandTotal += this.shipping.SelectedRate().Charge;

            return '$' + grandTotal.toFixed(2);
        }, ctrl.vm);

        ctrl.vm.cart.isShippingAddressValid = ko.pureComputed(function() {
            return this.cart.Name() && this.cart.Name().trim() != '' &&
                this.cart.Street() && this.cart.Street().trim() != '' &&
                this.cart.City() && this.cart.City().trim() != '' &&
                this.cart.State() && this.cart.State().trim() != '' &&
                this.cart.Zip() && this.cart.Zip().trim() != '';
        }, ctrl.vm);

        ctrl.vm.cart.isEmailValid = ko.pureComputed(function() {
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return this.cart.Email() && re.test(this.cart.Email());
        }, ctrl.vm);

        ctrl.vm.cart.isCheckoutReady = ko.pureComputed(function() {
            var isShippingInfoValid = this.cart.isShippingAddressValid();
            var isEmailValid = this.cart.isEmailValid();

            return isShippingInfoValid && isEmailValid;
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