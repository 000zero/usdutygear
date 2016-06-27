using USDutyGear.Core.Common;

namespace USDutyGear.Common
{
    public static class CategoryHelper
    {
        public static string MapRouteKeyToProductName(string key)
        {
            switch (key.ToLower())
            {
                case "linerbelt": return ProductNames.LinerBelt;
                case "belt": return ProductNames.Belt;
                case "sambrownebelt": return ProductNames.SamBrowneBelt;
                case "dutybelt": return ProductNames.DutyBelt;
                case "beltkeeper": return ProductNames.BeltKeeper;
                case "keyholderkeeper": return ProductNames.KeyHolderKeeper;
                case "gloveholder": return ProductNames.GloveHolder;
                case "magazineholder": return ProductNames.MagazineHolder;
                default: return null;
            }
        }

        public static string MapProductNameToRouteKey(string name)
        {
            switch (name)
            {
                case ProductNames.LinerBelt: return "linerbelt";
                case ProductNames.Belt: return "belt";
                case ProductNames.SamBrowneBelt: return "sambrownebelt";
                case ProductNames.DutyBelt: return "dutybelt";
                case ProductNames.BeltKeeper: return "beltkeeper";
                case ProductNames.KeyHolderKeeper: return "keyholderkeeper";
                case ProductNames.GloveHolder: return "gloveholder";
                case ProductNames.MagazineHolder: return "magazineholder";
                default: return null;
            }
        }
    }
}