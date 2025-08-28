namespace CONSTANT
{
    public class CharactorAnimName
    {
        public static string Spawn = "Spawn";
        public static string Idle = "Idle";
        public static string Move = "Move";
        public static string Hit = "Hit";
        public static string Attack = "Attack";
        public static string Dead = "Dead";
        public static string CastSpell = "CastSpell";
        public static string Summon = "Summon";
        public static string Projectile = "Projectile";
        public static string Spin = "Spin";
        public static string Roll = "Roll";
    }
    public static class SceneName
    {
        public static string BootScene = "BootScene";
        public static string BufferScene = "BufferScene";
        public static string InGameScene = "InGameScene";
        
    }
    public static class TagName
    {
        public static string Player = "Player";
        public static string Enemy = "Enemy";
        public static string Bullet = "ShotGunBullet";
    }
    // MuzzleFlare + Impact + ShotGunBullet thì đặt tên hậu số tên súng theo vd : MuzzleFlare + ShortGunTurret
    public static class MuzzleFlareName
    {
        public static readonly string MuzzleFlareShortGunTurret = "MuzzleFlareShortGunTurret";
        public static readonly string MuzzleFlarePlasmaTurret = "MuzzleFlarePlasmaTurret";
        
    }

    public static class ProjectileName
    {
        public static readonly string ProjectileShortGunTurret = "ProjectileShortGunTurret";
        public static readonly string ProjectilePlasmaTurret = "ProjectilePlasmaTurret";
        public static readonly string ProjectileRocketTurret = "ProjectileRocketTurret";
    }

    public static class ImpactName
    {
        public static readonly string ImpactShortGunTurret = "ImpactShortGunTurret";
        public static string ImpactPlasmaTurret = "ImpactPlasmaTurret";
        public static readonly string ImpactRocketTurret = "ImpactRocketTurret";
    }

    public static class BuffVfxName
    {
        public static  readonly string TurretBuffHp = "TurretBuffHp";
        public static readonly string TurretBuffDef = "TurretBuffDef";
    }

    public static class AbilityName
    {
        public static readonly string SoulCastSpell = "SoulCastSpell";
    }
}
