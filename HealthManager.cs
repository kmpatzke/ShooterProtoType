using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour {

    [SerializeField] int health;
    private enum target { ENEMY, EXPLODABLE }
    [SerializeField] private target currentTarget;

    private void Start()
    {
        if (GetComponent<Enemy_AI>())
        {
            currentTarget = target.ENEMY;
        }
        else
        {
            currentTarget = target.EXPLODABLE;
        }
    }


    public void GetHit(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            if(currentTarget == target.ENEMY)
            {
                var selfEnemy_AI = GetComponent<Enemy_AI>();
                selfEnemy_AI.Die();
            }
            else
            {

            }
            
        }
    }

    
}
