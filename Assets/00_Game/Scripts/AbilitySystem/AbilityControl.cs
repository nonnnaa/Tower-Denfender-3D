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
    private SoulDataBinding dataBinding;
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

        if (dataBinding == null)
        {
            Debug.LogError($"[{name}] SoulDataBinding not found! Please add component.");
        }
    }

    IEnumerator HandleSkillLogic()
    {
        while (abilityState != AbilityState.None && abilitySkill != null)
        {
            switch (abilityState)
            {
                case AbilityState.Ready:
                    if (abilitySkill != null)
                    {
                        abilitySkill.OnActive();
                        if (dataBinding != null)
                            dataBinding.SetTriggerAnim(abilitySkill.AnimType);

                        abilityState = AbilityState.Active;
                        activeTime = abilitySkill.ActiveTime;
                        isTriggered = false;
                        triggerTime = 0;
                    }
                    else
                    {
                        abilityState = AbilityState.None;
                    }
                    break;

                case AbilityState.Active:
                    if (activeTime > 0)
                    {
                        activeTime -= Time.deltaTime;
                        triggerTime += Time.deltaTime;

                        if (!isTriggered && abilitySkill != null && triggerTime > abilitySkill.TriggerTime)
                        {
                            abilitySkill.OnTrigger();
                            isTriggered = true;
                        }
                    }
                    else
                    {
                        abilityState = AbilityState.Cooldown;
                        if (abilitySkill != null)
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
            yield return null;
        }
    }

    public void SelectAbilitySkill()
    {
        if (abilitySkillInfor == null || abilitySkillInfor.Count == 0)
        {
            Debug.LogWarning($"[{name}] No ability skills available to select!");
            return;
        }

        if (sumOfRatios <= 0)
        {
            Debug.LogWarning($"[{name}] Sum of ratios is zero, cannot select ability skill.");
            return;
        }

        int randomValue = Random.Range(0, sumOfRatios);
        int currentLimit = 0;

        for (int i = 0; i < abilitySkillInfor.Count; i++)
        {
            currentLimit += abilitySkillInfor[i].GetRatio();

            if (randomValue < currentLimit)
            {
                abilitySkill = abilitySkillInfor[i].GetSkill();
                if (abilitySkill == null)
                {
                    Debug.LogWarning($"[{name}] Selected ability at index {i} is NULL!");
                    return;
                }

                abilityState = AbilityState.Ready;
                StartCoroutine(HandleSkillLogic());
                break; 
            }
        }
    }

    private int GetSumOfRatios()
    {
        int sum = 0;
        foreach (AbilityInfo abilityInfo in abilitySkillInfor)
        {
            sum += Mathf.Max(0, abilityInfo.GetRatio()); 
        }
        return sum;
    }
}
