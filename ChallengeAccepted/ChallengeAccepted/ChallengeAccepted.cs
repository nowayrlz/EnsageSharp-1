using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ensage;
using Ensage.Common.Extensions;
using Ensage.Common;
using SharpDX.Direct3D9;
using SharpDX;
using Ensage.Common.Menu;

namespace ChallengeAccepted
{
    class ChallengeAccepted
    {
        private static readonly Menu Menu = new Menu("Challenge Accepted", "ChallengeAccepted", true);
        private static readonly Menu _item_config = new Menu("Ofensive Items", "Ofensive Items");
        private static readonly Menu _1_item_config = new Menu("Defensive Items", "Defensive Items");
        private static readonly Menu _2_item_config = new Menu("Remove Linkens Items", "Remove Linkens Items");
        private static readonly Menu _3_item_config = new Menu("Consume Items", "Consume Items");
        private static readonly Menu _skill_config = new Menu("Skills", "Skills");
        private static Ability Duel, Heal, Odds;
        private static Item blink, armlet, blademail, bkb, abyssal, mjollnir, halberd, medallion, madness, urn, satanic, solar, dust, sentry, mango, arcane, buckler, crimson, lotusorb, cheese, magistick, magicwand, soulring, force, cyclone, vyse;
        private static Hero me, target;
        private static readonly Dictionary<string, bool> ofensive_items = new Dictionary<string, bool>
            {
                {"item_blink",true},
                {"item_armlet",true},
                {"item_abyssal_blade",true},
                {"item_mjollnir",true},
                {"item_medallion_of_courage",true},
                {"item_mask_of_madness",true},
                {"item_urn_of_shadows",true},
                {"item_solar_crest",true}
            };
        private static readonly Dictionary<string, bool> defensive_items = new Dictionary<string, bool>
            {
                {"item_black_king_bar",true},
                {"item_armlet",true},
                {"item_blade_mail",true},
                {"item_satanic",true},
                {"item_lotus_orb",true},
                {"item_magic_stick",true},
                {"item_magic_wand",true}
            };
        private static readonly Dictionary<string, bool> remove_linkens_items = new Dictionary<string, bool>
            {
                {"item_heavens_halberd",true},
                {"item_force_staff",true},
                {"item_cyclone",true},
                {"item_sheepstick",true}
            };
        private static readonly Dictionary<string, bool> skills = new Dictionary<string, bool>
            {
                {"legion_commander_press_the_attack",true},
                {"legion_commander_overwhelming_odds",true}
            };
        private static readonly Dictionary<string, bool> consume_items = new Dictionary<string, bool>
            {
                {"item_dust",true},
                {"item_ward_sentry",true},
                {"item_enchanted_mango",true},
                {"item_arcane_boots",true},
                {"item_buckler",true},
                {"item_crimson_guard",true},
                {"item_cheese",true},
                {"item_soul_ring",true}
            };
        static void Main(string[] args)
        {
            Menu.AddItem(new MenuItem("DUEL!", "DUEL!").SetValue(new KeyBind('D', KeyBindType.Press)));
            Menu.AddSubMenu(_item_config);
            Menu.AddSubMenu(_1_item_config);
            Menu.AddSubMenu(_2_item_config);
            Menu.AddSubMenu(_3_item_config);
            Menu.AddSubMenu(_skill_config);
            _item_config.AddItem(new MenuItem("Ofensive Items", "Ofensive Items").SetValue(new AbilityToggler(ofensive_items)));
            _1_item_config.AddItem(new MenuItem("Defensive Items", "Defensive Items").SetValue(new AbilityToggler(defensive_items)));
            _2_item_config.AddItem(new MenuItem("Remove Linkens Items", "Remove Linkens Items").SetValue(new AbilityToggler(remove_linkens_items)));
            _3_item_config.AddItem(new MenuItem("Consume Items", "Consume Items").SetValue(new AbilityToggler(consume_items)));
            _skill_config.AddItem(new MenuItem("Skills", "Skills").SetValue(new AbilityToggler(skills)));
            Menu.AddToMainMenu();
            PrintSuccess(">Challenge Accepted");
            Game.OnUpdate += Working;
        }
        public static void Working(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null || me.ClassID != ClassID.CDOTA_Unit_Hero_Legion_Commander)
                return;
            if (Game.IsKeyDown(Menu.Item("DUEL!").GetValue<KeyBind>().Key) && !Game.IsChatOpen)
            {
                FindItems();
                target = me.ClosestToMouseTarget(1000);
                if (target != null && target.IsAlive && !target.IsIllusion && !target.IsInvul() && ( blink != null ? me.Distance2D(target) <= 1300 : me.Distance2D(target) <= 600))
                {
                    if (me.CanAttack() && me.CanCast())
                    {
                        if (CanInvisCrit(me))
                            me.Attack(target);
                        else
                        {
                            manacheck();
                            if (target.IsLinkensProtected())
                            {
                                if (cyclone.CanBeCasted() && Utils.SleepCheck("CycloneRemoveLinkens") && cyclone != null && Menu.Item("Remove Linkens Items").GetValue<AbilityToggler>().IsEnabled(cyclone.Name))
                                {
                                    if(blink.CanBeCasted() && blink != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(blink.Name))
                                        blink.UseAbility(target.Position);
                                    cyclone.UseAbility(target);
                                    Utils.Sleep(100, "CycloneRemoveLinkens");
                                }
                                else if (force.CanBeCasted() && Utils.SleepCheck("ForceRemoveLinkens") && force != null && Menu.Item("Remove Linkens Items").GetValue<AbilityToggler>().IsEnabled(force.Name))
                                {
                                    if (blink.CanBeCasted() && blink != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(blink.Name))
                                        blink.UseAbility(target.Position);
                                    force.UseAbility(target);
                                    Utils.Sleep(100, "ForceRemoveLinkens");
                                }
                                else if (halberd.CanBeCasted() && Utils.SleepCheck("halberdLinkens") && halberd != null &&  Menu.Item("Remove Linkens Items").GetValue<AbilityToggler>().IsEnabled(halberd.Name))
                                {
                                    if (blink.CanBeCasted() && blink != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(blink.Name))
                                        blink.UseAbility(target.Position);
                                    halberd.UseAbility(target);
                                    Utils.Sleep(100, "halberdLinkens");
                                }
                                else if (vyse.CanBeCasted() && Utils.SleepCheck("vyseLinkens") && vyse != null && Menu.Item("Remove Linkens Items").GetValue<AbilityToggler>().IsEnabled(vyse.Name))
                                {
                                    if (blink.CanBeCasted() && blink != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(blink.Name))
                                        blink.UseAbility(target.Position);
                                    vyse.UseAbility(target);
                                    Utils.Sleep(100, "vyseLinkens");
                                }
                                else if (abyssal.CanBeCasted() && Utils.SleepCheck("abyssal") && abyssal != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(vyse.Name))
                                {
                                    if (blink.CanBeCasted() && blink != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(blink.Name))
                                        blink.UseAbility(target.Position);
                                    abyssal.UseAbility(target);
                                    Utils.Sleep(100, "abyssal");
                                }
                            }
                            else
                            {
                                if (UsedInvis(target))
                                {
                                    if (me.Distance2D(target) <= 500)
                                    {
                                        if (dust != null && dust.CanBeCasted() && Utils.SleepCheck("dust") && dust != null && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(dust.Name))
                                        {
                                            dust.UseAbility();
                                            Utils.Sleep(100,"dust");
                                        }
                                        else if (sentry != null && sentry.CanBeCasted() && Utils.SleepCheck("sentry") && sentry != null && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(sentry.Name))
                                        {
                                            sentry.UseAbility(me.Position);
                                            Utils.Sleep(100, "sentry");
                                        }
                                    }
                                }
                                uint elsecount = 1;
                                if (Utils.SleepCheck("combo"))
                                {
                                    if (Heal != null && Heal.Level > 0 && Heal.Cooldown <= 0 && Menu.Item("Skills").GetValue<AbilityToggler>().IsEnabled(Heal.Name) && elsecount == 1)
                                        Heal.UseAbility(me);
                                    else
                                        elsecount += 1;
                                    if (crimson != null && crimson.Cooldown <= 0 && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(crimson.Name) && elsecount == 2)
                                        crimson.UseAbility();
                                    else
                                        elsecount += 1;
                                    if (buckler != null && buckler.Cooldown <= 0 && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(buckler.Name) && elsecount == 3)
                                        buckler.UseAbility();
                                    else
                                        elsecount += 1;
                                    if (lotusorb != null && lotusorb.Cooldown <= 0 && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(lotusorb.Name) && elsecount == 4)
                                        lotusorb.UseAbility(me);
                                    else
                                        elsecount += 1;
                                    if (bkb != null && bkb.Cooldown <= 0 && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(bkb.Name) && elsecount == 5)
                                        bkb.UseAbility();
                                    else
                                        elsecount += 1;
                                    if (mjollnir != null && mjollnir.Cooldown <= 0 && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(mjollnir.Name) && elsecount == 6)
                                        mjollnir.UseAbility(me);
                                    else
                                        elsecount += 1;
                                    if (armlet != null && !armlet.IsToggled && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(armlet.Name) && elsecount == 7 && Utils.SleepCheck("armlet"))
                                    {
                                        armlet.ToggleAbility();
                                        Utils.Sleep(300, "armlet");
                                    }
                                    else
                                        elsecount += 1;
                                    if (madness != null && madness.Cooldown <= 0 && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(madness.Name) && elsecount == 8)
                                        madness.UseAbility();
                                    else
                                        elsecount += 1;
                                    if (abyssal != null && abyssal.Cooldown <= 0 && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(abyssal.Name) && elsecount == 9)
                                        abyssal.UseAbility(target);
                                    else
                                        elsecount += 1;
                                    if (blink != null && blink.Cooldown <= 0 && me.Distance2D(target) <= 1300 && me.Distance2D(target) >= 200 && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(blink.Name) && elsecount == 10)
                                        blink.UseAbility(target.Position);
                                    else
                                        elsecount += 1;
                                    if (urn != null && urn.CurrentCharges > 0 && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(urn.Name) && elsecount == 11)
                                        urn.UseAbility(target);
                                    else
                                        elsecount += 1;
                                    if (solar != null && solar.CanBeCasted() && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(solar.Name) && elsecount == 12)
                                        solar.UseAbility(target);
                                    else
                                        elsecount += 1;
                                    if (medallion != null && medallion.CanBeCasted() && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(medallion.Name) && elsecount == 13)
                                        medallion.UseAbility(target);
                                    else
                                        elsecount += 1;
                                    if (blademail != null && blademail.Cooldown <= 0 && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(blademail.Name) && elsecount == 14)
                                        blademail.UseAbility();
                                    else
                                        elsecount += 1;
                                    if (satanic != null && satanic.Cooldown <= 0 && me.Health <= me.MaximumHealth * 0.5 && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(satanic.Name) && elsecount == 15)
                                        satanic.UseAbility();
                                    else
                                        elsecount += 1;
                                    if (Duel != null && Duel.Cooldown <= 0 && !target.IsLinkensProtected() && !target.Modifiers.Any(x => x.Name == "modifier_abaddon_borrowed_time") && Utils.SleepCheck("Duel") && elsecount == 16)
                                    {
                                        Duel.UseAbility(target);
                                        Utils.Sleep(100, "Duel");
                                    }
                                    Utils.Sleep(150, "combo");
                                } 
                                //if (Heal.CanBeCasted() && Utils.SleepCheck("heal") && stage == 0 && me.Distance2D(target) <= 1300 && Heal != null && Menu.Item("Skills").GetValue<AbilityToggler>().IsEnabled(Heal.Name))
                                //{
                                //    Heal.UseAbility(me);
                                //    Utils.Sleep(100 + (Heal.GetCastDelay(me, me, true, false, Heal.Name, false) * 1000), "heal");
                                //}
                                //else if ((!Heal.CanBeCasted() && stage == 0) || Heal != null && !Menu.Item("Skills").GetValue<AbilityToggler>().IsEnabled(Heal.Name))
                                //    stage = 1;
                                //if (blink.CanBeCasted() && stage == 1 && me.Distance2D(target) <= 1300 && me.Distance2D(target) >= 200 && Utils.SleepCheck("blink") && blink != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(blink.Name))
                                //{
                                //    blink.UseAbility(target.Position);
                                //    Utils.Sleep(10 + (blink.GetCastDelay(me, target, true, false, blink.Name, false) * 1000), "blink");
                                //}
                                //else if ((!blink.CanBeCasted() && stage == 1 || (me.Distance2D(target) <= 200) && stage == 1) || blink != null && !Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(blink.Name) && stage == 1)
                                //    stage = 2;
                                //if (UsedInvis(target) && stage == 2)
                                //{
                                //    if (me.Distance2D(target) <= 500)
                                //    {
                                //        if (dust != null && dust.CanBeCasted() && Utils.SleepCheck("dust") && dust != null && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(dust.Name))
                                //        {
                                //            dust.UseAbility();
                                //            Utils.Sleep(100 + (dust.GetCastDelay(me, me, true, false, dust.Name, false) * 1000), "dust");
                                //        }
                                //        else if (sentry != null && sentry.CanBeCasted() && Utils.SleepCheck("sentry") && sentry != null && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(sentry.Name))
                                //        {
                                //            sentry.UseAbility(me.Position);
                                //            Utils.Sleep(100 + (sentry.GetCastDelay(me, me, true, false, sentry.Name, false) * 1000), "sentry");
                                //        }
                                //        stage = 3;
                                //    }
                                //}
                                //else if (stage == 2)
                                //    stage = 3;
                                //if (crimson.CanBeCasted() && stage == 3 && Utils.SleepCheck("Crimson") && crimson != null && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(crimson.Name))
                                //{
                                //    crimson.UseAbility();
                                //    Utils.Sleep(30 + (crimson.GetCastDelay(me, me, true, false, crimson.Name, false) * 1000), "Crimson");
                                //}
                                //else if ((!crimson.CanBeCasted() && stage == 3) || crimson != null && !Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(crimson.Name) && stage == 3)
                                //    stage = 4;
                                //if (buckler.CanBeCasted() && stage == 4 && Utils.SleepCheck("buckler") && buckler != null && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(buckler.Name))
                                //{
                                //    buckler.UseAbility();
                                //    Utils.Sleep(30 + (buckler.GetCastDelay(me, me, true, false, buckler.Name, false) * 1000), "buckler");
                                //}
                                //else if ((!buckler.CanBeCasted() && stage == 4) || buckler != null && !Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(buckler.Name) && stage == 4)
                                //    stage = 5;
                                //if (lotusorb.CanBeCasted() && stage == 5 && Utils.SleepCheck("lotus") && lotusorb != null && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(lotusorb.Name))
                                //{
                                //    lotusorb.UseAbility(me);
                                //    Utils.Sleep(30 + (lotusorb.GetCastDelay(me, me, true, false, lotusorb.Name, false) * 1000), "lotus");
                                //}
                                //else if ((!lotusorb.CanBeCasted() && stage == 5) || lotusorb != null && !Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(lotusorb.Name) && stage == 5)
                                //    stage = 6;
                                //if (bkb.CanBeCasted() && stage == 6 && Utils.SleepCheck("bkb") && bkb != null && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(bkb.Name))
                                //{
                                //    bkb.UseAbility(me);
                                //    Utils.Sleep(30 + (bkb.GetCastDelay(me, me, true, false, bkb.Name, false) * 1000), "bkb");
                                //}
                                //else if ((!bkb.CanBeCasted() && stage == 6) || bkb != null && !Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(bkb.Name) && stage == 6)
                                //    stage = 7;
                                //if (mjollnir.CanBeCasted() && stage == 7 && Utils.SleepCheck("mjollnir") && mjollnir != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(mjollnir.Name))
                                //{
                                //    mjollnir.UseAbility(me);
                                //    Utils.Sleep(30 + (mjollnir.GetCastDelay(me, me, true, false, mjollnir.Name, false) * 1000), "mjollnir");
                                //}
                                //else if ((!mjollnir.CanBeCasted() && stage == 7) || mjollnir != null && !Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(mjollnir.Name) && stage == 7)
                                //    stage = 8;
                                //if (armlet.CanBeCasted() && !armlet.IsToggled && stage == 8 && Utils.SleepCheck("armlet") && armlet != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(armlet.Name))
                                //{
                                //    armlet.ToggleAbility();
                                //    Utils.Sleep(30 + (armlet.GetCastDelay(me, me, true, false, armlet.Name, false) * 1200), "armlet");
                                //}
                                //else if (((armlet == null || armlet.IsToggled) && stage == 8) || armlet != null && !Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(armlet.Name) && stage == 8)
                                //    stage = 9;
                                //if (madness.CanBeCasted() && stage == 9 && Utils.SleepCheck("madness") && madness != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(madness.Name))
                                //{
                                //    madness.UseAbility();
                                //    Utils.Sleep(30 + (madness.GetCastDelay(me, me, true, false, madness.Name, false) * 1000), "madness");
                                //}
                                //else if ((!madness.CanBeCasted() && stage == 9) || (madness != null && !Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(madness.Name) && stage == 9))
                                //    stage = 10;
                                //if (abyssal.CanBeCasted() && stage == 10 && Utils.SleepCheck("abyssal") && abyssal != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(abyssal.Name))
                                //{
                                //    abyssal.UseAbility(target);
                                //    Utils.Sleep(30 + (abyssal.GetCastDelay(me, target, true, false, abyssal.Name, false) * 1000), "abyssal");
                                //}
                                //else if ((!abyssal.CanBeCasted() && stage == 10) || abyssal != null && !Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(abyssal.Name) && stage == 10)
                                //    stage = 11;
                                //if (urn.CanBeCasted() && stage == 11 && Utils.SleepCheck("urn") && urn != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(urn.Name))
                                //{
                                //    urn.UseAbility(target);
                                //    Utils.Sleep(30 + (urn.GetCastDelay(me, target, true, false, urn.Name, false) * 1000), "urn");
                                //}
                                //else if ((!urn.CanBeCasted() && stage == 11) || urn != null && !Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(urn.Name) && stage == 11)
                                //    stage = 12;
                                //if (solar.CanBeCasted() && stage == 12 && Utils.SleepCheck("solar") && solar != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(solar.Name))
                                //{
                                //    solar.UseAbility(target);
                                //    Utils.Sleep(30 + (solar.GetCastDelay(me, target, true, false, solar.Name, false) * 1000), "solar");
                                //}
                                //else if ((!solar.CanBeCasted() && stage == 12) || solar != null && !Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(solar.Name) && stage == 12)
                                //    stage = 13;
                                //if (medallion.CanBeCasted() && stage == 13 && Utils.SleepCheck("medallion") && medallion != null && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(medallion.Name))
                                //{
                                //    medallion.UseAbility(target);
                                //    Utils.Sleep(30 + (medallion.GetCastDelay(me, target, true, false, medallion.Name, false) * 1000), "medallion");
                                //}
                                //else if ((!medallion.CanBeCasted() && stage == 13) || medallion != null && !Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(medallion.Name) && stage == 14)
                                //    stage = 14;
                                //if (blademail.CanBeCasted() && stage == 14 && Utils.SleepCheck("blademail") && blademail != null && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(blademail.Name))
                                //{
                                //    blademail.UseAbility();
                                //    Utils.Sleep(30 + (blademail.GetCastDelay(me, me, true, false, blademail.Name, false) * 1000), "blademail");
                                //}
                                //else if ((!blademail.CanBeCasted() && stage == 14) || blademail != null && !Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(blademail.Name) && stage == 14)
                                //    stage = 15;
                                //if (satanic.CanBeCasted() && stage == 15 && me.Health <= me.MaximumHealth * 0.5 && Utils.SleepCheck("satanic") && satanic != null && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(satanic.Name))
                                //{
                                //    satanic.UseAbility();
                                //    Utils.Sleep(30 + (satanic.GetCastDelay(me, me, true, false, satanic.Name, false) * 1000), "satanic");
                                //}
                                //else if (((!satanic.CanBeCasted() && stage == 15) || (me.Health >= me.MaximumHealth * 0.5 && stage == 16)) || satanic != null && !Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(satanic.Name) && stage == 15)
                                //    stage = 16;
                            }
                        }
                    }
                }
            }
        }
        static bool UsedInvis(Hero v)
        {
            if (v.Modifiers.Any(
                    x =>
                   (x.Name == "modifier_bounty_hunter_wind_walk" ||
                    x.Name == "modifier_riki_permanent_invisibility" ||
                    x.Name == "modifier_mirana_moonlight_shadow" || x.Name == "modifier_treant_natures_guise" ||
                    x.Name == "modifier_weaver_shukuchi" ||
                    x.Name == "modifier_broodmother_spin_web_invisible_applier" ||
                    x.Name == "modifier_item_invisibility_edge_windwalk" || x.Name == "modifier_rune_invis" ||
                    x.Name == "modifier_clinkz_wind_walk" || x.Name == "modifier_item_shadow_amulet_fade" ||
                    x.Name == "modifier_item_silver_edge_windwalk" ||
                    x.Name == "modifier_item_edge_windwalk" ||
                    x.Name == "modifier_nyx_assassin_vendetta" ||
                    x.Name == "modifier_invisible" ||
                    x.Name == "modifier_invoker_ghost_walk_enemy")))
                return true;
            else
                return false;
        }
        static bool CanInvisCrit(Hero x)
        {
            if (x.Modifiers.Any(m => m.Name == "modifier_item_invisibility_edge_windwalk" || m.Name == "modifier_item_silver_edge_windwalk"))
                return true;
            else
                return false;
        }
        static bool IsLinkensProtected(Hero x)
        {
            if (x.Modifiers.Any(m => m.Name == "modifier_item_sphere_target") || x.FindItem("item_sphere") != null && x.FindItem("item_sphere").Cooldown <= 0)
                return true;
            else
                return false;
        }
        static void FindItems()
        {
            blink = me.FindItem("item_blink");
            armlet = me.FindItem("item_armlet");
            blademail = me.FindItem("item_blade_mail");
            bkb = me.FindItem("item_black_king_bar");
            abyssal = me.FindItem("item_abyssal_blade");
            mjollnir = me.FindItem("item_mjollnir");
            halberd = me.FindItem("item_heavens_halberd");
            medallion = me.FindItem("item_medallion_of_courage");
            madness = me.FindItem("item_mask_of_madness");
            urn = me.FindItem("item_urn_of_shadows");
            satanic = me.FindItem("item_satanic");
            solar = me.FindItem("item_solar_crest");
            dust = me.FindItem("item_dust");
            sentry = me.FindItem("item_ward_sentry");
            mango = me.FindItem("item_enchanted_mango");
            arcane = me.FindItem("item_arcane_boots");
            buckler = me.FindItem("item_buckler");
            crimson = me.FindItem("item_crimson_guard");
            lotusorb = me.FindItem("item_lotus_orb");
            cheese = me.FindItem("item_cheese");
            magistick = me.FindItem("item_magic_stick");
            magicwand = me.FindItem("item_magic_wand");
            soulring = me.FindItem("item_soul_ring");
            force = me.FindItem("item_force_staff");
            cyclone = me.FindItem("item_cyclone");
            vyse = me.FindItem("item_sheepstick");
            Duel = me.Spellbook.SpellR;
            Heal = me.Spellbook.SpellW;
            Odds = me.Spellbook.SpellQ;
        }
        static void manacheck()
        {
            uint manacost = 0;
            if (me.IsAlive)
            {
                if (blademail != null && blademail.Cooldown <= 0 && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(blademail.Name))
                    manacost += blademail.ManaCost;
                if (abyssal != null && abyssal.Cooldown <= 0 &&  Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(abyssal.Name))
                    manacost += abyssal.ManaCost;
                if (mjollnir != null && mjollnir.Cooldown <= 0 && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(mjollnir.Name))
                    manacost += mjollnir.ManaCost;
                if (halberd != null && halberd.Cooldown <= 0 && Menu.Item("Remove Linkens Items").GetValue<AbilityToggler>().IsEnabled(halberd.Name))
                    manacost += halberd.ManaCost;
                if (madness != null && madness.Cooldown <= 0 && Menu.Item("Ofensive Items").GetValue<AbilityToggler>().IsEnabled(madness.Name))
                    manacost += madness.ManaCost;
                if (lotusorb != null && lotusorb.Cooldown <= 0 && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(lotusorb.Name))
                    manacost += lotusorb.ManaCost;
                if (buckler != null && buckler.Cooldown <= 0 && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(buckler.Name))
                    manacost += buckler.ManaCost;
                if (crimson != null && crimson.Cooldown <= 0 && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(crimson.Name))
                    manacost += crimson.ManaCost;
                if (force != null && force.Cooldown <= 0 && Menu.Item("Remove Linkens Items").GetValue<AbilityToggler>().IsEnabled(force.Name))
                    manacost += force.ManaCost;
                if (cyclone != null && cyclone.CanBeCasted() && Menu.Item("Remove Linkens Items").GetValue<AbilityToggler>().IsEnabled(cyclone.Name))
                    manacost += cyclone.ManaCost;
                if (vyse != null && vyse.Cooldown <= 0 && Menu.Item("Remove Linkens Items").GetValue<AbilityToggler>().IsEnabled(vyse.Name))
                    manacost += vyse.ManaCost;
                if (Heal.Cooldown <= 0 && Heal.Level > 0 && Menu.Item("Skills").GetValue<AbilityToggler>().IsEnabled(Heal.Name))
                    manacost += Heal.ManaCost;
                if (Duel.Cooldown <= 0 && Duel.Level > 0)
                    manacost += Heal.ManaCost;
            }
            if (manacost > me.Mana)
            {
                if (mango.CanBeCasted() && mango != null && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(mango.Name) && Utils.SleepCheck("FastMango"))
                {
                    mango.UseAbility();
                    Utils.Sleep(Game.Ping, "FastMango");
                }
                if (arcane.CanBeCasted() && arcane != null && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(arcane.Name) && Utils.SleepCheck("FastArcane"))
                {
                    arcane.UseAbility();
                    Utils.Sleep(Game.Ping, "FastArcane");
                }
                if (magicwand.CanBeCasted() && magicwand != null && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(magicwand.Name) && Utils.SleepCheck("Fastmagicwand"))
                {
                    magicwand.UseAbility();
                    Utils.Sleep(Game.Ping, "Fastmagicwand");
                }
                if (magistick.CanBeCasted() && magistick != null && Menu.Item("Defensive Items").GetValue<AbilityToggler>().IsEnabled(magistick.Name) && Utils.SleepCheck("Fastmagistick"))
                {
                    magistick.UseAbility();
                    Utils.Sleep(Game.Ping, "Fastmagistick");
                }
                if ((cheese.CanBeCasted() && cheese != null && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(cheese.Name) && me.Health <= me.MaximumHealth * 0.5) || me.Health <= me.MaximumHealth * 0.30 && Utils.SleepCheck("FastCheese"))
                {
                    cheese.UseAbility();
                    Utils.Sleep(Game.Ping, "FastCheese");
                }
                if (soulring.CanBeCasted() && soulring != null && Menu.Item("Consume Items").GetValue<AbilityToggler>().IsEnabled(soulring.Name) && Utils.SleepCheck("FastSoulRing"))
                {
                    soulring.UseAbility();
                    Utils.Sleep(Game.Ping, "FastSoulRing");
                }
            }
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
