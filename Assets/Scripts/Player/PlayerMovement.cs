using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private  NavMeshAgent agent;
    [SerializeField] private float rotationSpeed = 0.05f;
    [SerializeField] private float rotationVelocity;

    [SerializeField] private Animator _animator;
    private float motionSmoothing = 0.1f;

    public GameObject enemyTarget;
    public float stoppingDistance;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        PlayerAnimation();
        Movement();
    }

    public void PlayerAnimation()
    {
        float speed = agent.velocity.magnitude / agent.speed;
        _animator.SetFloat("Speed", speed, motionSmoothing, Time.deltaTime);
    }

    public void Movement()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Ground")
                {
                    MoveToPosition(hit.point);

                }
                else if (hit.collider.CompareTag("Enemy"))
                {
                    MoveToEnemy(hit.collider.gameObject);
                }
            }
        }
    }

    public void MoveToEnemy(GameObject enemy)
    {
        enemyTarget = enemy;
        agent.SetDestination(enemyTarget.transform.position);
        agent.stoppingDistance = stoppingDistance;

        Rotation(enemyTarget.transform.position);
    }

    private void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
        agent.stoppingDistance = 0;

        Rotation(position);

        if(position.y >= 0)
        {
            enemyTarget = null;
        }
    }

    private void Rotation(Vector3 position)
    {
        Quaternion rotation = Quaternion.LookRotation(position - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation.eulerAngles.y, ref rotationVelocity
            , rotationSpeed * (Time.deltaTime * 5));

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }
}
