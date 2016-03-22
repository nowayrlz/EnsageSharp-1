using System;
using System.Collections.Generic;
using System.Linq;
using Ensage;

namespace PerfectOverlay.scripts
{
    class Showillusions
    {
        private static List<Hero> heroes;
        private static Hero me;
        private static ParticleEffect particleEffect;
        private static readonly Dictionary<string, ParticleEffect> ParticleDictionary =
            new Dictionary<string, ParticleEffect>();
        private static uint oldhandle;
        public static void illusionstart()
        {
            me = ObjectMgr.LocalHero;
            if (me == null && !Game.IsInGame) return;
            try
            {
                List<Hero> heroes = ObjectMgr.GetEntities<Hero>().Where(x => x.IsIllusion && x.Team != me.Team && x.IsAlive && x.IsValid && x.IsVisible).ToList();
                if (heroes != null)
                {
                    foreach (Hero x in heroes)
                    {
                        try
                        {
                            if (x != null)
                            {
                                if (particleEffect == null || oldhandle != x.Handle)
                                {
                                    particleEffect = x.AddParticleEffect(@"particles/items2_fx/phase_boots.vpcf");
                                    oldhandle = x.Handle;
                                }
                                ParticleDictionary.Add(x.Handle.ToString(), particleEffect);
                            }
                            else
                            {
                                if (particleEffect == null) continue;
                                //if (ParticleDictionary.TryGetValue(x.Handle.ToString(), out particleEffect))
                                particleEffect.Dispose();
                                ParticleDictionary.Remove(x.Handle.ToString());
                            }
                        }
                        catch (SystemException)
                        {
                        }
                    }
                }
            }
            catch (EntityNotFoundException)
            {
            }
        }
    }
}
