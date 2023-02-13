using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    PlayerController player;
    [SerializeField]
    float iframes;

    void Start() {
        player = FindObjectOfType<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "MeleeEnemy") {
            print("Hit!");
            other.GetComponent<MeleeEnemy>().TakeDamage(player.attackDamage);
            other.GetComponent<MeleeEnemy>().Knockback(player.attackKB, iframes);
            
        }
        if (other.tag == "RangedEnemy") {
            other.GetComponent<RangedEnemy>().TakeDamage(player.attackDamage);
            other.GetComponent<RangedEnemy>().Knockback(player.attackKB, iframes);
        }
    }
}
