using System;
using System.Linq;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;

namespace AutoDeward
{
    class Program
    {
        private static Hero me;
        private static readonly string[] items = { "item_quelling_blade", "item_iron_talon", "item_bfury", "item_tango_single", "item_tango" };
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
            Unit Things = ObjectMgr.GetEntities<Unit>()
                .FirstOrDefault(
                    x =>
                        (x.ClassID == ClassID.CDOTA_NPC_Observer_Ward ||
                         x.ClassID == ClassID.CDOTA_NPC_Observer_Ward_TrueSight || x.ClassID == ClassID.CDOTA_NPC_TechiesMines /*|| x.ClassID == ClassID.CDOTA_NPC_Treant_EyesInTheForest*/)
                        && x.Team != me.Team && me.NetworkPosition.Distance2D(x.NetworkPosition) < 450 &&
                        x.IsVisible && x.IsAlive);
            if (!me.IsChanneling() && Utils.SleepCheck("Use"))
            {
                if (Things != null)
                {
                    var useItem = items.Select(item => me.FindItem(item)).FirstOrDefault(x => x != null && x.CanBeCasted());
                    if (useItem != null && !((useItem.Name == "item_tango_single" || useItem.Name == "item_tango") && Things.ClassID == ClassID.CDOTA_NPC_TechiesMines))
                    {
                        useItem.UseAbility(Things);
                        Utils.Sleep(250, "Use");
                    }
                    Utils.Sleep(250, "Use");
                }
            }
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
