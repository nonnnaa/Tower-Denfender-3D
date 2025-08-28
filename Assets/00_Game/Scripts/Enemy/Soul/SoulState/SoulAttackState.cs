using UnityEngine;

public class SoulAttackState : FSMState
{
   private SoulControl soulControl;
   private AbilityControl abilityControl;
   private SoulDataBinding dataBinding;
   private GameObject tower;
   public SoulAttackState(SoulControl soulControl)
   {
      this.soulControl = soulControl;
      abilityControl = soulControl.GetComponent<AbilityControl>();
      dataBinding = soulControl.GetComponent<SoulDataBinding>();
      tower = GameObject.FindGameObjectWithTag("Tower");
   }

   public override void EnterState()
   {
      base.EnterState();
      dataBinding.IsAttacking = true;
   }

   public override void UpdateState()
   {
      base.UpdateState();
      float distance = Vector3.Distance(tower.transform.position, soulControl.transform.position);
      if (distance > soulControl.GetAttackRange())
      {
         soulControl.ChangeState(soulControl.moveState);
      }
   }

   public override void ExitState()
   {
      base.ExitState();
      dataBinding.IsAttacking = false;
   }
}
