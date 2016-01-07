using System;
using Ensage;
using SharpDX;
using System.Collections.Generic;
using System.Linq;


namespace EXP_RANGE_NINJA
{
    class EXP_RANGE
    {
        private static Hero me;
        private static int _range_exp = 1300;
        private static bool chave = true;
        private static ParticleEffect[] rangedisplay_meepo = new ParticleEffect[5];
        private static ParticleEffect rangedisplay;
        static void Main(string[] args)
        {
            Game.OnUpdate += Tick;
            Console.WriteLine("> Range Script Loaded!");
        }
        public static void Tick(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsWatchingGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;
            if (me.ClassID == ClassID.CDOTA_Unit_Hero_Meepo)
            {
                List<Hero> meepo = ObjectMgr.GetEntities<Hero>().Where(x => x.Team == me.Team && x.Name == me.Name).ToList();
                uint i = 0;
                foreach(Hero m in meepo)
                {
                    i++;
                    if (m.IsAlive)
                    {
                        if (rangedisplay_meepo[i] == null)
                            rangedisplay_meepo[i] = m.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        if (rangedisplay_meepo[i].GetHighestControlPoint() != 2)
                        {
                            rangedisplay_meepo[i] = m.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                            rangedisplay_meepo[i].SetControlPoint(1, new Vector3(255, 255, 0));
                            rangedisplay_meepo[i].SetControlPoint(2, new Vector3(_range_exp, 255    , 0));
                        }
                    }
                    else
                        rangedisplay_meepo[i].Dispose();
                }
            }
            else
            {
                if (me.IsAlive)
                {
                    if (rangedisplay == null)
                        rangedisplay = me.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                    if (rangedisplay.GetHighestControlPoint() != 2)
                    {
                        rangedisplay = me.AddParticleEffect(@"particles\ui_mouseactions\drag_selected_ring.vpcf");
                        rangedisplay.SetControlPoint(1, new Vector3(255, 255, 0));
                        rangedisplay.SetControlPoint(2, new Vector3(_range_exp, 255, 0));
                    }
                }
                else
                    rangedisplay.Dispose();
            }
        }
    }
}
