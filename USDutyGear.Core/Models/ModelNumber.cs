using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using USDutyGear.Core.Common;

namespace USDutyGear.Core.Models
{
    public class ModelNumber
    {
        public string Model { get; set; }
        public string Finish { get; set; }
        public string Size { get; set; }
        public string Snap { get; set; }
        public string Buckle { get; set; }
        public string InnerLiner { get; set; }
        public string Package { get; set; }

        public List<string> AdjustmentModels
        {
            get { return (new[] {Finish, Buckle, Snap, InnerLiner, Size, Package}).Where(x => !string.IsNullOrWhiteSpace(x)).ToList(); }
        }

        public static ModelNumber Create(Regex regex, string model)
        {
            // get the product model
            var match = regex.Match(model);

            if (!match.Success)
                return null;

            var modelNumber = new ModelNumber {Model = match.Groups["Model"].Value};
            var groups = regex.GetGroupNames();

            if (groups.Any(x => x == ProductAdjustmentTypes.Finish))
                modelNumber.Finish = match.Groups[ProductAdjustmentTypes.Finish].Value;

            if (groups.Any(x => x == ProductAdjustmentTypes.Snap))
                modelNumber.Snap = match.Groups[ProductAdjustmentTypes.Snap].Value;

            if (groups.Any(x => x == ProductAdjustmentTypes.Buckle))
                modelNumber.Buckle = match.Groups[ProductAdjustmentTypes.Buckle].Value;

            if (groups.Any(x => x == ProductAdjustmentTypes.InnerLiner.Replace(" ", "")))
                modelNumber.InnerLiner = match.Groups[ProductAdjustmentTypes.InnerLiner.Replace(" ", "")].Value;

            if (groups.Any(x => x == ProductAdjustmentTypes.Size))
                modelNumber.Size = match.Groups[ProductAdjustmentTypes.Size].Value;

            if (groups.Any(x => x == ProductAdjustmentTypes.Package))
                modelNumber.Package = match.Groups[ProductAdjustmentTypes.Package].Value;

            return modelNumber;
        }
    }
}
