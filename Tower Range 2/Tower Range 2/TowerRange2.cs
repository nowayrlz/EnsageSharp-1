using System;
using System.Linq;
using Ensage;
using SharpDX;
using Ensage.Common.Menu;
//using Ensage.Common;
using System.Collections.Generic;

namespace Tower_Range_2
{
    class TowerRange2
    {
        private static readonly Menu Menu = new Menu("Tower range 2", "Tower Range 2", true);
        private static Hero me;
        private static Dictionary<string, ParticleEffect> rangeeffects = new Dictionary<string, ParticleEffect> { };
        private static int range = 0, range2 = 0, radiusbonus = 0;
        static void Main(string[] args)
        {
            Menu.AddItem(new MenuItem("OwnTowers", "My Towers").SetValue(false).SetTooltip("Show your tower range."));
            Menu.AddItem(new MenuItem("EnemyTowers", "Enemies Towers").SetValue(false).SetTooltip("Show the enemies towers range."));
            Menu.AddToMainMenu();
            Game.OnUpdate += FindTower;
            Game.OnFireEvent += TowerDestroyed;
            PrintSuccess(string.Format("> Tower Range 2 Loaded!"));
        }
        private static void TowerDestroyed(FireEventEventArgs args)
        {
            if (args.GameEvent.Name == "dota_tower_kill")
            {
                foreach(var x in rangeeffects.Values.ToList())
                {
                    x.Dispose();
                }
                rangeeffects.Clear();
            }
        }
        public static void FindTower(EventArgs args)
        {
            if (!Game.IsInGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
            {
                radiusbonus = 0;
                return;
            }
            if(radiusbonus == 0)
                radiusbonus = (int)me.HullRadius;
            List<Entity> building =
                ObjectMgr.GetEntities<Entity>()
                    .Where(x => x.IsAlive && (x.ClassID == ClassID.CDOTA_BaseNPC_Tower || x.ClassID == ClassID.CDOTA_BaseNPC_Fort || x.ClassID == ClassID.CDOTA_Unit_Fountain))
                    .ToList();
            if (!building.Any() && building == null)
                return;
            if (Menu.Item("EnemyTowers").GetValue<bool>())
            {
                foreach(var x in building)
                {
                    if (x.Team == me.Team) continue;
                    if(x.ClassID == ClassID.CDOTA_BaseNPC_Tower)
                    {
                        range = 900 + radiusbonus +50;
                        range2 = 850 + radiusbonus + 50;
                    }
                    if(x.ClassID == ClassID.CDOTA_BaseNPC_Fort)
                    {
                        range = 900 + radiusbonus + 50;
                        range2 = 0;
                    }
                    if(x.ClassID == ClassID.CDOTA_Unit_Fountain)
                    {
                        range = 1200 + radiusbonus + 50;
                        range2 = 1350 + radiusbonus + 50;
                    }
                    if (!rangeeffects.Keys.Any(y => y == "" + x.Handle + ""))
                    {
                        rangeeffects.Add(""+x.Handle+"", new ParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf", x));
                    }
                    if(rangeeffects.Keys.Any(y => y == "" + x.Handle + ""))
                    {
                        rangeeffects.FirstOrDefault(y => y.Key == "" + x.Handle + "").Value.SetControlPoint(1, new Vector3(30, 144, 255));
                        rangeeffects.FirstOrDefault(y => y.Key == "" + x.Handle + "").Value.SetControlPoint(2, new Vector3(range, 255, 0));
                    }
                    if (!rangeeffects.Keys.Any(y => y == "" + x.Handle + "2"))
                    {
                        rangeeffects.Add("" + x.Handle + "2", new ParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf", x));
                    }
                    if (rangeeffects.Keys.Any(y => y == "" + x.Handle + "2") && range2 > 0)
                    {
                        rangeeffects.FirstOrDefault(y => y.Key == "" + x.Handle + "2").Value.SetControlPoint(1, new Vector3(178, 34, 34));
                        rangeeffects.FirstOrDefault(y => y.Key == "" + x.Handle + "2").Value.SetControlPoint(2, new Vector3(range2, 255, 0));
                    }
                }
            }
            if (Menu.Item("OwnTowers").GetValue<bool>())
            {
                foreach (var x in building)
                {
                    if (x.Team != me.Team) continue;
                    if (x.ClassID == ClassID.CDOTA_BaseNPC_Tower)
                    {
                        range = 900 + radiusbonus + 50;
                        range2 = 850 + radiusbonus + 50;
                    }
                    if (x.ClassID == ClassID.CDOTA_BaseNPC_Fort)
                    {
                        range = 900 + radiusbonus + 50;
                        range2 = 0;
                    }
                    if (x.ClassID == ClassID.CDOTA_Unit_Fountain)
                    {
                        range = 1200 + radiusbonus + 50;
                        range2 = 1350 + radiusbonus + 50;
                    }
                    if (!rangeeffects.Keys.Any(y => y == "" + x.Handle + ""))
                    {
                        rangeeffects.Add("" + x.Handle + "", new ParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf", x));
                    }
                    if (rangeeffects.Keys.Any(y => y == "" + x.Handle + ""))
                    {
                        rangeeffects.FirstOrDefault(y => y.Key == "" + x.Handle + "").Value.SetControlPoint(1, new Vector3(30, 144, 255));
                        rangeeffects.FirstOrDefault(y => y.Key == "" + x.Handle + "").Value.SetControlPoint(2, new Vector3(range, 255, 0));
                    }
                    if (!rangeeffects.Keys.Any(y => y == "" + x.Handle + "2"))
                    {
                        rangeeffects.Add("" + x.Handle + "2", new ParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf", x));
                    }
                    if (rangeeffects.Keys.Any(y => y == "" + x.Handle + "2") && range2 > 0)
                    {
                        rangeeffects.FirstOrDefault(y => y.Key == "" + x.Handle + "2").Value.SetControlPoint(1, new Vector3(178, 34, 34));
                        rangeeffects.FirstOrDefault(y => y.Key == "" + x.Handle + "2").Value.SetControlPoint(2, new Vector3(range2, 255, 0));
                    }
                }
            }
        }
        private static void PrintSuccess(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.Green, arguments);
        }
        private static void PrintEncolored(string text, ConsoleColor color, params object[] arguments)
        {
            ConsoleColor clr = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, arguments);
            Console.ForegroundColor = clr;
        }
    }
}
