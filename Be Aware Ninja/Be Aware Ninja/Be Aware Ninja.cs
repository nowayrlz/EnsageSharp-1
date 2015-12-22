using System;
using System.Linq;
using Ensage;
using Ensage.Common;
using SharpDX;


namespace Be_Aware_Ninja
{
    class Be_Aware_Ninja
    {
        private static Hero me;
        private static System.Collections.Generic.List<Ensage.Hero> enemies;
        private static SideMessage informationmessage;
        static void Main(string[] args)
        {
            Game.OnUpdate += Tick;
            PrintSuccess(string.Format("> Be Aware Loaded!"));
        }
        public static void Tick(EventArgs args)
        {
            if (!Game.IsInGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;
            enemies = ObjectMgr.GetEntities<Hero>().Where(x => me.Team != x.Team && x.IsVisible && !x.IsIllusion && x.IsAlive).ToList();
            if (enemies == null)
                return;
            foreach (var v in enemies)
            {
                string index;
                System.Collections.Generic.IEnumerable<Modifier> mod = v.Modifiers.ToList(); // Ty Jumpering
                if (mod.Any(x => x.Name == "modifier_mirana_moonlight_shadow") && Utils.SleepCheck("mirana")) //mirana
                {
                    MessageCreator("mirana", "mirana_invis");
                    Utils.Sleep(15000,"mirana");
                }
                if (mod.Any(x => x.Name == "modifier_alchemist_unstable_concoction") && Utils.SleepCheck("alch")) //alch
                {
                    MessageCreator("alchemist", "alchemist_unstable_concoction");
                    Utils.Sleep(15000, "alch");
                }
                if (mod.Any(x => x.Name == "modifier_morphling_replicate_timer") && Utils.SleepCheck("morph")) //morph
                {
                    MessageCreator("morphling", "morphling_replicate");
                    Utils.Sleep(15000, "morph");
                }
                //if (mod.Any(x => x.Name == "modifier_ember_spirit_fire_remnant_timer") && Utils.SleepCheck("ember")) // ember
                //{
                //    MessageCreator("ember_spirit", "ember_spirit_fire_remnant");
                //    Utils.Sleep(15000, "ember");
                //}
                if (mod.Any(x => x.Name == "modifier_bloodseeker_thirst_speed") && Utils.SleepCheck("bloodseeker")) //bloodseeker
                {
                    MessageCreator("bloodseeker", "bloodseeker_thirst");
                    Utils.Sleep(15000, "bloodseeker");
                }
                if (mod.Any(x => x.Name == "modifier_invoker_ghost_walk_enemy") && Utils.SleepCheck("invoker")) //invoker
                {
                    MessageCreator("invoker", "invoker_ghost_walk");
                    Utils.Sleep(15000, "invoker");
                }
                if (mod.Any(x => x.Name == "modifier_item_invisibility_edge_windwalk") && Utils.SleepCheck("shadowblade")) //shadow blade
                {
                    index = v.Name.Remove(0, 14);
                    MessageItemCreator(index, "invis_sword");
                    Utils.Sleep(15000, "shadowblade");
                }
                if (mod.Any(x => x.Name == "modifier_rune_invis") && Utils.SleepCheck("modifier_rune_invis")) // InvisRune
                {
                    index = v.Name.Remove(0, 14);
                    MessageRuneCreator(index, "rune_invis");
                    Utils.Sleep(15000, "modifier_rune_invis");
                }
                if (mod.Any(x => x.Name == "modifier_rune_haste") && Utils.SleepCheck("modifier_rune_haste")) // hasteRune
                {
                    index = v.Name.Remove(0, 14);
                    MessageRuneCreator(index, "rune_haste");
                    Utils.Sleep(15000, "modifier_rune_haste");
                }
                if (mod.Any(x => x.Name == "modifier_rune_regen") && Utils.SleepCheck("modifier_rune_regen")) // RegenRune
                {
                    index = v.Name.Remove(0, 14);
                    MessageRuneCreator(index, "rune_regen");
                    Utils.Sleep(15000, "modifier_rune_regen");
                }
                if (mod.Any(x => x.Name == "modifier_rune_arcane") && Utils.SleepCheck("modifier_rune_arcane")) // ArcaneRune
                {
                    index = v.Name.Remove(0, 14);
                    MessageRuneCreator(index, "rune_arcane");
                    Utils.Sleep(15000, "modifier_rune_arcane");
                }
                if (mod.Any(x => x.Name == "modifier_rune_doubledamage") && Utils.SleepCheck("modifier_doubledamage")) // doubledamageRune
                {
                    index = v.Name.Remove(0, 14);
                    MessageRuneCreator(index, "rune_doubledamage");
                    Utils.Sleep(15000, "modifier_doubledamage");
                }
                //Console.WriteLine(me.Modifiers.LastOrDefault().Name);
                if (mod.Any(x => x.Name == "modifier_item_shadow_amulet_fade") && Utils.SleepCheck("modifier_item_shadow_amulet_fade")) // shadow amulet
                {
                    index = v.Name.Remove(0, 14);
                    MessageItemCreator(index, "shadow_amulet");
                    Utils.Sleep(15000, "modifier_item_shadow_amulet_fade");
                }
                if (mod.Any(x => x.Name == "modifier_invisible") && Utils.SleepCheck("modifier_invisible")) // glimmer cape
                {
                    index = v.Name.Remove(0, 14);
                    MessageItemCreator(index, "glimmer_cape");
                    Utils.Sleep(15000, "modifier_invisible");
                }
                if (mod.Any(x => x.Name == "modifier_treant_natures_guise") && Utils.SleepCheck("modifier_treant_natures_guise")) //treant invis
                {
                    index = v.Name.Remove(0, 14);
                    MessageCreator(index, "treant_natures_guise");
                    Utils.Sleep(15000, "modifier_treant_natures_guise");
                }
                if (mod.Any(x => x.Name == "modifier_item_silver_edge_windwalk") && Utils.SleepCheck("silveredge")) // silver edge
                {
                    index = v.Name.Remove(0, 14);
                    MessageItemCreator(index, "silver_edge");
                    Utils.Sleep(15000, "silveredge");
                }
                if (mod.Any(x => x.Name == "modifier_clinkz_wind_walk") && Utils.SleepCheck("clinkz")) // Clinkz
                {
                    MessageCreator("clinkz", "clinkz_wind_walk");
                    Utils.Sleep(15000, "clinkz");
                }
                if (mod.Any(x => x.Name == "modifier_teleporting") && v.ClassID == ClassID.CDOTA_Unit_Hero_Wisp && Utils.SleepCheck("wisp")) // IO
                {
                    MessageCreator("wisp", "wisp_relocate");
                    Utils.Sleep(15000, "wisp");
                }
                if (mod.Any(x => x.Name == "modifier_bounty_hunter_wind_walk") && Utils.SleepCheck("bounty")) // BOUNTY
                {
                    MessageCreator("bounty_hunter", "bounty_hunter_wind_walk");
                    Utils.Sleep(15000, "bounty");
                }
                if (mod.Any(x => x.Name == "modifier_nyx_assassin_vendetta") && Utils.SleepCheck("nyx")) // NYX ASSASSIN
                {
                    MessageCreator("nyx_assassin", "nyx_assassin_vendetta");
                    Utils.Sleep(15000, "nyx");
                }
                if (mod.Any(x => x.Name == "modifier_teleporting") && v.ClassID == ClassID.CDOTA_Unit_Hero_Furion && Utils.SleepCheck("furion")) // furion
                {
                    MessageCreator("furion", "furion_teleportation");
                    Utils.Sleep(15000, "furion");
                }
                if (v.Spellbook.SpellR.IsInAbilityPhase && v.ClassID == ClassID.CDOTA_Unit_Hero_Furion && Utils.SleepCheck("Nature Ult")) // furion ULT
                {
                    MessageCreator("furion", "nature_wrath_of_nature");
                    Utils.Sleep(2300, "Nature Ult");
                }
            }
            // AA
            // BARA
            // SNIPER
            // BOUNTY ULT
        }
        static void MessageCreator(string saitama, string onepunch)
        {
            informationmessage = new SideMessage("Skills", new Vector2(180, 50));
            informationmessage.AddElement(new Vector2(10, 10), new Vector2(54, 30), Drawing.GetTexture("ensage_ui/heroes_horizontal/" + saitama));
            informationmessage.AddElement(new Vector2(70, 12), new Vector2(62, 31), Drawing.GetTexture("ensage_ui/other/arrow_usual"));
            informationmessage.AddElement(new Vector2(140, 10), new Vector2(30, 30), Drawing.GetTexture("ensage_ui/spellicons/"+onepunch));
            informationmessage.CreateMessage();
        }
        static void MessageRuneCreator(string saitama, string onepunch)
        {
            informationmessage = new SideMessage("Rune", new Vector2(180, 50));
            informationmessage.AddElement(new Vector2(10, 10), new Vector2(54, 30), Drawing.GetTexture("ensage_ui/heroes_horizontal/" + saitama));
            informationmessage.AddElement(new Vector2(70, 12), new Vector2(62, 31), Drawing.GetTexture("ensage_ui/other/arrow_usual"));
            informationmessage.AddElement(new Vector2(140, 10), new Vector2(30, 30), Drawing.GetTexture("ensage_ui/modifier_textures/" + onepunch));
            informationmessage.CreateMessage();
        }
        static void MessageItemCreator(string saitama, string punch)
        {
            informationmessage = new SideMessage("Items", new Vector2(180, 48));
            informationmessage.AddElement(new Vector2(006, 06), new Vector2(72, 36), Drawing.GetTexture("ensage_ui/heroes_horizontal/" + saitama));
            informationmessage.AddElement(new Vector2(078, 12), new Vector2(64, 32), Drawing.GetTexture("ensage_ui/other/arrow_usual"));
            informationmessage.AddElement(new Vector2(142, 06), new Vector2(72, 36), Drawing.GetTexture("ensage_ui/items/" + punch));
            informationmessage.CreateMessage();
        }
        private static void PrintSuccess(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.Green, arguments);
        }
        private static void PrintEncolored(string text, ConsoleColor color, params object[] arguments)
        {
            var clr = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, arguments);
            Console.ForegroundColor = clr;
        }
    }
}
