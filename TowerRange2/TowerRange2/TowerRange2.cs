using System;
using Ensage.Common.Menu;
using System.Collections.Generic;
using System.Linq;
using Ensage;
using SharpDX;
namespace TowerRange2
{
    internal class Program
    {
        private static readonly Menu Menu = new Menu("Tower range 2", "Tower Range 2", true);
        private static Hero me;

        private static readonly List<ParticleEffect> Effects = new List<ParticleEffect>(); // keep references Ty zynox

        private static void Main()
        {
            Menu.AddItem(new MenuItem("OwnTowers", "My Towers").SetValue(true).SetTooltip("Show your tower range."));
            Menu.AddItem(new MenuItem("EnemyTowers", "Enemies Towers").SetValue(true).SetTooltip("Show the enemies towers range."));
            Menu.AddToMainMenu();
            Game.OnFireEvent += Game_OnFireEvent;
            GetRange();
            Console.WriteLine("> Tower 2 Script Loaded!");
        }
        private static void Game_OnFireEvent(FireEventEventArgs args)
        {
            if (args.GameEvent.Name == "dota_game_state_change")
            {
                var state = (GameState)args.GameEvent.GetInt("new_state");
                if (state == GameState.Started || state == GameState.Prestart)
                    GetRange();
            }
        }

        public static void GetRange()
        {
            me = ObjectMgr.LocalHero;
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame || me == null)
                return; 
            foreach (var e in Effects)
                e.Dispose();
            Effects.Clear();
            var towers =
                ObjectMgr.GetEntities<Building>()
                    .Where(x => x.IsAlive && x.ClassID == ClassID.CDOTA_BaseNPC_Tower)
                    .ToList();
            var Ancient =
                ObjectMgr.GetEntities<Building>()
                    .Where(x => x.IsAlive && x.ClassID == ClassID.CDOTA_BaseNPC_Fort)
                    .ToList();
            var Fountains =
                ObjectMgr.GetEntities<Entity>()
                    .Where(x => x.IsAlive && x.ClassID == ClassID.CDOTA_Unit_Fountain)
                    .ToList();
            if (!towers.Any() && !Ancient.Any())
                return;
            if (Menu.Item("EnemyTowers").GetValue<bool>())
            {
                foreach (var effect in towers.Where(x => x.Team != me.Team).Select(tower => tower.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf")))
                {
                    effect.SetControlPoint(1, new Vector3(850, 0, 0));
                    Effects.Add(effect);
                }
                foreach (var effect in towers.Where(x => x.Team != me.Team).Select(tower => tower.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf")))
                {
                    effect.SetControlPoint(1, new Vector3(900, 0, 0));
                    Effects.Add(effect);
                }
                foreach (var effect in Ancient.Where(x => x.Team != me.Team).Select(Ancients => Ancients.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf")))
                {
                    effect.SetControlPoint(1, new Vector3(900, 0, 0));
                    Effects.Add(effect);
                }
                foreach (var effect in Fountains.Where(x => x.Team != me.Team).Select(Fountain => Fountain.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf")))
                {
                    effect.SetControlPoint(1, new Vector3(1200, 0, 0));
                    Effects.Add(effect);
                }
            }
            if (Menu.Item("OwnTowers").GetValue<bool>())
            {
                foreach (var effect in towers.Where(x => x.Team == me.Team).Select(tower => tower.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf")))
                {
                    effect.SetControlPoint(1, new Vector3(850, 0, 0));
                    Effects.Add(effect);
                }
                foreach (var effect in towers.Where(x => x.Team == me.Team).Select(tower => tower.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf")))
                {
                    effect.SetControlPoint(1, new Vector3(900, 0, 0));
                    Effects.Add(effect);
                }
                foreach (var effect in Ancient.Where(x => x.Team == me.Team).Select(Ancients => Ancients.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf")))
                {
                    effect.SetControlPoint(1, new Vector3(900, 0, 0));
                    Effects.Add(effect);
                }
                foreach (var effect in Fountains.Where(x => x.Team == me.Team).Select(Fountain => Fountain.AddParticleEffect(@"particles\ui_mouseactions\range_display.vpcf")))
                {
                    effect.SetControlPoint(1, new Vector3(1200, 0, 0));
                    Effects.Add(effect);
                }
            }
        }

    }
}
