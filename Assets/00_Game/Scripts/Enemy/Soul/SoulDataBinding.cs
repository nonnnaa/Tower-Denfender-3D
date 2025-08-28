using UnityEngine;
using CONSTANT;
public class SoulDataBinding : MonoBehaviour
{
    private readonly int attackAnimKey = Animator.StringToHash("IsAttacking");
    private readonly int moveAnimKey = Animator.StringToHash("SpeedMove");
    private readonly int spawnAnimKey = Animator.StringToHash(CharactorAnimName.Spawn);
    private readonly int hitAnimKey = Animator.StringToHash(CharactorAnimName.Hit);
    private readonly int deadAnimKey = Animator.StringToHash(CharactorAnimName.Dead);
    private readonly int castSpellAnimKey = Animator.StringToHash(CharactorAnimName.CastSpell);
    private readonly int  summonAnimKey = Animator.StringToHash(CharactorAnimName.Summon);
    private readonly int projectileAnimKey = Animator.StringToHash(CharactorAnimName.Projectile);
    
    [SerializeField] private Animator animator;
    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    private bool isAttacking = false;
    private float speedMove;
    public bool IsAttacking
    {
        get => isAttacking;
        set
        {
            isAttacking = value; 
            animator.SetBool(attackAnimKey, isAttacking);
        }
    }
    public float SpeedMove
    {
        get => speedMove;
        set
        {
            speedMove = value;
            animator.SetFloat(moveAnimKey, speedMove);
        }
    }
    public void SetHitTrigger() => animator.SetTrigger(hitAnimKey);
    public void SetDeadTrigger() => animator.SetTrigger(deadAnimKey);
    public void SetCastSpellTrigger() => animator.SetTrigger(castSpellAnimKey);
    public void SetSummonTrigger() => animator.SetTrigger(summonAnimKey);
    public void SetProjectileTrigger() => animator.SetTrigger(projectileAnimKey);
}
