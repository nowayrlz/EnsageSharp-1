using System;
using Ensage;
using SharpDX;

namespace EXP_RANGE_NINJA
{
    class EXP_RANGE
    {
        private static Hero me;
        private static int _range_exp = 1300;
        private static bool chave = true;
        private static ParticleEffect rangedisplay;
        static void Main(string[] args)
        {
            Game.OnFireEvent += Tick;
            Console.WriteLine("> Runa Script Loaded!");
        }
        public static void Tick(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;
            if(rangedisplay == null)
                rangedisplay = me.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf");
            if (me.IsAlive && chave)
            {
                rangedisplay.SetControlPoint(1, new Vector3(_range_exp, 0, 0));
                chave = false;
            }
            else if(!me.IsAlive)
            {
                rangedisplay.SetControlPoint(0, new Vector3(_range_exp, 0, 0));
                chave = true;
            }
        }
    }
}
