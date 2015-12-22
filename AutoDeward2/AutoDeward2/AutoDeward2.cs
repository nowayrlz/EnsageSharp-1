using System;
using System.Linq;
using Ensage;
using SharpDX;
using Ensage.Common;
using Ensage.Common.Extensions;
using System.Collections.Generic;

namespace AutoDeward
{
    class Program
    {
        private static Hero me;
        private static string[] items;
        private static void Main()
        {
            Game.OnUpdate += Game_OnUpdate;
            PrintSuccess("> AutoDeward 2 Loaded");
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Game.IsPaused || Game.IsWatchingGame || !Game.IsInGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;

            if (!me.IsAlive)
                return;
            Unit wards = ObjectMgr.GetEntities<Unit>()
                .Where(
                    x =>
                        (x.ClassID == ClassID.CDOTA_NPC_Observer_Ward ||
                         x.ClassID == ClassID.CDOTA_NPC_Observer_Ward_TrueSight || x.ClassID == ClassID.CDOTA_NPC_TechiesMines || x.ClassID == ClassID.CDOTA_NPC_Treant_EyesInTheForest)
                        && x.Team != me.Team && GetDistance2D(x.NetworkPosition, me.NetworkPosition) < 450 &&
                        x.IsVisible && x.IsAlive).FirstOrDefault();
            if (!me.IsChanneling() && Utils.SleepCheck("Use"))
            {
                if (wards != null)
                {
                    uint i = 0;
                    uint last_i = 100;
                    string item_name = null;
                    List<Item> me_inventory = me.Inventory.Items.ToList();
                    items = new string[5] { "item_quelling_blade", "item_iron_talon", "item_bfury", "item_tango_single", "item_tango" };
                    foreach (Item x in me_inventory)
                    {
                        for (i = 0; i < 5; i++)
                        {
                            if (items[i] == x.Name && x.Cooldown <= 0)
                            {
                                if (i <= last_i)
                                {
                                    item_name = x.Name;
                                    last_i = i;
                                }
                            }
                        }
                    }
                    if (item_name != null && me.FindItem(item_name).CanBeCasted())
                        me.FindItem(item_name).UseAbility(wards);
                    Utils.Sleep(250, "Use");
                }
            }
        }
        private static float GetDistance2D(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        public static void PrintInfo(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.White, arguments);
        }

        public static void PrintSuccess(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.Green, arguments);
        }

        public static void PrintError(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.Red, arguments);
        }

        public static void PrintEncolored(string text, ConsoleColor color, params object[] arguments)
        {
            var clr = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, arguments);
            Console.ForegroundColor = clr;
        }
    }
}
