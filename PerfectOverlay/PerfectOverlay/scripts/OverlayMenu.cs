using System.Collections.Generic;
using System.Linq;
using Ensage;
using Ensage.Common;
using System;
using SharpDX;


namespace PerfectOverlay.scripts
{
    class OverlayMenu
    {
        private static Hero me;

        private static float boxSizeX;

        private static IEnumerable<Ability> Herospells;

        private static IEnumerable<Item> DangerItems;

        private static List<Hero> heroes;

        private static float boxSizeY;

        static float ScaleX, ScaleY;

        private static float i, i2, i3;

        public static void start()
        {
            me = ObjectMgr.LocalHero;
            if (me == null && !Game.IsInGame) return;
            GetHPbarSize();
            ScaleX = (float)Drawing.Width / 1400;
            ScaleY = (float)Drawing.Height / 900;
            heroes = ObjectMgr.GetEntities<Hero>().Where(x => x.IsAlive && !x.IsIllusion && x.IsVisible && x.IsValid).ToList();
            foreach (Hero x in heroes)
            {
                if (x == null) continue;
                if (x == me) continue;
                Herospells = x.Spellbook.Spells;
                DangerItems = x.Inventory.Items;
                i = 0;
                i2 = 0;
                i3 = 0;
                try
                {
                    if (x.Mana > 0 && x.Team != me.Team && !x.UnitState.HasFlag(UnitState.NoHealthbar))
                    {
                        Drawing.DrawRect(new Vector2((float)(HUDInfo.GetHPbarPosition(x).X - (0.5 * ScaleX)), HUDInfo.GetHPbarPosition(x).Y + (9 * ScaleY)), new Vector2((boxSizeX * 6) + (2 * ScaleX), boxSizeY - (13 * ScaleY)), new Color(0x00, 0x80, 0xF8, 0x32), false);
                        Drawing.DrawRect(new Vector2((float)(HUDInfo.GetHPbarPosition(x).X - (0.5 * ScaleX)), HUDInfo.GetHPbarPosition(x).Y + (9 * ScaleY)), new Vector2((boxSizeX * 6) + (2 * ScaleX), boxSizeY - (13 * ScaleY)), Color.Black, true);
                        Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + (1 * ScaleX), HUDInfo.GetHPbarPosition(x).Y + (10 * ScaleY)), new Vector2(((boxSizeX * 6) / (x.MaximumMana / x.Mana)) - (1 * ScaleX), boxSizeY - (15 * ScaleY)), new Color(0x00, 0x80, 0xF8, 0xFF), false);
                    }
                    foreach (Ability y in Herospells)
                    {
                        if (y != null /*&& y.Name != "attribute_bonus"*/ && !y.Name.Contains("empty") && (x.Name.Contains("rubick") ? !y.Name.Contains("hidden") : true) && !x.UnitState.HasFlag(UnitState.NoHealthbar))
                        {
                            //if (!Game.IsPaused)
                            if (y.AbilitySlot == AbilitySlot.Slot_1ULL || y.AbilitySlot == AbilitySlot.Slot_2 || y.AbilitySlot == AbilitySlot.Slot_3 || y.AbilitySlot == AbilitySlot.Slot_4 || y.AbilitySlot == AbilitySlot.Slot_5 || y.AbilitySlot == AbilitySlot.Slot_6)
                            {
                                Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + ((i * 20) *ScaleX), HUDInfo.GetHPbarPosition(x).Y - (25 * ScaleY)), new Vector2(boxSizeX, boxSizeY), Drawing.GetTexture("materials/ensage_ui/spellicons/" + y.Name + ".vmat"));
                                Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + ((i * 20) *ScaleX), HUDInfo.GetHPbarPosition(x).Y - (25 * ScaleY)), new Vector2(boxSizeX, boxSizeY), Color.Black, true);
                                if (y.Cooldown > 0 || (y.ManaCost > x.Mana && y.ManaCost > 0))
                                {
                                    Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + ((i * 20) * ScaleX), HUDInfo.GetHPbarPosition(x).Y - (25 * ScaleY)), new Vector2(boxSizeX, boxSizeY), new Color(0x5F, 0x00, 0xCE, 0x9E), false);
                                    Drawing.DrawText((int)(y.Cooldown == 0 ? (y.ManaCost - x.Mana) : y.Cooldown)+ "", new Vector2((float)(HUDInfo.GetHPbarPosition(x).X + (0.8 + (i * 20) * ScaleX)), HUDInfo.GetHPbarPosition(x).Y - (17 * ScaleY)), new Vector2(10 * ScaleX, 20 * ScaleY), y.Cooldown == 0 ? Color.LightSkyBlue : Color.White, FontFlags.AntiAlias | FontFlags.StrikeOut | FontFlags.Additive);
                                }
                                if (y.Level <= 0)
                                    Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + ((i * 20) * ScaleX), HUDInfo.GetHPbarPosition(x).Y - (25 * ScaleY)), new Vector2(boxSizeX, boxSizeY), new Color(0x5F, 0x00, 0x00, 0x9E), false);
                                else
                                {
                                    Drawing.DrawRect(new Vector2((HUDInfo.GetHPbarPosition(x).X + ((i * 20) * ScaleX)), HUDInfo.GetHPbarPosition(x).Y - (25 * ScaleY)), new Vector2(boxSizeX - ((y.Level < 10 ? 8 : 5) * ScaleX), boxSizeY - (9 * ScaleY)), Color.Black, false);
                                    Drawing.DrawText((int)y.Level + "", new Vector2((float)(HUDInfo.GetHPbarPosition(x).X + (((y.Level < 10 ? 0.8 : 0) + (i * 20)) * ScaleX)), HUDInfo.GetHPbarPosition(x).Y - (25 * ScaleY)), new Vector2(10 * ScaleX, 20 * ScaleY), Color.YellowGreen, FontFlags.AntiAlias | FontFlags.StrikeOut | FontFlags.Additive);
                                }
                                i += 1 * ScaleX;
                            }
                            else
                            {
                                if (y.Cooldown != 0 || y.ManaCost > x.Mana)
                                {
                                    Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + ((i2 * 20) * ScaleX), HUDInfo.GetHPbarPosition(x).Y - (50 * ScaleY)), new Vector2(boxSizeX, boxSizeY), Drawing.GetTexture("materials/ensage_ui/spellicons/" + y.Name + ".vmat"));
                                    Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + ((i2 * 20) * ScaleX), HUDInfo.GetHPbarPosition(x).Y - (50 * ScaleY)), new Vector2(boxSizeX, boxSizeY), Color.Black, true);
                                    if (y.Cooldown > 0 || (y.ManaCost > x.Mana && y.ManaCost > 0))
                                    {
                                        Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + ((i2 * 20) * ScaleX), HUDInfo.GetHPbarPosition(x).Y - (50 * ScaleY)), new Vector2(boxSizeX, boxSizeY), new Color(0x5F, 0x00, 0xCE, 0x9E), false);
                                        Drawing.DrawText((int)(y.Cooldown == 0 ? (y.ManaCost - x.Mana) : y.Cooldown) + "", new Vector2((float)(HUDInfo.GetHPbarPosition(x).X + ((0.8 + (i2 * 20)) * ScaleX)), HUDInfo.GetHPbarPosition(x).Y - (42 * ScaleY)), new Vector2(10 * ScaleX, 20 * ScaleY), y.Cooldown == 0 ? Color.LightSkyBlue : Color.White, FontFlags.AntiAlias | FontFlags.StrikeOut | FontFlags.Additive);
                                    }
                                    //if (y.Level <= 0) // BETA VERSION NINJAMAN
                                    //    Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + (i2 * 20), HUDInfo.GetHPbarPosition(x).Y - 50), new Vector2(boxSizeX, boxSizeY), new Color(0x5F, 0x00, 0x00, 0x9E), false);
                                    //else
                                    //    Drawing.DrawText((int)y.Level + "", new Vector2((float)(HUDInfo.GetHPbarPosition(x).X + 1.2 + (i2 * 20)), HUDInfo.GetHPbarPosition(x).Y - 50), new Vector2(10, 20), Color.LimeGreen, FontFlags.AntiAlias | FontFlags.StrikeOut | FontFlags.Additive);
                                    i2 += 1 * ScaleX;
                                }
                            }
                        }
                    }
                    foreach (Item z in DangerItems)
                    {
                        if (z != null && !x.UnitState.HasFlag(UnitState.NoHealthbar) && (z.Name == "item_gem" || z.Name == "item_dust" || z.Name == "item_sphere" || z.Name == "item_blink" || z.Name == "item_ward_observer" || z.Name == "item_ward_sentry" || z.Name == "item_black_king_bar" || z.Name == "item_ward_dispenser" || z.Name == "item_sheepstick" || z.Name == "item_blade_mail" || z.Name == "item_rapier" || z.Name == "item_cyclone" || z.Name == "item_shadow_amulet" || z.Name == "item_invis_sword" || z.Name == "item_silver_edge" || z.Name == "item_glimmer_cape" || z.Name == "item_lotus_orb" || z.Name == "item_orchid" || z.Name.Contains("item_dagon") || z.Name == "item_manta" || z.Name == "item_aegis" || z.Name == "item_cheese"))
                        {
                            Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + ((i3 * 20) * ScaleX), HUDInfo.GetHPbarPosition(x).Y + (15 * ScaleY)), new Vector2(boxSizeX + (6 * ScaleX), boxSizeY), Drawing.GetTexture("materials/ensage_ui/items/" + z.Name.Remove(0, 5)));
                            Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + ((i3 * 20) * ScaleX), HUDInfo.GetHPbarPosition(x).Y + (15 * ScaleY)), new Vector2(boxSizeX, boxSizeY), Color.Black, true);
                            if (z.Cooldown > 0 || (z.ManaCost > x.Mana && z.ManaCost > 0))
                            {
                                Drawing.DrawRect(new Vector2(HUDInfo.GetHPbarPosition(x).X + ((i3 * 20) * ScaleX), HUDInfo.GetHPbarPosition(x).Y + (15 * ScaleY)), new Vector2(boxSizeX, boxSizeY), new Color(0x5F, 0x00, 0xCE, 0x9E), false);
                                Drawing.DrawText((int)(z.Cooldown == 0 ? (z.ManaCost - x.Mana) : z.Cooldown) + "", new Vector2((float)(HUDInfo.GetHPbarPosition(x).X + ((1.2 + (i3 * 20)) * ScaleX)), HUDInfo.GetHPbarPosition(x).Y + (23 * ScaleY)), new Vector2(10 * ScaleX, 20 * ScaleY), z.Cooldown == 0 ? Color.LightSkyBlue : Color.White, FontFlags.AntiAlias | FontFlags.StrikeOut | FontFlags.Additive);
                            }
                            i3 += 1 * ScaleX;
                        }
                    }
                }
                catch(EntityNotFoundException)
                {

                }
            }
        }
        private static void GetHPbarSize()
        {
            boxSizeX = (HUDInfo.GetHPBarSizeX() / 6);
            boxSizeY = (HUDInfo.GetHpBarSizeY() + 7);
        }
    }
}
