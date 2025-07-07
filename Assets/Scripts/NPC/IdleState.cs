using UnityEngine;

public class IdleState : IState
{
    public void Enter()
    {
        Debug.Log("Enteded IdleState");
    }

    public void Exit()
    {
        Debug.Log("Exit IdleState");
    }

    public void UpdateState()
    {
        Debug.Log("UpdateState IdleState");
    }
}
