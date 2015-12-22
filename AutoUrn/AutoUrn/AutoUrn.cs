using System;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using System.Linq;

namespace AutoUrn
{
    class AutoUrn
    {
        private static Hero me;
        private static System.Collections.Generic.List<Ensage.Hero> target;
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
            target = ObjectMgr.GetEntities<Hero>().Where(x => x.Health <= 100 && x.Distance2D(me) <= 950 && !x.IsIllusion && x.IsAlive && x.Team != me.Team).ToList();
            if (me == null || target.FirstOrDefault() == null)
                return;
            urn_of_shadows = me.FindItem("item_urn_of_shadows");

            if (!me.IsChanneling() && target.FirstOrDefault() != null && urn_of_shadows != null && (!me.IsInvisible() || me.ClassID == ClassID.CDOTA_Unit_Hero_Riki))
            {
                if (me.CanCast() && urn_of_shadows.CanBeCasted() && Utils.SleepCheck("urn"))
                {
                    urn_of_shadows.UseAbility(target.FirstOrDefault());
                    Utils.Sleep(200, "urn");
                }
            }
        }
    }
}
