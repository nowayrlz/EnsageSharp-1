using System;
using System.Collections.Generic;
using System.Linq;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using Ensage.Common.Menu;

namespace AutoItems
{
    class AutoItems_NINJA
    {
        private static Hero me;
        private static Item item_bottle, item_phase_boots, item_magic_stick, item_magic_wand;
        private static double PercentStickUse = 0.2;
        private static readonly uint WM_MOUSEWHEEL = 0x020A;
        private static readonly Menu Menu = new Menu("Auto Items", "Auto Items", true);
        private static readonly Menu _item_config = new Menu("Items", "Items");
        private static readonly Menu _percent_config = new Menu("Stick Percent Config", "Stick Percent Config");
        private static readonly Dictionary<string, bool> list_of_items = new Dictionary<string, bool>
            {
                {"item_bottle",true},
                {"item_phase_boots",true},
                {"item_magic_stick",true},
                {"item_magic_wand",true}
            };
        static void Main(string[] args)
        {
            Menu.AddSubMenu(_item_config);
            Menu.AddSubMenu(_percent_config);
            _item_config.AddItem(new MenuItem("Items: ", "Items: ").SetValue(new AbilityToggler(list_of_items)));
            _percent_config.AddItem(new MenuItem("Percent Configuration", "Percent Configuration").SetValue(new Slider(20, 10, 100)));
            Menu.AddToMainMenu();
            Game.OnUpdate += Tick;
            PrintSuccess(string.Format("> Auto Items Loaded!"));
        }
        public static void Tick(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame || Game.IsChatOpen)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;

            item_bottle = me.FindItem("item_bottle");
            item_phase_boots = me.FindItem("item_phase_boots");
            item_magic_stick = me.FindItem("item_magic_stick");
            item_magic_wand = me.FindItem("item_magic_wand");
            PercentStickUse =((double)Menu.Item("Percent Configuration").GetValue<Slider>().Value / 100);
            if (me.IsAlive && (!me.IsUnitState(UnitState.Invisible) || me.ClassID == ClassID.CDOTA_Unit_Hero_Riki)  && !me.IsChanneling() && me.CanUseItems() && Utils.SleepCheck("AutoItems"))
            {
                if (item_bottle != null && me.Modifiers.Any(x => x.Name == "modifier_fountain_aura_buff") && !me.Modifiers.Any(x => x.Name == "modifier_bottle_regeneration") && (me.Health < me.MaximumHealth || me.Mana < me.MaximumMana))
                    item_bottle.UseAbility();
                if (item_phase_boots != null && item_phase_boots.CanBeCasted() && me.NetworkActivity == NetworkActivity.Move)
                    item_phase_boots.UseAbility();
                if (item_magic_stick != null && item_magic_stick.CanBeCasted() && item_magic_stick.CurrentCharges > 0 && (double)me.Health / me.MaximumHealth < PercentStickUse)
                    item_magic_stick.UseAbility();
                if (item_magic_wand != null && item_magic_wand.CanBeCasted() && item_magic_wand.CurrentCharges > 0 && (double) me.Health / me.MaximumHealth < PercentStickUse)
                    item_magic_wand.UseAbility();
                Utils.Sleep(250, "AutoItems");
            }
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
