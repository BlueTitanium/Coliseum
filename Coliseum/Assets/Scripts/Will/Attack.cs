using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    PlayerController player;
    float damage, knockback;
    [SerializeField]
    float iframes;

    void Start() {
        player = FindObjectOfType<PlayerController>();
        damage = player.attackDamage;
        knockback = player.attackKB;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "MeleeEnemy") {
            print("Hit!");
            other.GetComponent<MeleeEnemy>().TakeDamage(damage);
            other.GetComponent<MeleeEnemy>().Knockback(knockback, iframes);
            
        }
        if (other.tag == "RangedEnemy") {
            other.GetComponent<RangedEnemy>().TakeDamage(damage);
            other.GetComponent<RangedEnemy>().Knockback(knockback, iframes);
        }
    }
}
