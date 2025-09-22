using System;
using UnityEngine;

public class EventManager : SingletonMono<EventManager>
{
    public Action OnOpenTurretSelection;
    public Action OnOpenSellTurretPopup;
}