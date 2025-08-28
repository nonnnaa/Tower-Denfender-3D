using UnityEngine;

public class SoulMoveState : FSMState
{
    private SoulControl soulControl;
    private GameObject tower;
    private Transform soulTransform;
    public SoulMoveState(SoulControl soulControl)
    {
        this.soulControl = soulControl;
        soulTransform = soulControl.transform;
        tower = GameObject.FindGameObjectWithTag("Tower");
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        if (tower != null)
        {
            float distance = Vector3.Distance(tower.transform.position, soulTransform.position);
            if (distance < soulControl.GetAttackRange())
            {
                soulControl.ChangeState(soulControl.attackState);
            }
            else
            {
                soulTransform.position = Vector3.MoveTowards(
                    soulTransform.position,
                    tower.transform.position,
                    soulControl.GetSpeedMove() * Time.deltaTime
                );
                soulTransform.LookAt(tower.transform);
            }
        }
    }
    public override void ExitState()
    {
        
    }
}
