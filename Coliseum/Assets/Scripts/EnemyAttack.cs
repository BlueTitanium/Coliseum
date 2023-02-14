using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public MeleeEnemy enemy;
    public AudioSource source;

    void Start() {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !enemy.hasHit)
        {
            source.Play();
            PlayerController.p.TakeKnockback(enemy.direction, enemy.knockback);
            PlayerController.p.TakeDamage(enemy.damage);
            enemy.hasHit = true;
        }
    }
}
