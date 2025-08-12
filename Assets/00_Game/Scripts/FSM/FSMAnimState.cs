using UnityEngine;
public class FSMAnimState : StateMachineBehaviour
{
    private FSMSystem fsmSystem;
    private float timeMiddle;
    private float timeCount;
    private bool isCall;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isCall = false;
        timeCount = 0;
        if (fsmSystem == null)
        {
            fsmSystem = animator.GetComponent<FSMSystem>();
        }
        fsmSystem.OnEnterAnim();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeCount += Time.deltaTime;
        if(timeCount >= timeMiddle && !isCall)
        {
            isCall = true;
            fsmSystem.OnMidleAnim();
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        fsmSystem.OnExitAnim();
    }
}