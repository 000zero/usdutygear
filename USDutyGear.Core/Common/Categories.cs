using System.Collections.Generic;

namespace USDutyGear.Core.Common
{
    // TODO: do we need this?
    //public static class Categories
    //{
    //    public const string LinerBelt = "Liner Belt";
    //    public const string Belt = "Belt";
    //    public const string SamBrowneBelt = "Sam Brown Belt";
    //    public const string DutyBelt = "Duty Belt";
    //    public const string Keeper = "Keeper";
    //    public const string KeyHolderKeeper = "Key Holder Keeper";
    //    public const string GloveHolder = "Glove Holder";
    //    public const string MagazineHolder = "Magazine Holder";
    //}

    public class ProductCategory
    {
        public string Category { get; set; }
        public List<KeyValuePair<string, string>> Products { get; set; }
    }
}
