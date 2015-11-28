using System;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;

namespace AutoUrn
{
    class AutoUrn
    {
        private static Hero me, target;
        private static Item urn_of_shadows;

        static void Main(string[] args)
        {
            Game.OnUpdate += Tick;
            Console.WriteLine("> Auto Urn Loaded!");
        }
        public static void Tick(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame)
                return;
            me = ObjectMgr.LocalHero;
            target = me.ClosestToMouseTarget(1000);
            if (me == null || target == null)
                return;
            urn_of_shadows = me.FindItem("item_urn_of_shadows");

            if (!me.IsChanneling() && target != null)
            {
                if (me.CanCast() && urn_of_shadows.CanBeCasted() && target.Health <= 150 && target.Distance2D(me) <= 950 && !target.IsIllusion && target.IsAlive && Utils.SleepCheck("urn") && urn_of_shadows.CurrentCharges > 0)
                {
                    urn_of_shadows.UseAbility(target);
                    Utils.Sleep(200, "urn");
                }
            }
        }
    }
}
