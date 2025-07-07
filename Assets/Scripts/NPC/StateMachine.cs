using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StateMachine : MonoBehaviour
{
    private Transform target;
    private IState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = FindAnyObjectByType<PlayerController>().transform;
        currentState = new IdleState();
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case IdleState idle: 
                if(Vector3.Distance(this.transform.position, target.transform.position) < 5f)
                {
                    currentState.Exit();
                    currentState = new ChaseState(target,this.transform);
                    currentState.Enter();
                }
                break;
            case ChaseState chase:
                if (Vector3.Distance(this.transform.position, target.transform.position) > 7f)
                {
                    currentState.Exit();
                    currentState = new IdleState();
                    currentState.Enter();
                }
                break;
        }

        currentState.UpdateState();
    }
}
