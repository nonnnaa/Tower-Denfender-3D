using System;

public class EventManager : SingletonMono<EventManager>
{
    public Action<bool> OnOpenTurretSelection;
    public Action OnOpenSellTurretPopup;

    public Action OnEnemyDestroy;
    public Action OnLoseLevel;
}