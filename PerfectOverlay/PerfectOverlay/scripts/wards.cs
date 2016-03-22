using System.Collections.Generic;
using System.Linq;
using Ensage;
using SharpDX;


namespace PerfectOverlay.scripts
{
    class wards
    {
        private static Hero me;

        private static Dictionary<string, ParticleEffect> wardeffects = new Dictionary<string, ParticleEffect> { };

        private static Dictionary<string, float> wardtiming = new Dictionary<string, float> { };

        private static Dictionary<string, Vector3> getpos = new Dictionary<string, Vector3> { };

        private static Dictionary<string, string> getname = new Dictionary<string, string> { };

        private static int range = 0, range2 = 0;
        private static Vector3 color1, color2;
        private static Vector2 boxsize;
        private static string keyvalue;
        public static void start()
        {
            me = ObjectMgr.LocalHero;
            if (me == null || !Game.IsInGame || Game.IsPaused) return;
            var wards = ObjectMgr.GetEntities<Unit>().Where(x => (x.ClassID == ClassID.CDOTA_NPC_Observer_Ward || x.ClassID == ClassID.CDOTA_NPC_Observer_Ward_TrueSight || x.ClassID == ClassID.CDOTA_NPC_TechiesMines) && x.Team != me.Team);
            foreach (Unit x in wards)
            {
                if (x == null) continue;
                if (x.IsAlive)
                    keyvalue = (x.Modifiers.FirstOrDefault().Name + "" + x.Handle);
                if (!wardeffects.Keys.Any(y => y == keyvalue) && x.IsAlive && x.IsValid)
                {
                    wardeffects.Add(keyvalue, new ParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf", x.Position));
                    wardtiming.Add(keyvalue, x.Modifiers.FirstOrDefault().DieTime);
                    getpos.Add(keyvalue, x.Position);
                    getname.Add(keyvalue, x.Name);
                    if (x.Name == "npc_dota_observer_wards")
                    {
                        range = 1600;
                        range2 = 0;
                        color1 = new Vector3(255, 215, 0);
                    }
                    else if (x.Name == "npc_dota_sentry_wards")
                    {
                        range = 850;
                        range2 = 0;
                        color1 = new Vector3(30, 144, 255);
                    }
                    else if (x.Name == "npc_dota_techies_land_mine")
                    {
                        range = 250;
                        range2 = 550;
                        wardeffects.Add(keyvalue + "2", new ParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf", x.Position));
                        color1 = new Vector3(128, 0, 0);
                        color2 = new Vector3(111, 111, 111);
                    }
                    else if (x.Name == "npc_dota_techies_stasis_trap")
                    {
                        range = 500;
                        range2 = 0;
                        color1 = new Vector3(65, 105, 225);
                    }
                    else if (x.Modifiers.FirstOrDefault().Name == "modifier_techies_remote_mine")
                    {
                        range = 475;
                        range2 = 0;
                        color1 = new Vector3(139, 0, 0);
                    }
                    else
                    {
                        range = 0;
                        range2 = 0;
                    }
                    if (range > 0)
                    {
                        wardeffects.FirstOrDefault(y => y.Key == keyvalue).Value.SetControlPoint(1, color1);
                        wardeffects.FirstOrDefault(y => y.Key == keyvalue).Value.SetControlPoint(2, new Vector3(range, 255, 0));
                    }
                    if (range2 > 0)
                    {
                        wardeffects.FirstOrDefault(y => y.Key == keyvalue + "2").Value.SetControlPoint(1, color2);
                        wardeffects.FirstOrDefault(y => y.Key == keyvalue + "2").Value.SetControlPoint(2, new Vector3(range2, 255, 0));
                    }
                }
                if (wardeffects.Keys.Any(y => y.Contains("" + x.Handle + "")) && (x.IsAlive == false || x.Health == 0) && x.IsValid)
                {
                    if (wardeffects.Keys.Any(y => y.Contains("" + x.Handle + "2")))
                    {
                        wardeffects.FirstOrDefault(y => y.Key.Contains("" + x.Handle + "2")).Value.Dispose();
                        wardeffects.Remove(wardeffects.FirstOrDefault(y => y.Key.Contains("" + x.Handle + "2")).Key);

                    }
                    wardeffects.FirstOrDefault(y => y.Key.Contains("" + x.Handle + "")).Value.Dispose();
                    wardeffects.Remove(wardeffects.FirstOrDefault(y => y.Key.Contains("" + x.Handle + "")).Key);
                }
                if (wardtiming[keyvalue] <= Game.GameTime)
                {
                    wardtiming.Remove(keyvalue);
                    wardeffects[keyvalue].Dispose();
                    wardeffects.Remove(keyvalue);
                }
            }
            foreach(var x in wardeffects)
            {
                string texturename;
                if(x.Value.GetControlPoint(1) != null && getname.Keys.Any(y => y == x.Key))
                {
                    if (getname.FirstOrDefault(y => y.Key == x.Key).Value.Contains("observer"))
                    {
                        texturename = "materials/ensage_ui/items/ward_observer.vmat_c";
                        boxsize = new Vector2(41, 30);
                    }
                    else if (getname.FirstOrDefault(y => y.Key == x.Key).Value.Contains("sentry"))
                    {
                        texturename = "materials/ensage_ui/items/ward_sentry.vmat_c";
                        boxsize = new Vector2(41, 30);
                    }
                    else if (getname.FirstOrDefault(y => y.Key == x.Key).Value.Contains("land"))
                    {
                        texturename = "materials/ensage_ui/spellicons/techies_land_mines.vmat";
                        boxsize = new Vector2(30, 30);
                    }
                    else if (getname.FirstOrDefault(y => y.Key == x.Key).Value.Contains("stasis"))
                    {
                        texturename = "materials/ensage_ui/spellicons/techies_stasis_trap.vmat";
                        boxsize = new Vector2(30, 30);
                    }
                    else if (getname.FirstOrDefault(y => y.Key == x.Key).Value.Contains("remote"))
                    {
                        texturename = "materials/ensage_ui/spellicons/techies_remote_mines.vmat_c";
                        boxsize = new Vector2(30, 30);
                    }
                    else
                        texturename = null;
                    if (texturename != null)
                    {
                        Drawing.DrawRect(Drawing.WorldToScreen(getpos.FirstOrDefault(y => y.Key == x.Key).Value), boxsize, Drawing.GetTexture(texturename));
                        Drawing.DrawRect(Drawing.WorldToScreen(getpos.FirstOrDefault(y => y.Key == x.Key).Value), new Vector2(30,30), Color.Black, true);
                    }
                }
            }
        }
    }
}
