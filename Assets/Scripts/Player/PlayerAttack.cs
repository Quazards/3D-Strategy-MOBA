using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement movement;
    private EntityStats stats;
    private Animator animator;

    public GameObject targetEnemy;

    private float atkInterval = 0.7f;
    private float nextAttack;
    public bool doBasicAttack = true;

    public Transform shootPoint;
    public GameObject projectilePrefab;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        stats = GetComponent<EntityStats>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        targetEnemy = movement.enemyTarget;

        if(targetEnemy != null && Time.time > nextAttack)
        {
            if(Vector3.Distance(transform.position, targetEnemy.transform.position) <= movement.stoppingDistance)
            {
                StartCoroutine(SimulateBasicAttack());
            }
        }
    }

    private IEnumerator SimulateBasicAttack()
    {
        if (!doBasicAttack)
            yield break; // Prevent multiple simultaneous attacks

        doBasicAttack = false;

        // Trigger the attack animation (SetTrigger ensures it plays the animation once)
        animator.SetTrigger("isAttacking");

        // Wait for the duration of the attack animation
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        // Wait for the animation to finish before continuing
        yield return new WaitForSeconds(animationDuration);

        // After attack animation finishes, trigger the basic attack logic

        // Cooldown before next attack
        nextAttack = Time.time + atkInterval;
        doBasicAttack = true;
    }

    private void BasicAttack()
    {
        GameObject attackProjectile = Instantiate(projectilePrefab, shootPoint.transform.position, shootPoint.transform.rotation);

        Projectile currentProjectie = attackProjectile.GetComponent<Projectile>();

        if(currentProjectie != null)
        {
            currentProjectie.SetTarget(targetEnemy.transform);
        }

        nextAttack = Time.time + atkInterval;

        doBasicAttack = true;
        animator.SetBool("isAttacking", false );
    }    
}
