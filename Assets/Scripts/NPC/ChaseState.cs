using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChaseState : IState
{

    private Transform chaseTarget;
    private Transform chaserPosition;
    private float speed = 3f;

    public ChaseState(Transform target, Transform ownPosition)
    {

        chaseTarget = target;
        chaserPosition = ownPosition;
    }

    public void Enter()
    {
        Debug.Log("Enteded ChaseState");
    }

    public void Exit()
    {
        Debug.Log("Exit ChaseState");
    }

    public void UpdateState()
    {
        Debug.Log("UpdateState ChaseState");

        Vector3 direction = (chaseTarget.position - chaserPosition.position).normalized;
        chaserPosition.position += direction * speed * Time.deltaTime;
    }
}