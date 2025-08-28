using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AbilityState
{
   Ready,
   Active,
   Cooldown,
}

[Serializable]
public class AbilityInfo
{
    [SerializeField] private AbilitySkill skill;
    [SerializeField] private float ratio;
    public AbilitySkill GetSkill() => skill;
    public float GetRatio() => ratio;
}

public class AbilityControl : MonoBehaviour
{
    [SerializeField] private List<AbilityInfo> abilitySkillInfor = new List<AbilityInfo>();
    
    private AbilitySkill abilitySkill;
    private AbilityState abilityState = AbilityState.Cooldown;
    private float cooldownTime;
    private float activeTime;
    private float triggerTime;
    private bool isTriggered;

    private void Update()
    {
        if (abilitySkill == null) return;
        if(abilitySkill.CanActivate())
        {
            HandleSkillLogic();
        }
    }
    void HandleSkillLogic()
    {
        switch (abilityState)
        {
            case AbilityState.Ready:
               
                abilitySkill = SelectSkillByRatio();
                abilitySkill.OnActive();
                abilityState = AbilityState.Active;
                activeTime = abilitySkill.ActiveTime;
                isTriggered = false;
                triggerTime = 0;
                break;

            case AbilityState.Active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                    triggerTime += Time.deltaTime;

                    if (!isTriggered && triggerTime > abilitySkill.TriggerTime)
                    {
                        abilitySkill.OnTrigger();
                        isTriggered = true;
                    }
                }
                else
                {
                    abilityState = AbilityState.Cooldown;
                    cooldownTime = abilitySkill.CoolDownTime;
                }
                break;

            case AbilityState.Cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    abilityState = AbilityState.Ready;
                    abilitySkill = null;
                }
                break;
        }
    }
    private AbilitySkill SelectSkillByRatio()
    {
        if (abilitySkillInfor.Count == 0) return null;
        float totalWeight = 0f;
        foreach (var infor in abilitySkillInfor)
        {
            totalWeight += infor.GetRatio(); 
        }

        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var infor in abilitySkillInfor)
        {
            cumulative += infor.GetRatio();
            if (randomValue <= cumulative)
            {
                return infor.GetSkill();
            }
        }
        return null; 
    }
}
