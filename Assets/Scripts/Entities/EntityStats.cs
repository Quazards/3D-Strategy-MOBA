using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    private float health = 300;
    private float mana = 500;
    private float minAtk = 41;
    private float maxAtk = 45;

    public float basicAtkStat
    {
        get
        {
            return Random.Range(minAtk, maxAtk);
        }
    }

    public void TakeDamage(GameObject target, float damage)
    {
        target.GetComponent<EntityStats>().health -= damage;
    }
}
