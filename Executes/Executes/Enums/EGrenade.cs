namespace Executes.Enums
{
    public enum EGrenade
    {
        Flashbang,
        HighExplosive,
        Smoke,
        Molotov,
        Incendiary,
        Decoy
    }

    public static class EGrenadeExtension
    {
        public static string? GetDesignerName(this EGrenade grenade)
        {
            return grenade switch
            {
                EGrenade.Flashbang => "weapon_flashbang",
                EGrenade.HighExplosive => "weapon_hegrenade",
                EGrenade.Smoke => "weapon_smokegrenade",
                EGrenade.Molotov => "weapon_molotov",
                EGrenade.Incendiary => "weapon_incgrenade",
                EGrenade.Decoy => "weapon_decoy",
                _ => null,
            };
        }

        public static string? GetProjectileName(this EGrenade grenade)
        {
            return grenade switch
            {
                EGrenade.Flashbang => "flashbang_projectile",
                EGrenade.HighExplosive => "hegrenade_projectile",
                EGrenade.Smoke => "smokegrenade_projectile",
                EGrenade.Molotov => "molotov_projectile",
                EGrenade.Incendiary => "molotov_projectile",
                EGrenade.Decoy => "decoy_projectile",
                _ => null,
            };
        }

        public static EGrenade? ToEnum(this string grenade)
        {
            return grenade switch
            {
                "weapon_flashbang" => EGrenade.Flashbang,
                "weapon_hegrenade" => EGrenade.HighExplosive,
                "weapon_smokegrenade" => EGrenade.Smoke,
                "weapon_molotov" => EGrenade.Molotov,
                "weapon_incgrenade" => EGrenade.Incendiary,
                "weapon_decoy" => EGrenade.Decoy,
                _ => null
            };
        }

        public static EGrenade? DesignerNameToEnum(this string designerName)
        {
            return designerName switch
            {
                "smokegrenade_projectile" => EGrenade.Smoke,
                "flashbang_projectile" => EGrenade.Flashbang,
                "hegrenade_projectile" => EGrenade.HighExplosive,
                "decoy_projectile" => EGrenade.Decoy,
                "molotov_projectile" => EGrenade.Molotov,
                _ => null
            };
        }
    }
}
