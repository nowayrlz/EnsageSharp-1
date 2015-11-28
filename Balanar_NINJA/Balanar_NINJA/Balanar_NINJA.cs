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
        private static int tempo = 190;
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
            if ((int)Game.GameTime == 190)
                tempo = 190;
            else if ((int)Game.GameTime == 670)
                tempo = 670;
            else if ((int)Game.GameTime == 1150)
                tempo = 1150;
            else if ((int)Game.GameTime == 1630)
                tempo = 1630;
            else if ((int)Game.GameTime == 2110)
                tempo = 2110;
            else if ((int)Game.GameTime == 2590)
                tempo = 2590;
            else if ((int)Game.GameTime == 3070)
                tempo = 3070;
            //night
            if ((int)Game.GameTime == 480)
                tempo = 480;
            else if ((int)Game.GameTime == 960)
                tempo = 960;
            else if ((int)Game.GameTime == 1440)
                tempo = 1440;
            else if ((int)Game.GameTime == 1920)
                tempo = 1920;
            else if ((int)Game.GameTime == 2400)
                tempo = 2400;
            else if ((int)Game.GameTime == 2880)
                tempo = 2880;
            else if ((int)Game.GameTime == 3360)
                tempo = 3360;

            if ((((int)(Game.GameTime)) == tempo))
                {
                    if (stage == 0)
                    {
                        stage = 1;
                        balanartimer = new SideMessage("Use your ultimate!", new Vector2(200, 48));
                        balanartimer.AddElement(new Vector2(142, 06), new Vector2(72, 36), Drawing.GetTexture("materials/ensage_ui/spellicons/night_stalker_darkness.vmat_c"));
                        balanartimer.AddElement("USE ULT!", new Vector2(20, 10), new Vector2(30, 20), Color.Red, FontFlags.DropShadow);
                        balanartimer.CreateMessage();
                        Utils.Sleep(190000, "time");
                        Utils.Sleep(2000, "time");
                        tempo += 480;
                        return;
                    }
                    else if (Utils.SleepCheck("time") && stage == 1)
                    {
                        balanartimer = new SideMessage("Use your ultimate!", new Vector2(200, 48));
                        balanartimer.AddElement(new Vector2(142, 06), new Vector2(72, 36), Drawing.GetTexture("materials/ensage_ui/spellicons/night_stalker_darkness.vmat_c"));
                        balanartimer.AddElement("USE ULT!", new Vector2(20, 10), new Vector2(30, 20), Color.Red, FontFlags.DropShadow);
                        balanartimer.CreateMessage();
                        stage = 0;
                        tempo += 480;
                    }
                }
        }
    }
}
