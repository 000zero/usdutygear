namespace USDutyGear.UPS.Models
{
    public class LabelSpecs
    {
        public LabelSpecs()
        {
            LabelImageFormat = new CodeSet();
        }

        public CodeSet LabelImageFormat { get; set; }
        public string HTTPUserAgent { get; set; }
    }
}
