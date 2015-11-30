using System;
using System.Linq;
using Ensage;
using Ensage.Common.Extensions;
using Ensage.Common;
using SharpDX.Direct3D9;
using SharpDX;
using Ensage.Common.Menu;
using System.Collections.Generic;

namespace Necrophos
{
    class Necrophos
    {
        private static Ability Qskill, Rskill;
        private static Item Blink, shadow, silveredge, dagon, ethereal, veil,  euls, forcestaff, shivas, malevo;
        private static Hero me, target;

        private static Font _HP;
        private static Font _description;
        private static Font _comboing;

        private static double[] UltDmg = new double[3] { 0.4, 0.6, 0.9 };
        private static double[] AUltDmg = new double[3] { 0.6, 0.9, 1.2 };

        private static int[] DDamage = new int[5] { 400, 500, 600, 700, 800 };
        private static readonly Menu Menu = new Menu("Necrophos", "Necrophos", true);
        private static readonly Menu _item_config = new Menu("Items", "Items");
        private static readonly Menu _abillity_config = new Menu("Abilities", "Abilities");

        private static readonly Dictionary<string, bool> listofuse_skills1 = new Dictionary<string, bool>
            {
                {"item_blink",true},
                {"item_dagon",true},
                {"item_cyclone",true},
                {"item_ethereal_blade",true},
                {"item_shivas_guard",true},
                {"item_orchid",true},
                {"item_force_staff",true},
                {"item_veil_of_discord",true}
            };
        private static readonly Dictionary<string, bool> listofuse_abylities = new Dictionary<string, bool>
            {
                {"necrolyte_death_pulse",true},
                {"necrolyte_reapers_scythe",true}
            };

        static void Main(string[] args)
        {
            Menu.AddItem(new MenuItem("2001", "Key to Combo").SetValue(new KeyBind('D', KeyBindType.Press)));
            Menu.AddItem(new MenuItem("2002", "Key to Force Combo").SetValue(new KeyBind('E', KeyBindType.Press)));
            Menu.AddSubMenu(_item_config);
            Menu.AddSubMenu(_abillity_config);
            _item_config.AddItem(new MenuItem("Items: ", "Items: ").SetValue(new AbilityToggler(listofuse_skills1)));
            _abillity_config.AddItem(new MenuItem("Abilities: ", "Abilities: ").SetValue(new AbilityToggler(listofuse_abylities)));
            Menu.AddToMainMenu();
            Game.OnUpdate += Tick;
            Drawing.OnPreReset += Drawing_OnPreReset;
            Drawing.OnPostReset += Drawing_OnPostReset;
            Drawing.OnEndScene += Drawing_OnEndScene;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            PrintSuccess(string.Format("> Necrophos Script Loaded!"));
            DrawLib.Draw.Init();

            //TEXT CONFIG
            _description = new Font(
               Drawing.Direct3DDevice9,
               new FontDescription
               {
                   FaceName = "Segoe UI",
                   Height = 16,
                   OutputPrecision = FontPrecision.Default,
                   Quality = FontQuality.ClearType
               });
            _HP = new Font(
               Drawing.Direct3DDevice9,
               new FontDescription
               {
                   FaceName = "Segoe UI",
                   Height = 16,
                   OutputPrecision = FontPrecision.Default,
                   Quality = FontQuality.ClearType
               });

            _comboing = new Font(
               Drawing.Direct3DDevice9,
               new FontDescription
               {
                   FaceName = "Segoe UI",
                   Height = 22,
                   OutputPrecision = FontPrecision.Default,
                   Quality = FontQuality.ClearType
               });
        }

        public static void Tick(EventArgs args)
        {
            // initial things
            me = ObjectMgr.LocalHero;
            if ((Game.IsKeyDown(Menu.Item("2001").GetValue<KeyBind>().Key)) || (Game.IsKeyDown(Menu.Item("2002").GetValue<KeyBind>().Key)) || target == null || Utils.SleepCheck("selected"))
            {
                target = me.ClosestToMouseTarget(1000);
                Utils.Sleep(500,"selected");
            }
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame || Game.IsChatOpen)
                return;
            if (me.ClassID != ClassID.CDOTA_Unit_Hero_Necrolyte)
                return;
            if (me == null || target == null)
                return;
            // skills
            if (Qskill == null)
                Qskill = me.Spellbook.Spell1;
            if (Rskill == null)
                Rskill = me.Spellbook.Spell4;
            // itens
            Blink = me.FindItem("item_blink");
            shadow = me.FindItem("item_invis_sword");
            silveredge = me.FindItem("item_silver_edge");
            dagon = me.Inventory.Items.FirstOrDefault(x => x.Name.Contains("item_dagon"));
            ethereal = me.FindItem("item_ethereal_blade");
            veil = me.FindItem("item_veil_of_discord");
            euls = me.FindItem("item_cyclone");
            shivas = me.FindItem("item_shivas_guard");
            malevo = me.FindItem("item_orchid");
            forcestaff = me.FindItem("item_force_staff");
            int ComboDamage = 0;
            if (target != null && !target.IsIllusion)
                ComboDamage = Damagetokill();
            else
                ComboDamage = 0;
            //Starting Combo
            var blinkposition = ((me.Position - target.Position) * 300 / me.Distance2D(target) + target.Position);
            var IsLinkensProtected = (target.Modifiers.Any(x => x.Name == "modifier_item_sphere_target") || (target.FindItem("item_sphere") != null && (target.FindItem("item_sphere").Cooldown <= 0)));
            var _Is_in_Advantage = (target.Modifiers.Any(x => x.Name == "modifier_item_blade_mail_reflect") || target.Modifiers.Any(x => x.Name == "modifier_item_lotus_orb_active") || target.Modifiers.Any(x => x.Name == "modifier_nyx_assassin_spiked_carapace") || target.Modifiers.Any(x => x.Name == "modifier_templar_assassin_refraction_damage") || target.Modifiers.Any(x => x.Name == "modifier_ursa_enrage") || target.Modifiers.Any(x => x.Name == "modifier_abaddon_borrowed_time") || (target.Modifiers.Any(x => x.Name == "modifier_dazzle_shallow_grave")));
            var WindWalkMod = me.Modifiers.Any(x => x.Name == "modifier_item_silver_edge_windwalk" || x.Name == "modifier_item_invisibility_edge_windwalk");
            if (((Game.IsKeyDown(Menu.Item("2001").GetValue<KeyBind>().Key) && ComboDamage <= 0 && me.Distance2D(target) <= 1000 && target.IsVisible && target.IsAlive && !target.IsMagicImmune() && !target.IsIllusion && target != null && !_Is_in_Advantage) ||( me.Distance2D(target) <= 1000 && target.IsVisible && target.IsAlive && !target.IsMagicImmune() && !target.IsIllusion && target != null && Game.IsKeyDown(Menu.Item("2002").GetValue<KeyBind>().Key))) && Utils.SleepCheck("combo"))
            {
                if (me.CanCast() && !me.IsChanneling())
                {
                    if (WindWalkMod)
                    {
                        me.Attack(target);
                        Utils.Sleep(100 + me.GetTurnTime(target) * 500, "shadowblade");
                        Utils.ChainStun(me, 100 + Game.Ping, null, false);
                    }
                    else
                    {
                        if (IsLinkensProtected && (!WindWalkMod))
                        {
                            if (euls != null && euls.Cooldown <= 0 && IsLinkensProtected && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(euls.Name))
                            {
                                euls.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "euls");
                            }
                            else if (forcestaff != null && forcestaff.Cooldown <= 0 && IsLinkensProtected && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(forcestaff.Name))
                            {
                                forcestaff.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "forcestaff");
                            }
                            else if (dagon != null && dagon.Cooldown <= 0 && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(dagon.Name))
                            {
                                dagon.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "dagon");
                            }
                            else if (ethereal != null && ethereal.Cooldown <= 0 && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(ethereal.Name))
                            {
                                ethereal.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "ethereal");
                            }
                        }
                        else if (Rskill.Level > 0 && !IsLinkensProtected && (!WindWalkMod))
                        {
                            if (Blink != null && Blink.Cooldown <= 0 && me.Distance2D(blinkposition) > 300 && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(Blink.Name))
                            {
                                Blink.UseAbility(blinkposition);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "blink");
                                Utils.ChainStun(me, 500 + Game.Ping, null, false);
                            }
                            if (Rskill.Cooldown <= 0 && Menu.Item("Abilities: ").GetValue<AbilityToggler>().IsEnabled(Rskill.Name))
                            {
                                Rskill.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "ultimate");
                            }
                            if (malevo != null && malevo.Cooldown <= 0 && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(malevo.Name))
                            {
                                malevo.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "malevo");
                                Utils.ChainStun(me, 170 + Game.Ping, null, false);
                            }
                            if (ethereal != null && ethereal.Cooldown <= 0 && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(ethereal.Name))
                            {
                                ethereal.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "ethereal");
                                Utils.ChainStun(me, 200 + Game.Ping, null, false);
                            }
                            if (veil != null && veil.Cooldown <= 0 && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(veil.Name))
                            {
                                veil.UseAbility(target.Position);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "veil");
                            }
                            if (Qskill.Level > 0 && Qskill.Cooldown <= 0 && me.Distance2D(target) < 475 && Menu.Item("Abilities: ").GetValue<AbilityToggler>().IsEnabled(Qskill.Name))
                            {
                                Qskill.UseAbility();
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "Qskill");
                                Utils.ChainStun(me, 170 + Game.Ping, null, false);
                            }
                            if (dagon != null && dagon.Cooldown <= 0 && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(dagon.Name))
                            {
                                dagon.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "dagon");
                                Utils.ChainStun(me, 170 + Game.Ping, null, false);
                            }
                            if (shivas != null && shivas.Cooldown <= 0 && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(shivas.Name))
                            {
                                shivas.UseAbility();
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "shivas");
                            }
                            Utils.Sleep(200, "combo");
                        }
                    }
                }
            }

        }
        static int Damagetokill()
        {
            veil = me.FindItem("item_veil_of_discord");
            ethereal = me.FindItem("item_ethereal_blade");
            if (target == null || me == null)
                return (0);
            int damagetokill = 0;
            if (target != null) {
                if (Rskill.Level > 0 && Rskill.CanBeCasted() && !me.AghanimState() && Menu.Item("Abilities: ").GetValue<AbilityToggler>().IsEnabled(Rskill.Name))
                    damagetokill = (int)Math.Floor((UltDmg[Rskill.Level - 1] / (1 + UltDmg[Rskill.Level - 1])) * target.MaximumHealth);
                else if (Rskill.Level > 0 && Rskill.CanBeCasted() && me.AghanimState() && Menu.Item("Abilities: ").GetValue<AbilityToggler>().IsEnabled(Rskill.Name))
                    damagetokill = (int)Math.Floor((UltDmg[Rskill.Level - 1] / (1 + UltDmg[Rskill.Level - 1])) * target.MaximumHealth);
                else
                    damagetokill = 0;
            }
            //Magic Damage
            int dagondamage = 0;
            if (dagon != null && dagon.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(dagon.Name))
            {
                dagondamage = DDamage[dagon.Level - 1];
                damagetokill += dagondamage;
            }
            if (shivas != null && shivas.Cooldown <= 0 && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(shivas.Name))
            {
                damagetokill += 200;
            }
            if (Qskill.Level > 0 && Qskill.CanBeCasted() && Menu.Item("Abilities: ").GetValue<AbilityToggler>().IsEnabled(Qskill.Name) && (me.Distance2D(target) < 475 || (Blink != null && Blink.CanBeCasted())))
            {
                int[] Qskilldamage = new int[4] { 75, 125, 200, 275 };
                damagetokill += Qskilldamage[Qskill.Level - 1];
            }
            //Bonus ethereal and veil
            if (veil != null && veil.CanBeCasted() && !target.Modifiers.Any(x => x.Name == "modifier_item_veil_of_discord_debuff") && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(veil.Name))
                damagetokill = (int)(damagetokill * (1.25 * (1 - target.MagicDamageResist)));
            else if (ethereal != null && ethereal.CanBeCasted() && !target.Modifiers.Any(x => x.Name == "modifier_item_ethereal_blade_ethereal") && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(ethereal.Name))
                damagetokill = (int)(damagetokill * (1.40 * (1 - target.MagicDamageResist)));
            else if (veil != null && veil.CanBeCasted() && !target.Modifiers.Any(x => x.Name == "modifier_item_veil_of_discord_debuff") && ethereal != null && ethereal.CanBeCasted() && !target.Modifiers.Any(x => x.Name == "modifier_item_ethereal_blade_ethereal") && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(veil.Name) && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(ethereal.Name))
                damagetokill = (int)(damagetokill * (1.75 * (1 - target.MagicDamageResist)));
            else
                damagetokill = (int)(damagetokill * (1 - target.MagicDamageResist));
            //ethereal will not gain 40% bonus.
            if (ethereal != null && ethereal.CanBeCasted() && Menu.Item("Items: ").GetValue<AbilityToggler>().IsEnabled(ethereal.Name))
            {
                int damageethereal = (int)Math.Floor(((me.TotalIntelligence * 2) + 75));
                damagetokill += (int)(damageethereal * (1 - target.MagicDamageResist));
            }
            var WindWalkMod2 = me.Modifiers.Any(x => x.Name == "modifier_item_silver_edge_windwalk" || x.Name == "modifier_item_invisibility_edge_windwalk");
            if (WindWalkMod2)
            {
                if (me.Modifiers.Any(x => x.Name == "modifier_item_silver_edge_windwalk"))
                    damagetokill += (int)((me.MinimumDamage * (1 - (target.DamageResist))) + 225);
                else if (me.Modifiers.Any(x => x.Name == "modifier_item_invisibility_edge_windwalk"))
                    damagetokill += (int)((me.MinimumDamage * (1 - (target.DamageResist))) + 175);
            }
            // final calculation
            damagetokill = (int)(target.Health - damagetokill);
            return damagetokill;

        }
        static void Drawing_OnEndScene(EventArgs args)
        {
            try {
                if (Drawing.Direct3DDevice9 == null || Drawing.Direct3DDevice9.IsDisposed || !Game.IsInGame)
                    return;
                if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame)
                    return;
                if (Game.GameTime < 60)
                    DrawLib.Draw.DrawShadowText("Necrophos bruninjaman Script: Thanks to use :D,  this msg will disapear in: " + (int)(60 - Game.GameTime) + " seconds", 10, 600, Color.LimeGreen, _description);
                var player = ObjectMgr.LocalPlayer;
                var me = ObjectMgr.LocalHero;
                var target = me.ClosestToMouseTarget(1000);
                if (player == null || player.Team == Team.Observer || me.ClassID != ClassID.CDOTA_Unit_Hero_Necrolyte || me == null || target == null)
                    return;
                if (Rskill == null)
                    Rskill = me.Spellbook.Spell4;

                int ComboDamage = Damagetokill();
                var index = target.NetworkName.Remove(0, 16);
                if (ComboDamage > 0)
                {
                    DrawLib.Draw.DrawPanel(2, 400, 220, 46, 1, new ColorBGRA(0, 0, 139, 100));
                    DrawLib.Draw.DrawShadowText("HP-TO-KILL: " + ComboDamage + "  Hero: " + index, 4, 415, Color.Aquamarine, _HP);
                }
                else
                {
                    DrawLib.Draw.DrawPanel(2, 400, 220, 46, 1, new ColorBGRA(0, 0, 139, 100));
                    DrawLib.Draw.DrawShadowText("Killable      Hero: " + index, 4, 415, Color.DarkRed, _HP);
                }
                if ((Game.IsKeyDown(Menu.Item("2001").GetValue<KeyBind>().Key)) || (Game.IsKeyDown(Menu.Item("2002").GetValue<KeyBind>().Key)))
                {
                    DrawLib.Draw.DrawPanel(2, 360, 120, 25, 1, new ColorBGRA(238, 201, 0, 100));
                    DrawLib.Draw.DrawShadowText("Doing Combo!", 6, 360, Color.Goldenrod, _comboing);
                }
            }
            catch (Exception)
            {
            }
        }

        static void Drawing_OnPostReset(EventArgs args)
        {
            _HP.OnResetDevice();
            _comboing.OnResetDevice();
        }

        static void Drawing_OnPreReset(EventArgs args)
        {
            _HP.OnLostDevice();
            _comboing.OnLostDevice();
        }
        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            _HP.Dispose();
            _comboing.Dispose();
        }
        private static void PrintSuccess(string text, params object[] arguments) //thanks jumper attacker
        {
            PrintEncolored(text, ConsoleColor.Green, arguments);
        }
        private static void PrintEncolored(string text, ConsoleColor color, params object[] arguments) //thanks jumper attacker
        {
            var clr = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, arguments);
            Console.ForegroundColor = clr;
        }
    }
}
