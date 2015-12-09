using System;
using System.Linq;
using Ensage;
using Ensage.Common.Extensions;
using Ensage.Common;
using SharpDX;
using Ensage.Common.Menu;


namespace ES_NINJA
{
    class ES_NINJA
    {
        private static Hero me, stunned, boulder_slow;
        private static Ability Qskill, Wskill, Eskill, DRemnant, Rskill, Fskill;
        private static Unit stone_for_combo, stone_nearly;
        private static Vector3 Mouse_Position;
        private static System.Collections.Generic.List<Unit> stone;
        private static System.Collections.Generic.IEnumerable<Ensage.Hero> stone_peoples, stun_target;
        private static int stage = 0, stage_combo2 = 0, stage_combo3 = 0, stage_combo4 = 0;
        private static bool auto_atack = false, key_active = false, key_active_2 = false/*, key_active_3 = false*/, key_active_4 = false, auto_atack_after_spell = false;

        private static readonly Menu Menu = new Menu("ES_NINJA", "ES_NINJA", true);
        private static readonly Menu _hotkeys_config = new Menu("HotKeys", "Hotkeys");
        //private static Item Blink, shadow, silveredge, dagon, ethereal, veil, blademail, lotusorb, shivas, malevo;
        static void Main(string[] args)
        {
            Menu.AddSubMenu(_hotkeys_config);
            _hotkeys_config.AddItem(new MenuItem("hotkeycombo1", "Combo kick pull roll").SetValue(new KeyBind('T', KeyBindType.Press)));
            _hotkeys_config.AddItem(new MenuItem("hotkeycombo3", "Combo aghanim").SetValue(new KeyBind('G', KeyBindType.Press)));
            _hotkeys_config.AddItem(new MenuItem("hotkeycombo5", "Combo reverse").SetValue(new KeyBind('Y', KeyBindType.Press)));
            //_hotkeys_config.AddItem(new MenuItem("hotkeycombo4", "Combo Stun and Roll").SetValue(new KeyBind('U', KeyBindType.Press)));
            _hotkeys_config.AddItem(new MenuItem("hotkeycombo2", "Cancel Combo").SetValue(new KeyBind('S', KeyBindType.Press)));
            Menu.AddToMainMenu();
            Game.OnUpdate += Tick;
            Drawing.OnDraw += Magnetize;
            PrintSuccess(string.Format("> ES_NINJA Script Loaded!"));
        }
        public static void Tick(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;
            if (me.ClassID != ClassID.CDOTA_Unit_Hero_EarthSpirit)
                return;


            //SKILLS
            
            if (Qskill == null)
                Qskill = me.Spellbook.SpellQ;
            if (Wskill == null)
                Wskill = me.Spellbook.SpellW;
            if (Eskill == null)
                Eskill = me.Spellbook.SpellE;
            if (DRemnant == null)
                DRemnant = me.Spellbook.SpellD;
            if (Rskill == null)
                Rskill = me.Spellbook.SpellR;
            if (Fskill == null)
                Fskill = me.Spellbook.SpellF;

            // UNIT VARIABLES

            stone_for_combo = ObjectMgr.GetEntities<Unit>().Where(x => x.ClassID == ClassID.CDOTA_Unit_Earth_Spirit_Stone && me.Distance2D(x.NetworkPosition) < 200).FirstOrDefault();
            stone = ObjectMgr.GetEntities<Unit>().Where(x => x.ClassID == ClassID.CDOTA_Unit_Earth_Spirit_Stone && x.NetworkPosition.Distance2D(me.NetworkPosition) <= 1300).ToList();
            if (boulder_slow == null || stage_combo4 == 0)
                boulder_slow = ObjectMgr.GetEntities<Hero>().Where(x => x.Team != me.Team && x.IsVisible && !x.IsIllusion && x.NetworkPosition.Distance2D(me.NetworkPosition) < 200 && x.Modifiers.Any(y => y.Name == "modifier_earth_spirit_rolling_boulder_slow")).FirstOrDefault();

            //KEYS TOGGLE

            if ((Game.IsKeyDown(_hotkeys_config.Item("hotkeycombo1").GetValue<KeyBind>().Key)) && !Game.IsChatOpen && stunned == null)
                key_active = true;
            if ((Game.IsKeyDown(_hotkeys_config.Item("hotkeycombo3").GetValue<KeyBind>().Key)) && !Game.IsChatOpen)
                key_active_2 = true;
            if ((Game.IsKeyDown(_hotkeys_config.Item("hotkeycombo5").GetValue<KeyBind>().Key)) && !Game.IsChatOpen)
                key_active_4 = true;

            if (key_active == false)
            { stage = 0; key_active = false; stunned = null; }
            if (stage == 2 && stunned == null)
                stunned = ObjectMgr.GetEntities<Hero>().Where(x => x.Modifiers.Any(y => y.Name == "modifier_stunned") && !x.IsIllusion && x.Team != me.Team && x.IsVisible).FirstOrDefault();

            if (me.CanCast() && !me.IsChanneling() && key_active)
            {
                Mouse_Position = Game.MousePosition;
                if ((!Eskill.CanBeCasted() && !Qskill.CanBeCasted() && !Wskill.CanBeCasted() && (!DRemnant.CanBeCasted() && stone_for_combo == null) || ((!DRemnant.CanBeCasted() && stone_for_combo == null))) || (Game.IsKeyDown(_hotkeys_config.Item("hotkeycombo2").GetValue<KeyBind>().Key)))
                {
                    key_active = false;
                    stage = 0;
                    if (auto_atack_after_spell)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 1");
                        auto_atack_after_spell = false;
                    }
                    if (auto_atack)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack 1");
                        auto_atack = false;
                    }
                    stunned = null;
                }
                if (stage == 0 && Utils.SleepCheck("auto_atack_change"))
                {
                    if (Game.GetConsoleVar("dota_player_units_auto_attack_after_spell").GetInt() == 1)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 0");
                        auto_atack_after_spell = true;
                    }
                    else
                        auto_atack_after_spell = false;
                    if (Game.GetConsoleVar("dota_player_units_auto_attack").GetInt() == 1)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack 0");
                        auto_atack = true;
                    }
                    else
                        auto_atack = false;
                    stage = 1;
                    Utils.Sleep(Game.Ping + 50, "auto_atack_change");

                }
                else if (stage == 1 && Utils.SleepCheck("Qskill") && Utils.SleepCheck("auto_atack_change"))
                {
                    if (DRemnant.CanBeCasted() && Utils.SleepCheck("DRemnant") && stone_for_combo == null && (Qskill.CanBeCasted() && Wskill.CanBeCasted() && Eskill.CanBeCasted()))
                    {
                        DRemnant.UseAbility(me.NetworkPosition);
                        Utils.Sleep((int)Game.Ping + 500, "DRemnant");
                    }
                    if (Qskill.CanBeCasted())
                    {
                        Qskill.UseAbility((Mouse_Position - me.NetworkPosition) * 200 / Mouse_Position.Distance2D(me) + me.NetworkPosition);
                        Utils.Sleep((int)Game.Ping + 50, "Qskill");
                    }
                    else
                        stage = 2;
                }
                else if (stage == 2 && stunned != null && Utils.SleepCheck("Eskill"))
                {
                    stone_nearly = stone.Where(x => stunned.Distance2D(x) < stunned.Distance2D((me.NetworkPosition - stunned.NetworkPosition) * 400 / stunned.NetworkPosition.Distance2D(me.NetworkPosition) + stunned.NetworkPosition)).FirstOrDefault();
                    if (Eskill.CanBeCasted())
                    {
                        if (stone_nearly != null)
                        {
                            if (Game.Ping < 100)
                            {
                                Eskill.UseAbility(stone_nearly.NetworkPosition);
                                Utils.Sleep((int)Game.Ping + 100, "Eskill");
                            }
                            else
                            {
                                Eskill.UseAbility(stunned.Predict(-Game.Ping));
                                Utils.Sleep((int)Game.Ping + 100, "Eskill");
                            }
                        }
                    }
                    else
                        stage = 3;
                }
                else if ((Game.IsKeyDown(_hotkeys_config.Item("hotkeycombo1").GetValue<KeyBind>().Key)) && stage == 2 && Utils.SleepCheck("Eskill") && stunned == null)
                {
                    stone_nearly = stone.Where(x => x.NetworkPosition.Distance2D(Mouse_Position) < 200).FirstOrDefault();
                    if (Eskill.CanBeCasted() && stone_nearly != null)
                    {
                        Eskill.UseAbility(Mouse_Position);
                        Utils.Sleep( 100, "Eskill");
                    }
                    if (!Eskill.CanBeCasted())
                        stage = 3;
                }
                else if (stage == 3 && stunned != null && stunned.Modifiers.Any(x => x.Name == "modifier_earth_spirit_geomagnetic_grip_debuff") && Utils.SleepCheck("Wskill"))
                {
                    if (auto_atack_after_spell)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 1");
                        Utils.Sleep(Game.Ping + 50, "auto_attack_1");
                        auto_atack_after_spell = false;
                    }
                    if (auto_atack)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack 1");
                        Utils.Sleep(Game.Ping + 50, "auto_attack_1");
                        auto_atack = false;
                    }
                    if (Wskill.CanBeCasted() && Utils.SleepCheck("auto_attack_1"))
                    {
                        Wskill.UseAbility(stunned.NetworkPosition);
                        Utils.Sleep(Game.Ping + (Qskill.GetCastDelay(me, stunned, true, true) * 100), "Wskill");
                    }
                }
                else if (stage == 3 && stunned == null && Utils.SleepCheck("Wskill") && (Game.IsKeyDown(_hotkeys_config.Item("hotkeycombo1").GetValue<KeyBind>().Key)))
                {
                    if (auto_atack_after_spell)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 1");
                        Utils.Sleep(Game.Ping + 50, "auto_attack_1");
                        auto_atack_after_spell = false;
                    }
                    if (auto_atack)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack 1");
                        Utils.Sleep(Game.Ping+50, "auto_attack_1");
                        auto_atack = false;
                    }
                    if (Wskill.CanBeCasted() && Utils.SleepCheck("auto_attack_1"))
                    {
                        Wskill.UseAbility(Mouse_Position);
                        Utils.Sleep((int)Game.Ping + 100, "Wskill");
                    }
                }
                else if (!Wskill.CanBeCasted())
                {
                    stage = 0;
                    key_active = false;
                    stunned = null;
                    if (auto_atack_after_spell)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 1");
                        auto_atack_after_spell = false;
                    }
                    if (auto_atack)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack 1");
                        auto_atack = false;
                    }
                }
            }
            if (key_active_2 && !Game.IsChatOpen)
            {
                if ((Game.IsKeyDown(_hotkeys_config.Item("hotkeycombo2").GetValue<KeyBind>().Key) && !Game.IsChatOpen))
                {
                    stage_combo2 = 0;
                    key_active_2 = false;
                    if (auto_atack_after_spell)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 1");
                        auto_atack_after_spell = false;
                    }
                    if (auto_atack)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack 1");
                        auto_atack = false;
                    }
                }
                if (stage_combo2 == 0 || stone_peoples == null)
                {
                    stone_peoples = ObjectMgr.GetEntities<Hero>().Where(x => x.Team != me.Team && !x.IsIllusion && x.Modifiers.Any(xy => xy.Name == "modifier_earthspirit_petrify") && x.Distance2D(me) <= 1600);
                    Mouse_Position = Game.MousePosition;
                }
                if (stone_peoples.FirstOrDefault() != null && me.CanCast() && !me.IsChanneling())
                {
                    if (stage_combo2 == 0)
                    {
                        if (Game.GetConsoleVar("dota_player_units_auto_attack_after_spell").GetInt() == 1)
                        {
                            Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 0");
                            auto_atack_after_spell = true;
                        }
                        else
                            auto_atack_after_spell = false;
                        if (Game.GetConsoleVar("dota_player_units_auto_attack").GetInt() == 1)
                        {
                            Game.ExecuteCommand("dota_player_units_auto_attack 0");
                            auto_atack = true;
                        }
                        else
                            auto_atack = false;
                        Utils.Sleep(Game.Ping + 50, "auto_atack_change");
                        stage_combo2 = 1;
                    }
                    if (stage_combo2 == 1 && Utils.SleepCheck("EskillSleep") && Utils.SleepCheck("auto_atack_change"))
                    {
                        if (Eskill.CanBeCasted() && stone_peoples.FirstOrDefault().NetworkPosition.Distance2D(me) >= 180)
                        {
                            Eskill.UseAbility(stone_peoples.FirstOrDefault().NetworkPosition);
                            Utils.Sleep(Game.Ping, "EskillSleep");
                        }
                        else
                            stage_combo2 = 2;
                    }
                    else if (stage_combo2 == 2)
                    {
                        if (Wskill.CanBeCasted())
                        {
                            int DelayCombo = 0;
                            if (stone_peoples.FirstOrDefault().NetworkPosition.Distance2D(stone_peoples.FirstOrDefault().NetworkPosition / me.NetworkPosition * 100) < 200)
                                DelayCombo = 150 - (int)Game.Ping;
                            else
                                DelayCombo = 350 - (int)Game.Ping;
                            Wskill.UseAbility(Mouse_Position);
                            Utils.Sleep(Game.Ping + DelayCombo + (me.GetTurnTime(Mouse_Position) * 100), "PerfectDelay");
                        }
                        else
                            stage_combo2 = 3;
                    }
                    else if (stage_combo2 == 3 && Utils.SleepCheck("PerfectDelay"))
                    {
                        if (Qskill.CanBeCasted())
                            Qskill.UseAbility(Mouse_Position);
                        else
                        {
                            stage_combo2 = 0;
                            key_active_2 = false;
                            if (auto_atack_after_spell)
                            {
                                Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 1");
                                auto_atack_after_spell = false;
                            }
                            if (auto_atack)
                            {
                                auto_atack = false;
                                Game.ExecuteCommand("dota_player_units_auto_attack 1");
                            }
                        }
                    }
                }
                if (stone_peoples.FirstOrDefault() == null || !me.CanCast())
                {
                    stage_combo2 = 0;
                    key_active_2 = false;
                    if (auto_atack_after_spell)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 1");
                        auto_atack_after_spell = false;
                    }
                    if (auto_atack)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack 1");
                        auto_atack = false;
                    }
                }
            }
            if (key_active_4 && !Game.IsChatOpen)
            {
                if (stage_combo4 == 0 && (!Qskill.CanBeCasted() || !Wskill.CanBeCasted()))
                {
                    stage_combo4 = 0;
                    key_active_4 = false;
                    if (auto_atack_after_spell)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 1");
                        auto_atack_after_spell = false;
                    }
                    if (auto_atack)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack 1");
                        auto_atack = false;
                    }
                }
                if ((Game.IsKeyDown(_hotkeys_config.Item("hotkeycombo2").GetValue<KeyBind>().Key) && !Game.IsChatOpen))
                {
                    stage_combo4 = 0;
                    key_active_4 = false;
                    if (auto_atack_after_spell)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 1");
                        auto_atack_after_spell = false;
                    }
                    if (auto_atack)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack 1");
                        auto_atack = false;
                    }
                }
                if (stage_combo4 == 0)
                {
                    if (Game.GetConsoleVar("dota_player_units_auto_attack_after_spell").GetInt() == 1)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 0");
                        auto_atack_after_spell = true;
                    }
                    else
                        auto_atack_after_spell = false;
                    if (Game.GetConsoleVar("dota_player_units_auto_attack").GetInt() == 1)
                    {
                        Game.ExecuteCommand("dota_player_units_auto_attack 0");
                        auto_atack = true;
                    }
                    else
                        auto_atack = false;
                    Utils.Sleep(Game.Ping+50, "auto_atack_change");
                    stage_combo4 = 1;
                }
                else if (stage_combo4 == 1 && Utils.SleepCheck("auto_atack_change"))
                {
                    Mouse_Position = Game.MousePosition;
                    stone_for_combo = ObjectMgr.GetEntities<Unit>().Where(x => x.ClassID == ClassID.CDOTA_Unit_Earth_Spirit_Stone && Mouse_Position.Distance2D(x.NetworkPosition) < 200).FirstOrDefault();
                    Hero geomagnetic_target = ObjectMgr.GetEntities<Hero>().Where(x => x.Team != me.Team && x.IsVisible && !x.IsIllusion && x.NetworkPosition.Distance2D(me.NetworkPosition) <= 1600 && x.Modifiers.Any(y => y.Name == "modifier_earth_spirit_geomagnetic_grip_debuff")).FirstOrDefault();
                    if (DRemnant.CanBeCasted() && Utils.SleepCheck("DRemnant") && stone_for_combo == null && (Qskill.CanBeCasted() && Wskill.CanBeCasted() && Eskill.CanBeCasted()))
                    {
                        DRemnant.UseAbility(Mouse_Position);
                        Utils.Sleep((int)Game.Ping + 2000, "DRemnant");
                    }
                    if (Eskill.CanBeCasted() && Utils.SleepCheck("Eskill_3combo"))
                    {
                        Eskill.UseAbility(Mouse_Position);
                        Utils.Sleep(500, "Eskill_3combo");
                    }
                    if (!Eskill.CanBeCasted() && geomagnetic_target == null)
                    {
                        stone_for_combo = ObjectMgr.GetEntities<Unit>().Where(x => x.ClassID == ClassID.CDOTA_Unit_Earth_Spirit_Stone && me.Distance2D(x.NetworkPosition) < 400).FirstOrDefault();
                        Vector3 perfect_position = (me.NetworkPosition + Mouse_Position) * 200 / me.NetworkPosition.Distance2D(Mouse_Position) + me.NetworkPosition;
                        if (DRemnant.CanBeCasted() && Utils.SleepCheck("DRemnant") && stone_for_combo == null && (Qskill.CanBeCasted() && Wskill.CanBeCasted()) && me.Distance2D(perfect_position) <= 400)
                        {
                            DRemnant.UseAbility(me.NetworkPosition);
                            Utils.Sleep((int)Game.Ping + 2000, "DRemnant");
                        }
                        else
                            stage_combo4 = 2;
                    }
                    else if (!Eskill.CanBeCasted())
                    {
                        stage_combo4 = 2;
                    }
                }
                else if (stage_combo4 == 2)
                {
                    if (Wskill.CanBeCasted() && Utils.SleepCheck("Wskill_3combo"))
                    {
                        Wskill.UseAbility(Mouse_Position);
                        Utils.Sleep(500, "Wskill_3combo");
                    }
                    if (!Wskill.CanBeCasted())
                        stage_combo4 = 3;
                }
                else if (stage_combo4 == 3)
                {
                    if( boulder_slow != null)
                    {
                        Mouse_Position = Game.MousePosition;
                        Vector3 perfect_position = (boulder_slow.NetworkPosition - Mouse_Position) * 150 / boulder_slow.NetworkPosition.Distance2D(Mouse_Position) + boulder_slow.NetworkPosition;
                        if (Utils.SleepCheck("moving") && me.NetworkPosition.Distance2D(perfect_position) >= 70)
                        {
                            me.Move(perfect_position);
                            Utils.Sleep(500, "moving");
                        }
                        if (me.NetworkPosition.ToVector2().FindAngleBetween(Mouse_Position.ToVector2(), true) - me.NetworkPosition.ToVector2().FindAngleBetween(boulder_slow.NetworkPosition.ToVector2(), true) <= 0.1 && me.NetworkPosition.ToVector2().FindAngleBetween(Mouse_Position.ToVector2(), true) - me.NetworkPosition.ToVector2().FindAngleBetween(boulder_slow.NetworkPosition.ToVector2(), true) >= -0.1)
                        {
                            if (Qskill.CanBeCasted() && Utils.SleepCheck("Qskill_combo3"))
                            {
                                Qskill.UseAbility(boulder_slow);
                                Utils.Sleep(500, "Qskill_combo3");
                            }
                            if (!Qskill.CanBeCasted())
                            {
                                stage_combo4 = 0;
                                key_active_4 = false;
                                if (auto_atack_after_spell)
                                {
                                    Game.ExecuteCommand("dota_player_units_auto_attack_after_spell 1");
                                    auto_atack_after_spell = false;
                                }
                                if (auto_atack)
                                {
                                    Game.ExecuteCommand("dota_player_units_auto_attack 1");
                                    auto_atack = false;
                                }
                            }
                        }
                    }
                }
            }
            //if (key_active_3 && !Game.IsChatOpen)
            //{
            //    if (stun_target == null || stage_combo3 == 0)
            //    {
            //        Mouse_Position = Game.MousePosition;
            //        stun_target = ObjectMgr.GetEntities<Hero>().Where(x => x.Team != me.Team && !x.IsIllusion && x.IsAlive && x.Distance2D(me) <= 1600);
            //    }
            //    int distance = 0;
            //    int last_distance = 50000;
            //    Hero most_closer_mouse_position_hero;
            //    foreach (var v in stun_target)
            //    {
            //        distance = (int)v.NetworkPosition.Distance2D(Mouse_Position);
            //        if (distance < last_distance)
            //        {
            //            last_distance = distance;
            //            most_closer_mouse_position_hero = v;
            //        }
            //    } 

            //}
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
        static void Magnetize(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsWatchingGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;
            if (me.ClassID != ClassID.CDOTA_Unit_Hero_EarthSpirit)
                return;
            System.Collections.Generic.List<Hero> Magnetized = ObjectMgr.GetEntities<Hero>().Where(x => x.Modifiers.Any(y => y.Name == "modifier_earth_spirit_magnetize") && x.IsAlive && x.IsVisible).ToList();
            foreach (Hero v in Magnetized)
            {
                if(v != null)
                {
                    float time = v.Modifiers.Find(x => x.Name == "modifier_earth_spirit_magnetize").RemainingTime;
                    string count = time.ToString("0.0");;
                    Vector2 magnetized_position = HeroPositionOnScreen(v);
                    Drawing.DrawText(count, magnetized_position, new Vector2(30, 200), time < 1 ? Color.Red : Color.Azure, FontFlags.AntiAlias | FontFlags.Additive | FontFlags.DropShadow);
                }
            }
            if (key_active || key_active_2 || key_active_4)
            {
                Vector2 me_position = HeroPositionOnScreen(me);
                Drawing.DrawText("GO!", me_position, new Vector2(15, 200), Color.Magenta, FontFlags.AntiAlias | FontFlags.Additive | FontFlags.DropShadow | FontFlags.Outline);
                Utils.Sleep(1500, "Finishing Combo!.");
            }
            else if (!key_active && !key_active_2 && !key_active_4 && !(Game.IsKeyDown(_hotkeys_config.Item("hotkeycombo2").GetValue<KeyBind>().Key) && !Game.IsChatOpen) && Utils.SleepCheck("Finishing Combo!.2"))
            {
                Vector2 me_position = HeroPositionOnScreen(me);
                if (!Utils.SleepCheck("Finishing Combo!."))
                    Drawing.DrawText("Get Rekt", me_position, new Vector2(18, 200), Color.Gold, FontFlags.AntiAlias | FontFlags.Additive | FontFlags.DropShadow | FontFlags.Outline);
            }
            else if ((Game.IsKeyDown(_hotkeys_config.Item("hotkeycombo2").GetValue<KeyBind>().Key) && !Game.IsChatOpen) || !Utils.SleepCheck("Finishing Combo!.2"))
            {
                Vector2 me_position = HeroPositionOnScreen(me);
                if (Utils.SleepCheck("Finishing Combo!.2"))
                    Utils.Sleep(1500, "Finishing Combo!.2");
                if (!Utils.SleepCheck("Finishing Combo!."))
                    Drawing.DrawText("STOP", me_position, new Vector2(20, 200), Color.Crimson, FontFlags.AntiAlias | FontFlags.Additive | FontFlags.DropShadow);
            }
        }
        static Vector2 HeroPositionOnScreen(Hero x)
        {
            float scaleX = ((float)Drawing.Width / 1366);
            float scaleY = ((float)Drawing.Height / 768);
            Vector2 PicPosition;
            Vector2 Final;
            Drawing.WorldToScreen(x.Position, out PicPosition);
            PicPosition += new Vector2(-51 * scaleX, -22 * scaleY) + new Vector2((float)62.5 * scaleX, 14 * scaleY);
            Final = new Vector2(((PicPosition.X) + (27 * scaleX))+30, (PicPosition.Y - (0 * scaleY))) -90;
            return Final;
        }
    }
}
