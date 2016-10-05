namespace USDutyGear.TaxCloud.Models
{
    public class CaptureResponse : TaxCloudResponse
    {
        public CaptureResponse()
        {
            Success = false;
        }

        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
