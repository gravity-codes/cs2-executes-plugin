using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Executes.Configs;
using Executes.Enums;
using Executes.Models;

namespace Executes
{
    public class Executes : BasePlugin, IPluginConfig<ExecutesConfig>
    {
        #region Plugin Info
        public override string ModuleName => "Executes";
        public override string ModuleAuthor => "bazooka";
        public override string ModuleDescription => "Executes plugin for CS2.";
        public override string ModuleVersion => "0.0.1";
        #endregion

        private bool inDevMode = false;
        private Grenade? lastGrenade;

        public ExecutesConfig Config { get; set; } = new ExecutesConfig();

        public override void Load(bool hotReload)
        {
            RegisterListener<Listeners.OnMapStart>(OnMapStart);
            RegisterListener<Listeners.OnEntitySpawned>(OnEntitySpawnedHandler);

            Console.WriteLine("Executes loaded.");

            AddCommandListener("jointeam", OnCommandJoinTeam);

            if (hotReload)
            {
                OnMapStart(Server.MapName);
            }
        }

        public override void Unload(bool hotReload)
        {
            RemoveListener("OnMapStart", OnMapStart);

            Console.WriteLine("Executes unloaded.");
        }

        public void OnConfigParsed(ExecutesConfig config)
        {
            Config = config;
        }

        private void OnMapStart(string map)
        {
            // Manually set time while testing to not count down
            Server.ExecuteCommand("mp_warmup_start");
            Server.ExecuteCommand("mp_warmuptime 120");
            Server.ExecuteCommand("mp_warmup_pausetimer 1");
        }

        public void OnEntitySpawnedHandler(CEntityInstance entity)
        {
            if (!inDevMode) return;

            if (entity == null || entity.Entity == null) return;

            Server.NextFrame(() =>
            {
                CBaseCSGrenadeProjectile projectile = new CBaseCSGrenadeProjectile(entity.Handle);

                if (!projectile.IsValid ||
                    !projectile.Thrower.IsValid ||
                    projectile.Thrower.Value == null ||
                    projectile.Thrower.Value.Controller.Value == null ||
                    projectile.Globalname == "custom"
                ) return;

                CCSPlayerController player = new(projectile.Thrower.Value.Controller.Value.Handle);
                if (!player.IsValid || player.PlayerPawn.Value == null || !player.PlayerPawn.IsValid) return;
                int client = player.UserId!.Value;

                Vector position = new(projectile.AbsOrigin!.X, projectile.AbsOrigin.Y, projectile.AbsOrigin.Z);
                QAngle angle = new(projectile.AbsRotation!.X, projectile.AbsRotation.Y, projectile.AbsRotation.Z);
                Vector velocity = new(projectile.AbsVelocity.X, projectile.AbsVelocity.Y, projectile.AbsVelocity.Z);
                EGrenade nadeType = (EGrenade)entity.Entity.DesignerName.DesignerNameToEnum();

                lastGrenade = new Grenade(
                    0,
                    "last_grenade",
                    player.Team,
                    position,
                    angle,
                    velocity,
                    player.PlayerPawn.Value.CBodyComponent!.SceneNode!.AbsOrigin,
                    player.PlayerPawn.Value.EyeAngles,
                    nadeType,
                    DateTime.Now
                );

                player.ChatMessage(lastGrenade.ToString());
            });
        }

        private HookResult OnCommandJoinTeam(CCSPlayerController? player, CommandInfo commandInfo)
        {
            if (
                !Helpers.IsValidPlayer(player)
                || commandInfo.ArgCount < 2
                || !Enum.TryParse<CsTeam>(commandInfo.GetArg(1), out var toTeam)
            )
            {
                return HookResult.Handled;
            }

            return HookResult.Continue;
        }

        [ConsoleCommand("css_dev", "Allows the editing of spawns and grenades.")]
        public void OnToggleDevCommand(CCSPlayerController? player, CommandInfo commandInfo)
        {
            inDevMode = !inDevMode;

            player?.ChatMessage($"Dev mode is now {inDevMode}");
        }

        [ConsoleCommand("css_rethrow", "Rethrows the last grenade thrown.")]
        public void OnRethrowCommand(CCSPlayerController? player, CommandInfo commandInfo)
        {
            if (!inDevMode || lastGrenade == null || player == null) return;

            AddTimer(lastGrenade.Delay, () => lastGrenade.Throw());
        }
    }
}
