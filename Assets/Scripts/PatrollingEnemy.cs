using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    [SerializeField] PatrolData[] patrolBehaviours;


    [System.Serializable]
    public struct PatrolData
    {
        public float PatrolDuration;
        public float MoveSpeed;
        public float MoveDirectionDuration;
        public float waitDuration;
    }

    enum EnemyState
    {
        Stopped,
        Waiting,
        Patrolling
    }

    enum PatrolDirection
    {
        Left,
        Right
    }

    EnemyState currentEnemyState;

    PatrolDirection currentPatrolDirection;

    float startPatrolTime;

    float directionChangeTime;

    float waitTime;

    PatrolData currentPatrolData;

    int patrolIndex;


    void Start()
    {
        currentPatrolData = patrolBehaviours[0];
        currentEnemyState = EnemyState.Stopped;
        currentPatrolDirection = PatrolDirection.Right;
        directionChangeTime = 0;
        patrolIndex = -1;
    }

    void Update()
    {
        switch (currentEnemyState)
        {
            default:
            case EnemyState.Stopped:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (patrolIndex < patrolBehaviours.Length - 1)
                    {
                        patrolIndex++;
                        currentPatrolData = patrolBehaviours[patrolIndex];
                    }
                    currentEnemyState = EnemyState.Patrolling;
                    startPatrolTime = Time.time;
                    waitTime = 0;
                }
                break;

            case EnemyState.Waiting:
                Wait(currentPatrolData);
                break;

            case EnemyState.Patrolling:

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentEnemyState = EnemyState.Stopped;
                }

                if (Time.time > startPatrolTime + currentPatrolData.PatrolDuration)
                {
                    currentEnemyState = EnemyState.Waiting;
                }

                else
                {
                    PatrolRoutine(currentPatrolData);
                }

                break;
        }
    }


    void Wait(PatrolData patrolData)
    {
        if (waitTime < patrolData.waitDuration)
        {
            waitTime += Time.deltaTime;
        }
        else
        {
            currentEnemyState = EnemyState.Stopped;
        }

    }

    void PatrolRoutine(PatrolData patrolData)
    {
        directionChangeTime += Time.deltaTime;

        switch (currentPatrolDirection)
        {

            case PatrolDirection.Left:
                if (directionChangeTime > patrolData.MoveDirectionDuration)
                {
                    ChangePatrolDirection(PatrolDirection.Right);
                }
                else
                {
                    Translate(new Vector3(-patrolData.MoveSpeed * Time.deltaTime, 0, 0));
                }
                break;

            case PatrolDirection.Right:
                if (directionChangeTime > patrolData.MoveDirectionDuration)
                {
                    ChangePatrolDirection(PatrolDirection.Left);
                }
                else
                {
                    Translate(new Vector3(patrolData.MoveSpeed * Time.deltaTime, 0, 0));
                }
                break;
        }
    }

    void ChangePatrolDirection(PatrolDirection newPatrolDirection)
    {
        currentPatrolDirection = newPatrolDirection;
        directionChangeTime = 0;
    }

    void Translate(Vector3 translation)
    {
        transform.position = transform.position + translation;
    }
}
