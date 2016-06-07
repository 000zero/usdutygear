using System.Collections.Generic;

namespace USDutyGear.Core.Common
{
    public static class Finishes
    {
        public const string Karma = "Karma";
        public const string BasketWeave = "Basketweave";
        public const string Leather = "Leather";
        public const string Nylon = "Nylon";
        public const string HighGloss = "High Gloss";
        public const string Boltron = "Boltron";

        public static Dictionary<string, string> ImagePaths = new Dictionary<string, string>
        {
            { Karma, "images/leather_sample.png" },
            { BasketWeave, "images/basketweave_sample.png" },
            { Leather, "images/leather_sample.png" },
            { HighGloss, "images/higloss_sample.png" },
            { Nylon, "images/nylon_sample.png" },
            { Boltron, "images/boltron_sample.png" }
        };
    }
}
