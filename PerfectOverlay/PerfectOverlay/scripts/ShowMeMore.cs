//using System;
using System.Collections.Generic;
using System.Linq;
using Ensage;
using SharpDX;

namespace PerfectOverlay.scripts
{
    class ShowMeMore
    {
        static readonly Dictionary<Unit, ParticleEffect> Effects = new Dictionary<Unit, ParticleEffect>();
        public static void init(Unit Sender, ModifierChangedEventArgs args)
        {
            var Modifier = args.Modifier;
            ParticleEffect Display = null;
            switch (Modifier.Name)
            {
                // NEW MODIFIERS NINJAMAN
                // ARC wARDEN
                case "modifier_arc_warden_spark_wraith_thinker":
                    if (!Effects.TryGetValue(Sender, out Display))
                    {
                        Display = Sender.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        Display.SetControlPoint(2, new Vector3(375 + 50, 255, 0));
                        Display.SetControlPoint(1, new Vector3(255, 0, 0));
                        Effects.Add(Sender, Display);
                    }
                    break;
                case "modifier_arc_warden_magnetic_field_thinker":
                    if (!Effects.TryGetValue(Sender, out Display))
                    {
                        Display = Sender.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        Display.SetControlPoint(2, new Vector3(275 + 50, 255, 0));
                        Display.SetControlPoint(1, new Vector3(255, 0, 0));
                        Effects.Add(Sender, Display);
                    }
                    break;
                // AXE
                //ALCH
                case "modifier_alchemist_acid_spray_thinker":
                    if (!Effects.TryGetValue(Sender, out Display))
                    {
                        Display = Sender.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        Display.SetControlPoint(2, new Vector3(625 + 50, 255, 0));
                        Display.SetControlPoint(1, new Vector3(255, 0, 0));
                        Effects.Add(Sender, Display);
                    }
                    break;
                //CLOCKWERK
                case "modifier_rattletrap_rocket_flare":
                    if (!Effects.TryGetValue(Sender, out Display))
                    {
                        Display = Sender.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        Display.SetControlPoint(2, new Vector3(575 + 50, 255, 0));
                        Display.SetControlPoint(1, new Vector3(255, 0, 0));
                        Effects.Add(Sender, Display);
                    }
                    break;
                // ENIGMA
                case "modifier_enigma_black_hole_thinker":
                    if (!Effects.TryGetValue(Sender, out Display))
                    {
                        Display = Sender.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        Display.SetControlPoint(2, new Vector3(420 + 50, 255, 0));
                        Display.SetControlPoint(1, new Vector3(255, 0, 0));
                        Effects.Add(Sender, Display);
                    }
                    break;
                // NOVA MODIFIERS
                case "modifier_lina_light_strike_array":
                    if (!Effects.TryGetValue(Sender, out Display))
                    {
                        Display = Sender.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        Display.SetControlPoint(2, new Vector3(225 + 50, 255, 0));
                        Display.SetControlPoint(1, new Vector3(255, 0, 0));
                        Effects.Add(Sender, Display);
                    }
                    break;
                case "modifier_kunkka_torrent_thinker":
                    if (!Effects.TryGetValue(Sender, out Display))
                    {
                        Display = Sender.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        Display.SetControlPoint(2, new Vector3(225 + 50, 255, 0));
                        Display.SetControlPoint(1, new Vector3(255, 0, 0));
                        Effects.Add(Sender, Display);
                    }
                    break;
                case "modifier_leshrac_split_earth_thinker":
                    if (!Effects.TryGetValue(Sender, out Display))
                    {
                        var lesh = ObjectMgr.GetEntities<Hero>()
                                .FirstOrDefault(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Leshrac);
                        Display = Sender.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        Display.SetControlPoint(2, new Vector3(lesh.Spellbook.SpellQ.AbilityData.FirstOrDefault(x => x.Name == "radius").GetValue(lesh.Spellbook.SpellQ.Level - 1) + 50, 255, 0));
                        Display.SetControlPoint(1, new Vector3(255, 0, 0));
                        Effects.Add(Sender, Display);
                    }
                    break;
                case "modifier_invoker_sun_strike":
                    if (!Effects.TryGetValue(Sender, out Display))
                    {
                        Display = Sender.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        Display.SetControlPoint(2, new Vector3(175 + 50, 255, 0));
                        Display.SetControlPoint(1, new Vector3(255, 0, 0));
                        Effects.Add(Sender, Display);
                    }
                    break;
            }
        }
    }
}
