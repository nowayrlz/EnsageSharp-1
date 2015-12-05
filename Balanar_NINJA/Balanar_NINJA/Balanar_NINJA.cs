using System;
using Ensage;
using Ensage.Common;
using SharpDX;

namespace Balanar_NINJA
{
    class Balanar_NINJA
    {
        private static Hero me;
        private static SideMessage balanartimer;
        private static int tempo = 190, tempo_day = 190, tempo_night = 480;
        static void Main(string[] args)
        {
            Game.OnUpdate += Tick;
        }
        public static void Tick(EventArgs args)
        {
            var stage = 0;
            me = ObjectMgr.LocalHero;
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame)
                return;
            if (me == null)
                return;
            //day
            int tempo_day = 190;
            for (int i = 0; i == 15; i++)
            {
                if (Game.GameTime == tempo_day)
                    tempo = tempo_day;
                tempo_day += 480;
            }
            //night
            int tempo_night = 480;
            for (int i = 0; i == 15; i++)
            {
                if (Game.GameTime == tempo_night)
                    tempo = tempo_night;
                tempo_night += 480;
            }


            if ((((int)(Game.GameTime)) == tempo))
                {
                    if (stage == 0)
                    {
                        stage = 1;
                        balanartimer = new SideMessage("Use your ultimate!", new Vector2(200, 48));
                        balanartimer.AddElement(new Vector2(142, 06), new Vector2(72, 36), Drawing.GetTexture("materials/ensage_ui/spellicons/night_stalker_darkness.vmat_c"));
                        balanartimer.AddElement("USE ULT!", new Vector2(20, 10), new Vector2(30, 20), Color.Red, FontFlags.DropShadow);
                        balanartimer.CreateMessage();
                        Utils.Sleep(10000, "time");
                    }
                    else if (Utils.SleepCheck("time") && stage == 1)
                    {
                        stage = 0;
                    }
                }
        }
    }
}
