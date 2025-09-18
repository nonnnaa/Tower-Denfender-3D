using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AbilityState
{
    None,
    Ready,
    Active,
    Cooldown,
}

[Serializable]
public class AbilityInfo
{
    [SerializeField] private AbilitySkill skill;
    [SerializeField] private int ratio;
    public AbilitySkill GetSkill() => skill;
    public int GetRatio() => ratio;
}

public class AbilityControl : MonoBehaviour
{
    SoulDataBinding dataBinding;
    [SerializeField] private List<AbilityInfo> abilitySkillInfor = new List<AbilityInfo>();
    private AbilitySkill abilitySkill;
    private AbilityState abilityState = AbilityState.None;
    public AbilityState GetAbilityState() => abilityState;
    private float cooldownTime;
    private float activeTime;
    private float triggerTime;
    private bool isTriggered;
    private int sumOfRatios;
    private void Awake()
    {
        sumOfRatios = GetSumOfRatios();
        dataBinding = GetComponent<SoulDataBinding>();
    }

    IEnumerator HandleSkillLogic()
    {
        while (abilityState != AbilityState.None && abilitySkill != null)
        {
            switch (abilityState)
            {
                case AbilityState.Ready:
                    abilitySkill.OnActive();
                    dataBinding.SetTriggerAnim(abilitySkill.AnimType);
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
                        abilityState = AbilityState.None;
                        abilitySkill = null;
                    }
                    break;
            }
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
    }
    

    public void SelectAbilitySkill()
    {
        int randomValue = Random.Range(0, sumOfRatios);
        int currentLimit = abilitySkillInfor[0].GetRatio();
        for(int i=0 ; i<abilitySkillInfor.Count; i++)
        {
            if (randomValue < currentLimit)
            {
                abilitySkill =  abilitySkillInfor[i].GetSkill();
                abilityState = AbilityState.Ready;
                StartCoroutine(HandleSkillLogic());
            }
            else
            {
                currentLimit += abilitySkillInfor[i].GetRatio();
            }
        }
    }
    private int GetSumOfRatios()
    {
        int sum = 0;
        foreach (AbilityInfo abilityInfo in abilitySkillInfor)
        {
            sum += abilityInfo.GetRatio();
        }
        return sum;
    }
}
