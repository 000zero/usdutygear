using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDutyGear.UPS.Models
{
    public class UPSFreightRateRequest
    {
        public UPSSecurity UPSSecurity { get; set; }
        public UPSFreightRateRequestOptions FreightRateRequest { get; set; }
        public UPSShippingInfo ShipFrom { get; set; }
        public string ShipperNumber { get; set; }
        public UPSShippingInfo ShipTo { get; set; }
        public UPSPaymentInfo PaymentInformation { get; set; }
        // Service
        // HandlingUnitOne
        // Commodity
        // ShipmentServiceOption
        // DensityEligibleIndicator
        // AdjustedWeightIndicator
        // HandlingUnitWeight
        // AlternateRateOptions
        // PickupRequest
        // TimeInTransitIndicator
        // GFPOptions
    }

    public class UPSSecurity
    {
        public UPSUserToken UsernameToken { get; set; }
        public UPSServiceAccessToken ServiceAccessToken { get; set; }
    }

    public class UPSUserToken
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UPSServiceAccessToken
    {
        public string AccessLicenseNumber { get; set; }
    }


    public class UPSFreightRateRequestOptions
    {
        public UPSRequest Request { get; set; }
    }

    public class UPSRequest
    {
        public string RequestOption { get; set; }
        public UPSTransactionRef TransactionReference { get; set; }
    }

    public class UPSTransactionRef
    {
        public string TransactionIdentifier { get; set; }
    }

    public class UPSShippingInfo
    {
        public string Name { get; set; }
        public UPSAddress Address { get; set; }
        public string AttentionName { get; set; }
        public UPSPhone Phone { get; set; }
        public string EMailAddress { get; set; }
    }

    public class UPSAddress
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }

    public class UPSPhone
    {
        public string Number { get; set; }
        public string Extension { get; set; }
    }

    public class UPSPaymentInfo
    {
        public UPSPayer Payer { get; set; }
        public UPSShipmentBillingOption ShipmentBillingOption { get; set; }
    }

    public class UPSPayer
    {
        public string Name { get; set; }
        public UPSAddress Address { get; set; }
        public string ShippingNumber { get; set; }
        public string AccountType { get; set; }
        public string AttentionName { get; set; }
        public UPSPhone Phone { get; set; }
        public string EMailAddress { get; set; }
    }

    public class UPSShipmentBillingOption
    {
        public string Code { get; set; }
    }

    public class UPSService
    {
        public string Code { get; set; }
    }
}
