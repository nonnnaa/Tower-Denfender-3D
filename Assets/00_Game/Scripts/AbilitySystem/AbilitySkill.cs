using UnityEngine;

public class AbilitySkill : ScriptableObject
{
    [SerializeField] private float coolDownTime;
    public float CoolDownTime => coolDownTime;
    
    [SerializeField] private float triggerTime;
    public float TriggerTime => triggerTime;
    
    [SerializeField] private float activeTime;
    public float ActiveTime => activeTime;

    public virtual bool CanActivate()
    {
        return false;
    }
    
    public virtual void OnActive()
    {
        
    }
    public virtual void OnTrigger()
    {
        
    }
}
