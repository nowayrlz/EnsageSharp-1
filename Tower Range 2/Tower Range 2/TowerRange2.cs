using System;
using System.Linq;
using Ensage;
using SharpDX;
using Ensage.Common.Menu;
using System.Collections.Generic;

namespace Tower_Range_2
{
    class TowerRange2
    {
        private static readonly Menu Menu = new Menu("Tower range 2", "Tower Range 2", true);
        private static Hero me;
        private static ParticleEffect[] EFFECT1 = new ParticleEffect[30], EFFECT2 = new ParticleEffect[30];


        static void Main(string[] args)
        {
            Menu.AddItem(new MenuItem("OwnTowers", "My Towers").SetValue(false).SetTooltip("Show your tower range."));
            Menu.AddItem(new MenuItem("EnemyTowers", "Enemies Towers").SetValue(false).SetTooltip("Show the enemies towers range."));
            Menu.AddToMainMenu();
            Game.OnUpdate += FindTower;
            PrintSuccess(string.Format("> Tower Range 2 Loaded!"));
        }

        public static void FindTower(EventArgs args)
        {
            if (!Game.IsInGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;
            List<Building> building =
                ObjectMgr.GetEntities<Building>()
                    .Where(x => x.IsAlive && (x.ClassID == ClassID.CDOTA_BaseNPC_Tower || x.ClassID == ClassID.CDOTA_BaseNPC_Fort))
                    .ToList();
            List<Entity> Fountain =
                ObjectMgr.GetEntities<Entity>()
                    .Where(x => x.IsAlive && x.ClassID == ClassID.CDOTA_Unit_Fountain)
                    .ToList();
            if (!building.Any() && !Fountain.Any())
                return;
            uint i = 0;
            if (Menu.Item("EnemyTowers").GetValue<bool>())
            {
                foreach (Building build in building.Where(x => x.Team != me.Team).ToList())
                {
                    if (build != null)
                    {
                        if (build.ClassID == ClassID.CDOTA_BaseNPC_Tower)
                        {
                            i++;
                            if (EFFECT1[i] == null)
                                EFFECT1[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                            if (EFFECT1[i].GetHighestControlPoint() != 1)
                            {
                                EFFECT1[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                                EFFECT1[i].SetControlPoint(1, new Vector3(850, 0, 0));
                            }
                            if (EFFECT2[i] == null)
                                EFFECT2[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                            if (EFFECT2[i].GetHighestControlPoint() != 1)
                            {
                                EFFECT2[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                                EFFECT2[i].SetControlPoint(1, new Vector3(900, 0, 0));
                            }
                        }
                        if (build.ClassID == ClassID.CDOTA_BaseNPC_Fort)
                        {
                            i++;
                            if (EFFECT1[i] == null)
                                EFFECT1[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                            if (EFFECT1[i].GetHighestControlPoint() != 1)
                            {
                                EFFECT1[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                                EFFECT1[i].SetControlPoint(1, new Vector3(900, 0, 0));
                            }
                        }
                    }
                }
                foreach (Entity f in Fountain.Where(x => x.Team != me.Team).ToList())
                {
                    if (f != null)
                    {
                        i++;
                        if (EFFECT1[i] == null)
                            EFFECT1[i] = f.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                        if (EFFECT1[i].GetHighestControlPoint() != 1)
                        {
                            EFFECT1[i] = f.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                            EFFECT1[i].SetControlPoint(1, new Vector3(1200, 0, 0));
                        }
                        if (EFFECT2[i] == null)
                            EFFECT2[i] = f.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                        if (EFFECT2[i].GetHighestControlPoint() != 1)
                        {
                            EFFECT2[i] = f.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                            EFFECT2[i].SetControlPoint(1, new Vector3(1350, 0, 0));
                        }
                    }
                }
            }
            if (Menu.Item("OwnTowers").GetValue<bool>())
            {
                foreach (Building build in building.Where(x => x.Team == me.Team).ToList())
                {
                    if (build != null)
                    {
                        if (build.ClassID == ClassID.CDOTA_BaseNPC_Tower)
                        {
                            i++;
                            if (EFFECT1[i] == null)
                                EFFECT1[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                            if (EFFECT1[i].GetHighestControlPoint() != 1)
                            {
                                EFFECT1[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                                EFFECT1[i].SetControlPoint(1, new Vector3(850, 0, 0));
                            }
                            if (EFFECT2[i] == null)
                                EFFECT2[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                            if (EFFECT2[i].GetHighestControlPoint() != 1)
                            {
                                EFFECT2[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                                EFFECT2[i].SetControlPoint(1, new Vector3(900, 0, 0));
                            }
                        }
                        if (build.ClassID == ClassID.CDOTA_BaseNPC_Fort)
                        {
                            i++;
                            if (EFFECT1[i] == null)
                                EFFECT1[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                            if (EFFECT1[i].GetHighestControlPoint() != 1)
                            {
                                EFFECT1[i] = build.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                                EFFECT1[i].SetControlPoint(1, new Vector3(900, 0, 0));
                            }
                        }
                    }
                }
                foreach (Entity f in Fountain.Where(x => x.Team == me.Team).ToList())
                {
                    if (f != null)
                    {
                        i++;
                        if (EFFECT1[i] == null)
                            EFFECT1[i] = f.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                        if (EFFECT1[i].GetHighestControlPoint() != 1)
                        {
                            EFFECT1[i] = f.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                            EFFECT1[i].SetControlPoint(1, new Vector3(1200, 0, 0));
                        }
                        if (EFFECT2[i] == null)
                            EFFECT2[i] = f.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                        if (EFFECT2[i].GetHighestControlPoint() != 1)
                        {
                            EFFECT2[i] = f.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
                            EFFECT2[i].SetControlPoint(1, new Vector3(1350, 0, 0));
                        }
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
