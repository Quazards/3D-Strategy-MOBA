using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private Transform originalTarget;
    private Rigidbody _rigidbody;

    private EntityStats playerStats;

    [SerializeField] private float projectileSpeed;

    private void Start()
    {
        originalTarget = target;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(target != null)
        {
            Vector3 direction = target.position - transform.position;
            _rigidbody.velocity = direction.normalized * projectileSpeed;
        }
        else if (originalTarget != null)
        {
            Vector3 direction = originalTarget.position - transform.position;
            _rigidbody.velocity = direction.normalized * projectileSpeed;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target != null && ReferenceEquals(other.gameObject, target.gameObject))
        {
            EntityStats targetStats = target.gameObject.GetComponent<EntityStats>();
            targetStats?.TakeDamage(target.gameObject, playerStats.basicAtkStat);
            Destroy(gameObject);
        }
        else if (originalTarget != null && ReferenceEquals(other.gameObject, originalTarget.gameObject))
        {
            EntityStats originalTargetStats = originalTarget.gameObject.GetComponent<EntityStats>();
            originalTargetStats?.TakeDamage(originalTarget.gameObject, playerStats.basicAtkStat);
            Destroy(gameObject);
        }
    }
}
