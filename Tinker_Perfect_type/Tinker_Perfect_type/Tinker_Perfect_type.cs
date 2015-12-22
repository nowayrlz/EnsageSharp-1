using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

using Ensage;
using SharpDX;
using Ensage.Common.Extensions;
using Ensage.Common;
using SharpDX.Direct3D9;
using Ensage.Common.Menu;

namespace Tinker_Perfect_type
{
    class Tinker_Perfect_type
    {
        private static Ability Laser, Rocket, Refresh, March;
        private static Item Blink, Dagon, Hex, Soulring, Ethereal, Shiva;
        private static Hero me, target;
        private static readonly Menu Menu = new Menu("Tinker Perfect", "Tinker Perfect", true);

        static void Main(string[] args)
        {
            Menu.AddItem(new MenuItem("Hotkey", "Hotkey").SetValue(new KeyBind('D', KeyBindType.Press)));
            //Menu.AddItem(new MenuItem("Farm Hotkey", "Farm Hotkey").SetValue(new KeyBind('T', KeyBindType.Press)));
            Menu.AddToMainMenu();
            PrintSuccess(string.Format("> Necrophos Script Loaded!"));
            Game.OnUpdate += Tinker_In_Madness;
        }
        public static void Tinker_In_Madness(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsWatchingGame)
                return;
            me = ObjectMgr.LocalHero;
            target = me.ClosestToMouseTarget(1000);
            if (me == null)
                return;

            // ABILITIES

            Laser = me.Spellbook.SpellQ;
            Rocket = me.Spellbook.SpellW;
            Refresh = me.Spellbook.SpellR;
            March = me.Spellbook.SpellE;

            // ITEMS
            Blink = me.FindItem("item_blink");
            Dagon = me.Inventory.Items.FirstOrDefault(item => item.Name.Contains("item_dagon"));
            Hex = me.FindItem("item_sheepstick");
            Soulring = me.FindItem("item_soul_ring");
            Ethereal = me.FindItem("item_ethereal_blade");
            Shiva = me.FindItem("item_shivas_guard");

            if ((Game.IsKeyDown(Menu.Item("Hotkey").GetValue<KeyBind>().Key)) && !Game.IsChatOpen)
            {
                if (target != null && target.IsAlive && !me.IsChanneling() && me.CanCast() && !target.IsIllusion && !target.IsMagicImmune())
                {
                    int stage = 0;
                    switch (stage)
                    {
                        case 0:
                            if (stage == 0)
                            {
                                if (Soulring != null && Soulring.CanBeCasted() && me.Health > 300 && Utils.SleepCheck("SoulRingCast"))
                                {
                                    Soulring.UseAbility();
                                    Utils.Sleep(Soulring.GetCastDelay(me, target, true, true, Soulring.Name, false) * 1000, "SoulRingCast");
                                }
                                else if (!Soulring.CanBeCasted() || me.Health < 300)
                                    stage = 1;
                            }
                            goto case 1;
                        case 1:
                            if (stage == 1)
                            {
                                if (Shiva != null && Shiva.CanBeCasted() && Utils.SleepCheck("ShivasCast") && me.NetworkPosition.Distance2D(target.NetworkPosition) < 900)
                                {
                                    Shiva.UseAbility();
                                    Utils.Sleep(Shiva.GetCastDelay(me, target, true, true, Shiva.Name, false) * 1000, "ShivasCast");
                                }
                                else if (!Shiva.CanBeCasted() || me.NetworkPosition.Distance2D(target.NetworkPosition) > 900)
                                    stage = 2;
                            }
                            goto case 2;
                        case 2:
                            if (stage == 2)
                            {
                                if (Hex != null && Hex.CanBeCasted() && Utils.SleepCheck("HexCast") && me.NetworkPosition.Distance2D(target.NetworkPosition) <= Hex.CastRange)
                                {
                                    Hex.UseAbility(target);
                                    Utils.Sleep(Hex.GetCastDelay(me, target, true, true, Hex.Name, false) * 1000, "HexCast");
                                }
                                else if (!Hex.CanBeCasted() || me.NetworkPosition.Distance2D(target.NetworkPosition) >= Hex.CastRange)
                                    stage = 3;
                            }
                            goto case 3;
                        case 3:
                            if (stage == 3)
                            {
                                if (Laser != null && Laser.CanBeCasted() && Utils.SleepCheck("LaserCast") && me.NetworkPosition.Distance2D(target.NetworkPosition) <= Laser.CastRange)
                                {
                                    Laser.UseAbility(target);
                                    Utils.Sleep(Laser.GetCastDelay(me, target, true, true, Laser.Name, false) * 1000, "LaserCast");
                                }
                                else if (!Laser.CanBeCasted() || me.NetworkPosition.Distance2D(target.NetworkPosition) >= Laser.CastRange)
                                    stage = 4;
                            }
                            goto case 4;
                        case 4:
                            if (stage == 4)
                            {
                                if (Ethereal != null && Ethereal.CanBeCasted() && Utils.SleepCheck("EtherealCast") && me.NetworkPosition.Distance2D(target.NetworkPosition) <= Ethereal.CastRange)
                                {
                                    Ethereal.UseAbility(target);
                                    Utils.Sleep(Ethereal.GetCastDelay(me, target, true, true, Ethereal.Name, false) * 1000, "EtherealCast");
                                }
                                else if (!Ethereal.CanBeCasted() || me.NetworkPosition.Distance2D(target.NetworkPosition) >= Ethereal.CastRange)
                                    stage = 5;
                            }
                            goto case 5;
                        case 5:
                            if (stage == 5)
                            {
                                if (Dagon != null && Dagon.CanBeCasted() && Utils.SleepCheck("DagonCast") && me.NetworkPosition.Distance2D(target.NetworkPosition) <= Dagon.CastRange)
                                {
                                    Dagon.UseAbility(target);
                                    Utils.Sleep(Dagon.GetCastDelay(me, target, true, true, Dagon.Name, false) * 1000, "DagonCast");
                                }
                                else if (!Dagon.CanBeCasted() || me.NetworkPosition.Distance2D(target.NetworkPosition) >= Dagon.CastRange)
                                    stage = 6;
                            }
                            goto case 6;
                        case 6:
                            if (stage == 6)
                            {
                                if (Rocket != null && Rocket.CanBeCasted() && Utils.SleepCheck("RocketCast") && me.NetworkPosition.Distance2D(target.NetworkPosition) < 2500)
                                {
                                    Rocket.UseAbility();
                                    Utils.Sleep(Rocket.GetCastDelay(me, target, true, true, Rocket.Name, false) * 1000, "RocketCast");
                                }
                                else if (!Rocket.CanBeCasted())
                                    stage = 7;
                            }
                            goto case 7;
                        case 7:
                            if (stage == 7 && Utils.SleepCheck("END"))
                            {
                                if (Refresh != null && Refresh.CanBeCasted() && (me.Mana >= Refresh.ManaCost) && !Refresh.IsChanneling && Utils.SleepCheck("RefreshCast") && ReadyForRefresh())
                                {
                                    Refresh.UseAbility();
                                    Utils.Sleep(Refresh.GetCastDelay(me, target, true, true, Refresh.Name, false) * 1500, "RefreshCast");
                                }
                                else if (!Refresh.CanBeCasted() || Refresh.IsChanneling)
                                    stage = 0;
                                Utils.Sleep(250, "END");
                            }
                            break;
                    }

                }
                if (me.Mana <= 200 && !me.IsChanneling() && target != null && target.IsAlive && !target.IsIllusion && Utils.SleepCheck("attack"))
                {
                    me.Attack(target);
                    Utils.Sleep(500 - Game.Ping, "attack");
                }
            }
            //if ((Game.IsKeyDown(Menu.Item("Farm Hotkey").GetValue<KeyBind>().Key)) && !Game.IsChatOpen)
            //{

            //}
        }
        private static bool ReadyForRefresh()
        {
            if (!Laser.CanBeCasted() &&
                !Rocket.CanBeCasted() &&
                !Ethereal.CanBeCasted() &&
                !Dagon.CanBeCasted() &&
                !Hex.CanBeCasted() &&
                !Shiva.CanBeCasted())
                return true;
            else
                return false;
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
