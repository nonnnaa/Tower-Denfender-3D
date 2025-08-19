namespace CONSTANT
{
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
    }

    public static class ImpactName
    {
        public static readonly string ImpactShortGunTurret = "ImpactShortGunTurret";
        public static string ImpactPlasmaTurret = "ImpactPlasmaTurret";
    }
}
