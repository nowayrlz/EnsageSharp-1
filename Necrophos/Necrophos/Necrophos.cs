using System;
using System.Linq;
using Ensage;
using Ensage.Common.Extensions;
using System.Windows.Input;
using Ensage.Common;
using SharpDX.Direct3D9;
using SharpDX;
//At the end of your script, delete anything that's grey (Means it hasn't been used.)

namespace Necrophos
{
    class Necrophos
    {
        private static Ability Qskill, Rskill;
        private static Item Blink, shadow, silveredge, dagon, ethereal, veil, blademail, lotusorb,  euls, forcestaff, shivas, malevo;
        private static Hero me, target;

        private static int rangetocombo, totaldamage;
        private static Key toggleKey = Key.D;
        private static Key forceKey  = Key.E;
        private static bool active = true;
        private static bool active2 = true;
        private static Font _HP;
        private static Font _description;
        private static Font _comboing;

        private static double[] UltDmg = new double[3] { 0.4, 0.6, 0.9 };
        private static double[] AUltDmg = new double[3] { 0.6, 0.9, 1.2 };

        private static int[] DDamage = new int[5] { 400, 500, 600, 700, 800 };

        static void Main(string[] args)
        {
            Game.OnUpdate += Tick;
            Game.OnWndProc += config_key;
            Drawing.OnPreReset += Drawing_OnPreReset;
            Drawing.OnPostReset += Drawing_OnPostReset;
            Drawing.OnEndScene += Drawing_OnEndScene;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            Console.WriteLine("> Necrophos Script Loaded!");
            DrawLib.Draw.Init();

            //TEXT 
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
            target = me.ClosestToMouseTarget(1000);
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame)
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
            blademail = me.FindItem("item_blade_mail");
            euls = me.FindItem("item_cyclone");
            shivas = me.FindItem("item_shivas_guard");
            malevo = me.FindItem("item_orchid");
            forcestaff = me.FindItem("item_force_staff");
            int ComboDamage = 0;
            if (target != null)
                ComboDamage = Damagetokill();
            else
                ComboDamage = 0;
            // Blink Check
            if (Blink != null && Blink.CanBeCasted())
                rangetocombo = 1200;
            else
                rangetocombo = 600;
            //Starting Combo
            var LinkensMod = (target.Modifiers.Any(x => x.Name == "modifier_item_sphere_target") || (target.FindItem("item_sphere") != null && (target.FindItem("item_sphere").Cooldown <= 0)));
            var WindWalkMod = me.Modifiers.Any(x => x.Name == "modifier_item_silver_edge_windwalk");
            var ShadowMod = me.Modifiers.Any(x => x.Name == "modifier_item_invisibility_edge_windwalk");
            if ((active && ComboDamage <= 0 && me.Distance2D(target) <= rangetocombo && target.IsVisible && target.IsAlive && !target.IsMagicImmune() && !target.IsIllusion && target != null) ||( me.Distance2D(target) <= rangetocombo && target.IsVisible && target.IsAlive && !target.IsMagicImmune() && !target.IsIllusion && target != null && active2) && Utils.SleepCheck("combo"))
            {
                Console.WriteLine(ComboDamage);
                if (me.CanCast() && !me.IsChanneling())
                {
                    if (ShadowMod || WindWalkMod)
                    {
                        me.Attack(target);
                        Utils.Sleep(100 + me.GetTurnTime(target) * 500, "shadowblade");
                        Utils.ChainStun(me, 500 + Game.Ping, null, false);
                    }
                    else
                    {
                        if (LinkensMod && (!ShadowMod || !WindWalkMod))
                        {
                            if (euls != null && euls.Cooldown <= 0 && LinkensMod)
                            {
                                euls.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "euls");
                            }
                            else if (forcestaff != null && forcestaff.Cooldown <= 0 && LinkensMod)
                            {
                                forcestaff.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "forcestaff");
                            }
                            else if (dagon != null && dagon.Cooldown <= 0)
                            {
                                dagon.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "dagon");
                            }
                            else if (ethereal != null && ethereal.Cooldown <= 0)
                            {
                                ethereal.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "ethereal");
                            }
                        }
                        else if (Rskill.Level > 0 && !LinkensMod && (!ShadowMod || !WindWalkMod))
                        {
                            var blinkposition = ((me.Position - target.Position) * 300 / me.Distance2D(target) + target.Position);
                            if (Blink != null && Blink.Cooldown <= 0 && me.Distance2D(blinkposition) > 300)
                            {
                                Blink.UseAbility(blinkposition);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "blink");
                                Utils.ChainStun(me, 500 + Game.Ping, null, false);
                            }
                            if (Rskill.Cooldown <= 0)
                            {
                                Rskill.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "ultimate");
                            }
                            if (malevo != null && malevo.Cooldown <= 0)
                            {
                                malevo.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "malevo");
                                Utils.ChainStun(me, 170 + Game.Ping, null, false);
                            }
                            if (ethereal != null && ethereal.Cooldown <= 0)
                            {
                                ethereal.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "ethereal");
                                Utils.ChainStun(me, 200 + Game.Ping, null, false);
                            }
                            if (veil != null && veil.Cooldown <= 0)
                            {
                                veil.UseAbility(target.Position);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "veil");
                            }
                            if (Qskill.Level > 0 && Qskill.Cooldown <= 0 && me.Distance2D(target) < 475)
                            {
                                Qskill.UseAbility();
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "Qskill");
                                Utils.ChainStun(me, 170 + Game.Ping, null, false);
                            }
                            if (dagon != null && dagon.Cooldown <= 0)
                            {
                                dagon.UseAbility(target);
                                Utils.Sleep(100 + me.GetTurnTime(target) * 500, "dagon");
                                Utils.ChainStun(me, 170 + Game.Ping, null, false);
                            }
                            if (shivas != null && shivas.Cooldown <= 0)
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
                if (Rskill.Level > 0 && Rskill.CanBeCasted() && !me.AghanimState())
                    damagetokill = (int)Math.Floor((UltDmg[Rskill.Level - 1] / (1 + UltDmg[Rskill.Level - 1])) * target.MaximumHealth);
                else if (Rskill.Level > 0 && Rskill.CanBeCasted() && me.AghanimState())
                    damagetokill = (int)Math.Floor((UltDmg[Rskill.Level - 1] / (1 + UltDmg[Rskill.Level - 1])) * target.MaximumHealth);
                else
                    damagetokill = 0;
            }
            //Magic Damage
            int dagondamage = 0;
            if (dagon != null && dagon.CanBeCasted())
            {
                dagondamage = DDamage[dagon.Level - 1];
                damagetokill += dagondamage;
            }
            if (Qskill.Level > 0 && Qskill.CanBeCasted() && (me.Distance2D(target) < 475 || (Blink != null && Blink.CanBeCasted())))
            {
                int[] Qskilldamage = new int[4] { 75, 125, 200, 275 };
                damagetokill += Qskilldamage[Qskill.Level - 1];
            }
            //Bonus ethereal and veil
            if (veil != null && veil.CanBeCasted() && !target.Modifiers.Any(x => x.Name == "modifier_item_veil_of_discord_debuff"))
                damagetokill = (int)(damagetokill * (1.25 * (1 - target.MagicDamageResist)));
            else if (ethereal != null && ethereal.CanBeCasted() && !target.Modifiers.Any(x => x.Name == "modifier_item_ethereal_blade_ethereal"))
                damagetokill = (int)(damagetokill * (1.40 * (1 - target.MagicDamageResist)));
            else if (veil != null && veil.CanBeCasted() && !target.Modifiers.Any(x => x.Name == "modifier_item_veil_of_discord_debuff") && ethereal != null && ethereal.CanBeCasted() && !target.Modifiers.Any(x => x.Name == "modifier_item_ethereal_blade_ethereal"))
                damagetokill = (int)(damagetokill * (1.75 * (1 - target.MagicDamageResist)));
            else
                damagetokill = (int)(damagetokill * (1 - target.MagicDamageResist));
            //ethereal will not gain 40% bonus.
            if (ethereal != null && ethereal.CanBeCasted())
            {
                int damageethereal = (int)Math.Floor(((me.TotalIntelligence * 2) + 75));
                damagetokill += (int)(damageethereal * (1 - target.MagicDamageResist));
            }
            // final calculation
            damagetokill = (int)(target.Health - damagetokill);
            return damagetokill;

        }
        private static void config_key(WndEventArgs args)
        {
            if (!Game.IsChatOpen)
            {
                if (Game.IsKeyDown(toggleKey))
                    active = true;
                else
                    active = false;
                if (Game.IsKeyDown(forceKey))
                    active2 = true;
                else
                    active2 = false;
            }
        }
        static void Drawing_OnEndScene(EventArgs args)
        {
            try {
                if (Drawing.Direct3DDevice9 == null || Drawing.Direct3DDevice9.IsDisposed || !Game.IsInGame)
                    return;
                if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame)
                    return;
                if (Game.GameTime < 60)
                    DrawLib.Draw.DrawShadowText("Necrophos Script: ComboKey 'D' ForceComboKey 'E'  this msg will disapear in: " + (int)(60 - Game.GameTime) + " seconds", 10, 600, Color.Violet, _description);
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
                if (active)
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
    }
}
