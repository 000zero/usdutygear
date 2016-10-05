namespace USDutyGear.Models
{
    public class PayeezyPaymentResultsModel
    {
        public string x_response_code { get; set; }
        public string x_response_subcode { get; set; }
        public string x_response_reason_code { get; set; }
        public string x_response_reason_text { get; set; }
        public string x_auth_code { get; set; }
        public string x_trans_id { get; set; }
        public string x_MD5_Hash { get; set; }
        public string exact_ctr { get; set; }
        public string Client_Email { get; set; }
        public string DollarAmount { get; set; }
        public string Customer_Ref { get; set; }
        public string Reference_No { get; set; }
        public string Transaction_Approved { get; set; }
        public string Transaction_Error { get; set; }
    }
}