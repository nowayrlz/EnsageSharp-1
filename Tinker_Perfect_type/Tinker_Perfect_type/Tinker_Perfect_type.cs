using System;
using System.Linq;
using System.Collections.Generic;
using Ensage;
using Ensage.Common.Extensions;
using Ensage.Common;
using Ensage.Common.Menu;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;
using SharpDX;

namespace Tinker_Perfect_type
{
    class Tinker_Perfect_type
    {
        private static Ability Laser, Rocket, Refresh, March;
        private static Item Blink, Dagon, Hex, Soulring, Ethereal, Shiva, ghost, euls, forcestaff;
        private static Hero me, target;
        private static readonly Menu Menu = new Menu("Tinker Perfect", "Tinker Perfect", true);
        private static readonly Menu _skills = new Menu("Skills", "Skills");
        private static readonly Menu _items = new Menu("Items", "Items");
        private static readonly Dictionary<string, bool> Skills = new Dictionary<string, bool>
            {
                {"tinker_laser",true},
                {"tinker_heat_seeking_missile",true},
                {"tinker_rearm",true},
                {"tinker_march_of_the_machines",true}
            };
        private static readonly Dictionary<string, bool> Items = new Dictionary<string, bool>
            {
                {"item_blink",true},
                {"item_dagon",true},
                {"item_sheepstick",true},
                {"item_soul_ring",true},
                {"item_ethereal_blade",true},
                {"item_shivas_guard",true},
                {"item_ghost",true},
                {"item_cyclone",true},
                {"item_force_staff",true}
            };

        static void Main(string[] args)
        {
            Menu.AddItem(new MenuItem("Combo Key", "Combo Key").SetValue(new KeyBind('D', KeyBindType.Press)));
            Menu.AddSubMenu(_skills);
            Menu.AddSubMenu(_items);
            _skills.AddItem(new MenuItem("Skills: ", "Skills: ").SetValue(new AbilityToggler(Skills)));
            _items.AddItem(new MenuItem("Items: ", "Items: ").SetValue(new AbilityToggler(Items)));
            Menu.AddToMainMenu();
            PrintSuccess(string.Format("> Tinker Perfect Type Loaded!"));
            Game.OnUpdate += Tinker_In_Madness;
            Drawing.OnDraw += markedfordeath;
        }
        public static void Tinker_In_Madness(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsWatchingGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;
            if (me.ClassID != ClassID.CDOTA_Unit_Hero_Tinker)
                return;
            if ((Game.IsKeyDown(Menu.Item("Combo Key").GetValue<KeyBind>().Key)) && !Game.IsChatOpen)
            {
                target = me.ClosestToMouseTarget(1000);
                if (target != null && target.IsAlive && !target.IsIllusion && !me.IsChanneling())
                {
                    FindItems();
                    if (target.IsLinkensProtected())
                    {
                        if (euls != null && euls.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(euls.Name))
                        {
                            if (Utils.SleepCheck("TimingToLinkens"))
                            {
                                euls.UseAbility(target);
                                Utils.Sleep(200, "TimingToLinkens");
                            }
                        }
                        else if (forcestaff != null && forcestaff.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(forcestaff.Name))
                        {
                            if (Utils.SleepCheck("TimingToLinkens"))
                            {
                                forcestaff.UseAbility(target);
                                Utils.Sleep(200, "TimingToLinkens");
                            }
                        }
                        else if (Ethereal != null && Ethereal.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Ethereal.Name))
                        {
                            if (Utils.SleepCheck("TimingToLinkens"))
                            {
                                Ethereal.UseAbility(target);
                                Utils.Sleep(200, "TimingToLinkens");
                                Utils.Sleep((me.NetworkPosition.Distance2D(target.NetworkPosition) / 650) * 1000, "TimingToLinkens");
                            }
                        }
                        else if (Laser != null && Laser.CanBeCasted() && Menu.Item("Skills: ").GetValue<AbilityToggler>().IsEnabled(Laser.Name))
                        {
                            if (Utils.SleepCheck("TimingToLinkens"))
                            {
                                Laser.UseAbility(target);
                                Utils.Sleep(200, "TimingToLinkens");
                            }
                        }
                        else if (Dagon != null && Dagon.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled("item_dagon"))
                        {
                            if (Utils.SleepCheck("TimingToLinkens"))
                            {
                                Dagon.UseAbility(target);
                                Utils.Sleep(200, "TimingToLinkens");
                            }
                        }
                        else if (Hex != null && Hex.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Hex.Name))
                        {
                            if (Utils.SleepCheck("TimingToLinkens"))
                            {
                                Hex.UseAbility(target);
                                Utils.Sleep(200, "TimingToLinkens");
                            }
                        }
                    }
                    else
                    {
                        uint elsecount = 0;
                        bool magicimune = (!target.IsMagicImmune() && !target.Modifiers.Any(x => x.Name == "modifier_eul_cyclone"));
                        if (Utils.SleepCheck("combo"))
                        {
                            // ghost -> soulring -> hex -> laser -> ethereal -> dagon -> rocket -> shivas -> euls -> refresh
                            if (ghost != null && Ethereal == null && ghost.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(ghost.Name) && Utils.SleepCheck("Rearm"))
                                ghost.UseAbility();
                            else
                                elsecount += 1;
                            if (Soulring != null && Soulring.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Soulring.Name) && Utils.SleepCheck("Rearm"))
                                Soulring.UseAbility();
                            else
                                elsecount += 1;
                            if (Hex != null && Hex.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Hex.Name) && magicimune && Utils.SleepCheck("Rearm"))
                                Hex.UseAbility(target);
                            else
                                elsecount += 1;
                            if (Laser != null && Laser.CanBeCasted() && Menu.Item("Skills: ").GetValue<AbilityToggler>().IsEnabled(Laser.Name) && magicimune && Utils.SleepCheck("Rearm"))
                                Laser.UseAbility(target);
                            else
                                elsecount += 1;
                            if (Ethereal != null && Ethereal.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Ethereal.Name) && magicimune && Utils.SleepCheck("Rearm") && Utils.SleepCheck("EtherealTime"))
                            {
                                Ethereal.UseAbility(target);
                                Utils.Sleep((me.NetworkPosition.Distance2D(target.NetworkPosition) / 650) * 1000, "EtherealTime");
                            }
                            else
                                elsecount += 1;
                            if (Dagon != null && Dagon.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled("item_dagon") && magicimune && Utils.SleepCheck("Rearm") && Utils.SleepCheck("EtherealTime"))
                                Dagon.UseAbility(target);
                            else
                                elsecount += 1;
                            if (Rocket != null && Rocket.CanBeCasted() && Menu.Item("Skills: ").GetValue<AbilityToggler>().IsEnabled(Rocket.Name) && magicimune && Utils.SleepCheck("Rearm") && Utils.SleepCheck("RocketTime"))
                            {
                                Rocket.UseAbility();
                                Utils.Sleep((me.NetworkPosition.Distance2D(target.NetworkPosition) / 600) * 1000, "RocketTime");
                            }
                            else
                                elsecount += 1;
                            if (Shiva != null && Shiva.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Shiva.Name) && magicimune && Utils.SleepCheck("Rearm"))
                                Shiva.UseAbility();
                            else
                                elsecount += 1;
                            if (elsecount == 8 && euls != null && euls.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(euls.Name) && magicimune && Utils.SleepCheck("Rearm") && Utils.SleepCheck("EtherealTime") && Utils.SleepCheck("RocketTime"))
                                euls.UseAbility(target);
                            else
                                elsecount += 1;
                            if (elsecount == 9 && Refresh != null && Refresh.CanBeCasted() && Menu.Item("Skills: ").GetValue<AbilityToggler>().IsEnabled(Refresh.Name) && !Refresh.IsChanneling && Utils.SleepCheck("Rearm") && Ready_for_refresh())
                            {
                                Refresh.UseAbility();
                                Utils.Sleep(900, "Rearm");
                            }
                            else
                            {
                                if (!me.IsChanneling() && Utils.SleepCheck("Rearm") && me.Distance2D(target) <= me.AttackRange)
                                    me.Attack(target);
                            }
                            Utils.Sleep(150, "combo");
                        }
                    }
                }
                else
                {
                    if (!me.IsChanneling() && Utils.SleepCheck("Rearm"))
                        me.Move(Game.MousePosition);
                }
            }
        }
        static void markedfordeath(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsWatchingGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;
            if (me.ClassID != ClassID.CDOTA_Unit_Hero_Tinker)
                return;
            target = me.ClosestToMouseTarget(50000);
            if (target != null && !target.IsIllusion && target.IsAlive)
            {
                Vector2 target_health_bar = HeroPositionOnScreen(target);
                Drawing.DrawText("Marked for Death", target_health_bar, new Vector2(18, 200), me.Distance2D(target) < 1200 ? Color.Red : Color.Azure, FontFlags.AntiAlias | FontFlags.Additive | FontFlags.DropShadow);
            }

        }
        static void FindItems()
        {
            //Skils
            Laser = me.Spellbook.SpellQ;
            Rocket = me.Spellbook.SpellW;
            Refresh = me.Spellbook.SpellR;
            March = me.Spellbook.SpellE;
            //Items
            Blink = me.FindItem("item_blink");
            Dagon = me.Inventory.Items.FirstOrDefault(item => item.Name.Contains("item_dagon"));
            Hex = me.FindItem("item_sheepstick");
            Soulring = me.FindItem("item_soul_ring");
            Ethereal = me.FindItem("item_ethereal_blade");
            Shiva = me.FindItem("item_shivas_guard");
            ghost = me.FindItem("item_ghost");
            euls = me.FindItem("item_cyclone");
            forcestaff = me.FindItem("item_force_staff");
        }
        static Vector2 HeroPositionOnScreen(Hero x)
        {
            float scaleX = HUDInfo.ScreenSizeX();
            float scaleY = HUDInfo.ScreenSizeY();
            Vector2 PicPosition;
            Drawing.WorldToScreen(x.Position, out PicPosition);
            PicPosition = new Vector2((float)(PicPosition.X + (scaleX * -0.035)),(float)((PicPosition.Y) + (scaleY * -0.10)));
            return PicPosition;
        }
        static bool Ready_for_refresh()
        {
            if ((ghost != null && ghost.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(ghost.Name)) 
                || (Soulring != null && Soulring.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Soulring.Name)) 
                || (Hex != null && Hex.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Hex.Name)) 
                || (Laser != null && Laser.CanBeCasted() && Menu.Item("Skills: ").GetValue<AbilityToggler>().IsEnabled(Laser.Name)) 
                || (Ethereal != null && Ethereal.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Ethereal.Name)) 
                || (Dagon != null && Dagon.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled("item_dagon")) 
                || (Rocket != null && Rocket.CanBeCasted() && Menu.Item("Skills: ").GetValue<AbilityToggler>().IsEnabled(Rocket.Name)) 
                || (Shiva != null && Shiva.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Shiva.Name)) 
                || (euls != null && euls.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(euls.Name)))
                return false;
            else
                return true;
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
        static bool IsLinkensProtected(Hero x)
        {
            if (x.Modifiers.Any(m => m.Name == "modifier_item_sphere_target") || x.FindItem("item_sphere") != null && x.FindItem("item_sphere").Cooldown <= 0)
                return true;
            else
                return false;
        }
    }
}
