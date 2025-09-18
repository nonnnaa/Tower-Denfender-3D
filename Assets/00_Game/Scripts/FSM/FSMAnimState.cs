using UnityEngine;
public class FSMAnimState : StateMachineBehaviour
{
    private FSMSystem fsmSystem;
    private float timeCount;
    [SerializeField] private float timeMiddle;
    private bool isCall;
    public float GetTimeCount() { return timeCount; }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isCall = false;
        timeCount = 0;
        if (fsmSystem == null)
        {
            fsmSystem = animator.GetComponentInParent<FSMSystem>();
        }
        fsmSystem.OnEnterAnim();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeCount += Time.deltaTime;
        if(timeCount >= timeMiddle && !isCall)
        {
            isCall = true;
            fsmSystem.OnMidlleAnim();
        }
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        fsmSystem.OnExitAnim();
    }
}