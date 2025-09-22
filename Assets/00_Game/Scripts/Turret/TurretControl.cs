using UnityEngine;

public class TurretControl : FSMSystem
{
    protected HealthControl healthControl;

    protected override void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PAUSE)
        {
            return;
        }
        base.Update();
    }

    protected virtual void OnInit()
    {
        
    }
    protected virtual void OnSelect()
    {
        
    }
    protected virtual void OnDeselect()
    {
        
    }
    protected virtual void OnSell()
    {
        
    }
    protected virtual void OnDead()
    {
        
    }
    protected virtual void OnDamaged()
    {
        
    }
    protected virtual void OnDespawn()
    {
        
    }
}



