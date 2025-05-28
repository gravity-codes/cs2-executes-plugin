using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Executes.Enums;
using Executes.Models;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;
using TimerFlags = CounterStrikeSharp.API.Modules.Timers.TimerFlags;

namespace Executes.Managers
{
    public sealed class GrenadeManager : BaseManager
    {
        public GrenadeManager() { }

        public void SetupGrenades(Scenario scenario)
        {
            if (scenario.Grenades.Count == 0)
            {
                Console.WriteLine("[Executes] No grenades to setup.");
                return;
            }

            var freezeTimeDuration = 0;
            var freezeTime = ConVar.Find("mp_freezetime");
            if (freezeTime != null)
            {
                freezeTimeDuration = freezeTime.GetPrimitiveValue<int>();
            }
            else
            {
                Console.WriteLine("[Executes] mp_freezetime not found.");
            }

            var teams = new List<CsTeam>
            {
                CsTeam.Terrorist,
                CsTeam.CounterTerrorist
            };
            var nadesThrown = new Dictionary<CsTeam, int>
            {
                { CsTeam.Terrorist, 0 },
                { CsTeam.CounterTerrorist, 0 },
            };

            foreach (var team in teams)
            {
                foreach (var grenade in scenario.Grenades[team])
                {
                    var nadeThrowPercentage = new Random().Next(0, 100);

                    if (nadesThrown[team] >= Helpers.GetPlayerCount(team))
                    {
                        Console.WriteLine($"[Executes] Skipping \"{grenade.Name}\".");
                        continue;
                    }

                    new Timer(freezeTimeDuration + grenade.Delay, () => grenade.Throw(), TimerFlags.STOP_ON_MAPCHANGE);
                    nadesThrown[team] += 1;
                }
            }
        }
    }
}
