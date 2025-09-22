using UnityEngine;
[CreateAssetMenu(fileName = "CastSpellAbility", menuName = "Create Ability/CastSpellAbility")]
public class CastSpellSkill : AbilitySkill
{
    [SerializeField] private float damage;
    [SerializeField] private float rangeCast;
    private Transform parent;
    private Transform tower;

    public override AnimType AnimType => AnimType.CastSpell;

    public override bool CanActivate()
    {
        if (tower == null)
        {
             tower = GameObject.FindGameObjectWithTag("Tower").gameObject.transform;
        }
        if (Vector3.Distance(tower.transform.position, parent.position) < rangeCast)
        {
            return true;
        }
        return false;
    }
    
    public override void OnActive()
    {
        base.OnActive();
        //Debug.Log("CastSpellSkill take damage: " + damage);
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        //Debug.Log("CastSpellSkill Trigger damage");
    }
}
