using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{

    enum EnemyState
    {
        Stopped,
        Patrolling1,
        Patrolling2
    }

    enum PatrolDirection
    {
        Left,
        Right
    }


    [System.Serializable]
    struct PatrolData
    {
        public float PatrolDuration;
        public float MoveSpeed;
        public float MoveDirectionDuration;
    }

    EnemyState currentEnemyState;

    [SerializeField]
    PatrolData patrolData1;

    [SerializeField]
    PatrolData patrolData2;

    PatrolDirection currentPatrolDirection;

    float startPatrolTime;

    float directionChangeTime;

    void Start()
    {
        currentEnemyState = EnemyState.Stopped;
        currentPatrolDirection = PatrolDirection.Right;
        directionChangeTime = 0;
    }

    void Update()
    {
        switch (currentEnemyState)
        {
            default:
            case EnemyState.Stopped:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentEnemyState = EnemyState.Patrolling1;
                    startPatrolTime = Time.time;
                }
                break;

            case EnemyState.Patrolling1:
                if (Time.time > startPatrolTime + patrolData1.PatrolDuration)
                {
                    currentEnemyState = EnemyState.Patrolling2;
                }

                else
                {
                    PatrolRoutine(patrolData1);
                }

                break;

            case EnemyState.Patrolling2:
                if (Time.time > startPatrolTime + patrolData2.PatrolDuration)
                {
                    currentEnemyState = EnemyState.Stopped;
                }

                else
                {
                    PatrolRoutine(patrolData2);
                }

                break;
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
