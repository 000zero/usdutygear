// UPS access key 0D1529428864B0D8

var ctrl = {};

ctrl.doUpsStuff = function () {
    var request = {
        "UPSSecurity": {
            "UsernameToken": {
                "Username": "usdutygear1",
                "Password": "Flores2016"
            },
            "ServiceAccessToken": {
                "AccessLicenseNumber": "0D1529428864B0D8"
            }
        },
        "RateRequest": {
            "Request": {
                "RequestOption": "Shop",
                "TransactionReference": {
                    "CustomerContext": "YourCustomerContext"
                }
            },
            "Shipment": {
                "Shipper": {
                    "Name": "US DUTY GEAR",
                    "ShipperNumber": "1158Y8",
                    "Address": {
                        "AddressLine": ["2131 S Hellman Ave ", "UNIT D"],
                        "City": "Ontario",
                        "StateProvinceCode": "CA",
                        "PostalCode": "91761",
                        "CountryCode": "US"
                    }
                },
                "ShipTo": {
                    "Name": "Steven Garcia",
                    "Address": {
                        "AddressLine": ["6 Andalusia "],
                        "City": "Rancho Santa Margarita",
                        "StateProvinceCode": "CA",
                        "PostalCode": "92688",
                        "CountryCode": "US",
                        "ResidentialAddressIndicator": "True"
                    }
                },
                "Package": {
                    "PackagingType": {
                        "Code": "02",
                        "Description": "Rate"
                    },
                    "Dimensions": {
                        "UnitOfMeasurement": {
                            "Code": "IN",
                            "Description": "inches"
                        },
                        "Length": "5",
                        "Width": "4",
                        "Height": "3"
                    },
                    "PackageWeight": {
                        "UnitOfMeasurement": {
                            "Code": "Lbs",
                            "Description": "pounds"
                        },
                        "Weight": "1"
                    }
                },
                "ShipmentRatingOptions": {
                    "NegotiatedRatesIndicator": ""
                }
            }
        }
    };

    $.ajax({
        url: "https://wwwcie.ups.com/rest/Rate",
        type: "POST",
        data: JSON.stringify(request),
        contentType: "application/json",
        dataType: "json"
    }).then(function (response) {
        if (response) {
            // build the shipping options view model
            ctrl.vm.shipping().Options.removeAll();
            _.each(response.RateResponse.RatedShipment, function (shippingOption) {
                ctrl.vm.shipping().Options.push({
                    ServiceCode: shippingOption.Service.Code,
                    Service: consts.UPSServiceCodes[shippingOption.Service.Code],
                    Charge: shippingOption.TotalCharges.MonetaryValue
                });
            });
        }
    });
};

ctrl.removeCartItem = function (model, index) {
    rootCtrl.removeItem(model);
    ctrl.vm.cart.items.splice(index, 1);
};

ctrl.init = function () {
    ctrl.vm = {
        cart: {},
        shipping: {}
    };

    // get cart from the root controller
    $.ajax({
        url: "/api/cart",
        type: "POST",
        data: JSON.stringify({ Items: rootCtrl.getCartItems() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).then(function (response) {
        //ctrl.vm.cart = ko.observable();

        if (!response.Items) {
            ctrl.vm.cart.items = ko.observableArray();
            ctrl.vm.cart.total = ko.observable(0);
        } else {// set the cart view model here
            ctrl.vm.cart.items = ko.observableArray(response.Items);
            ctrl.vm.cart.total = ko.computed(function () {
                return _.reduce(this.ctrl.vm.cart.items(), function (memo, item) {
                    return memo + item.Total;
                }, 0);
            });
        }

        // apply view model binding
        //ctrl.vm.cart = ko.observable(ctrl.vm.cart);

        // setup shipping view model
        ctrl.vm.shipping = ko.observable({
            Options: ko.observableArray(),
            SelectedRate: ko.observable()
        });
        ko.applyBindings(ctrl.vm);
    });
};

ctrl.init();