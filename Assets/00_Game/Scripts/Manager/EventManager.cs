using System;
using UnityEngine;

public class EventManager : SingletonMono<EventManager>
{
    public Action<bool> OnOpenTurretSelection;
    public Action OnOpenSellTurretPopup;
}