using UnityEngine;
public class FSMSystem : MonoBehaviour
{
    public FSMState currentState;

    public void ChangeState(FSMState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }
    public void ChangeState(FSMState newState, object data)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState(data);
    }
    protected virtual void FixedUpdate()
    {
        currentState?.FixedUpdateState();
    }
    protected virtual  void LateUpdate()
    {
        currentState?.LateUpdateState();
    }
    protected virtual void Update()
    {
        currentState?.UpdateState();
    }
    public void OnEnterAnim()
    {
        currentState?.OnEnterAnim();
    }
    public virtual void OnMidlleAnim()
    {
        currentState?.OnMidlleAnim();
    }
    public void OnExitAnim()
    {
        currentState?.OnExitAnim();
    }
}