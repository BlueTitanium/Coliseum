using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public MeleeEnemy enemy;
    float damage, knockback;

    void Start() {
        damage = enemy.damage;
        knockback = enemy.knockback;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !enemy.hasHit)
        {
            PlayerController.p.TakeKnockback(enemy.direction, knockback);
            PlayerController.p.TakeDamage(damage);
            enemy.hasHit = true;
        }
    }
}
