using UnityEngine;
using System.Collections.Generic;
using CONSTANT;
public enum AnimType
{
    Spawn,
    Hit,
    Dead,
    CastSpell,
    Summon,
    Projectile,
    Spin,
    Roll
}

public class SoulDataBinding : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private readonly int attackAnimKey = Animator.StringToHash("IsAttacking");
    private readonly int moveAnimKey = Animator.StringToHash("MoveSpeed");
    private Dictionary<AnimType, int> animKeys;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animKeys = new Dictionary<AnimType, int>
        {
            { AnimType.Spawn, Animator.StringToHash(CharactorAnimName.Spawn) },
            { AnimType.Hit, Animator.StringToHash(CharactorAnimName.Hit) },
            { AnimType.Dead, Animator.StringToHash(CharactorAnimName.Dead) },
            { AnimType.CastSpell, Animator.StringToHash(CharactorAnimName.CastSpell) },
            { AnimType.Summon, Animator.StringToHash(CharactorAnimName.Summon) },
            { AnimType.Projectile, Animator.StringToHash(CharactorAnimName.Projectile) },
            { AnimType.Spin, Animator.StringToHash(CharactorAnimName.Spin) },
            { AnimType.Roll, Animator.StringToHash(CharactorAnimName.Roll) }
        };
    }

    private bool isAttacking;
    private float speedMove;

    public bool IsAttacking
    {
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

    public void SetTriggerAnim(AnimType animType)
    {
        if (animKeys.TryGetValue(animType, out int key))
        {
            animator.SetTrigger(key);
        }
        else
        {
            Debug.LogWarning($"[SoulDataBinding] No anim key mapped for {animType}");
        }
    }
}

