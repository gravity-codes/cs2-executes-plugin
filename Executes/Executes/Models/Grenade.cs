using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Executes.Enums;
using Executes.Models.JsonConverters;
using System.Text.Json.Serialization;

namespace Executes.Models
{
    public class Grenade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CsTeam Team { get; set; }

        [JsonConverter(typeof(VectorJsonConverter))]
        public Vector Position { get; private set; }

        [JsonConverter(typeof(QAngleJsonConverter))]
        public QAngle Angle { get; private set; }

        [JsonConverter(typeof(VectorJsonConverter))]
        public Vector Velocity { get; private set; }

        [JsonConverter(typeof(VectorJsonConverter))]
        public Vector PlayerPosition { get; private set; }

        [JsonConverter(typeof(QAngleJsonConverter))]
        public QAngle PlayerAngle { get; private set; }

        public EGrenade Type { get; private set; }

        public DateTime ThrownTime { get; private set; }

        public float Delay { get; set; }

        public Grenade(int grenadeId, string grenadeName, CsTeam grenadeTeam, Vector nadePosition, QAngle nadeAngle, Vector nadeVelocity, Vector playerPosition, QAngle playerAngle, EGrenade grenadeType, DateTime thrownTime)
        {
            Id = grenadeId;
            Name = grenadeName;
            Team = grenadeTeam;
            Position = new Vector(nadePosition.X, nadePosition.Y, nadePosition.Z);
            Angle = new QAngle(nadeAngle.X, nadeAngle.Y, nadeAngle.Z);
            Velocity = new Vector(nadeVelocity.X, nadeVelocity.Y, nadeVelocity.Z);
            PlayerPosition = new Vector(playerPosition.X, playerPosition.Y, playerPosition.Z);
            PlayerAngle = new QAngle(playerAngle.X, playerAngle.Y, playerAngle.Z);
            Type = grenadeType;
            ThrownTime = thrownTime;
            Delay = 0;
        }

        public void Throw()
        {
            if (Type == EGrenade.Smoke)
            {
                SmokeProjectile.Create(Position, Angle, Velocity, Team);
            }
            else
            {
                var entity = Utilities.CreateEntityByName<CBaseCSGrenadeProjectile>(Type.GetDesignerName());

                if (entity == null)
                {
                    Console.WriteLine($"[GrenadeThrownData Fatal] Failed to create entity!");
                    return;
                }

                if (Type == EGrenade.Molotov)
                {
                    entity.SetModel("weapons/models/grenade/incendiary/weapon_incendiarygrenade.vmdl");
                }

                //if (Type == EGrenade.Incendiary)
                //{
                //    // have to set IsIncGrenade after InitializeSpawnFromWorld as it forces it to false
                //    entity.IsIncGrenade = true;
                //    entity.SetModel("weapons/models/grenade/incendiary/weapon_incendiarygrenade.vmdl");
                //}

                entity.Elasticity = 0.33f;
                entity.IsLive = false;
                entity.DmgRadius = 350.0f;
                entity.Damage = 99.0f;
                entity.InitialPosition.X = Position.X;
                entity.InitialPosition.Y = Position.Y;
                entity.InitialPosition.Z = Position.Z;
                entity.InitialVelocity.X = Velocity.X;
                entity.InitialVelocity.Y = Velocity.Y;
                entity.InitialVelocity.Z = Velocity.Z;
                entity.Teleport(Position, Angle, Velocity);
                entity.DispatchSpawn();
                entity.Globalname = "custom";
                entity.AcceptInput("InitializeSpawnFromWorld");
            }
        }

        public override string ToString()
        {
            return $"Position: {Position}{Environment.NewLine}Angle: {Angle}{Environment.NewLine}Player Position: {PlayerPosition}{Environment.NewLine}Player Angle: {PlayerAngle}{Environment.NewLine}Type: {Type}{Environment.NewLine}Delay: {Delay}";
        }
    }
}
