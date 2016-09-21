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
                case "keyringkeeper": return ProductNames.KeyRingKeeper;
                case "glovepouch": return ProductNames.GlovePouch;
                case "magazinepouch": return ProductNames.MagazineHolder;
                case "beltkeeperclip": return ProductNames.BeltKeeperClip;
                case "nylonbelt15": return ProductNames.NylonBelt15;
                case "nylonbelt2": return ProductNames.NylonBelt2;
                case "nylonbeltkeeper": return ProductNames.NylonBeltKeeper;
                case "insidebelt": return ProductNames.InsideBelt;
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
                case ProductNames.KeyRingKeeper: return "keyringkeeper";
                case ProductNames.GlovePouch: return "glovepouch";
                case ProductNames.MagazineHolder: return "magazinepouch";
                case ProductNames.BeltKeeperClip: return "beltkeeperclip";
                case ProductNames.NylonBelt15: return "nylonbelt15";
                case ProductNames.NylonBelt2: return "nylonbelt2";
                case ProductNames.NylonBeltKeeper: return "nylonbeltkeeper";
                case ProductNames.InsideBelt: return "insidebelt";
                default: return null;
            }
        }
    }
}