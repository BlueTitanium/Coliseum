using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    GameObject player;
    float damage, knockback;

    void Start() {
        player = GameObject.FindWithTag("Player");
        damage = player.GetComponent<PlayerController>().attackDamage;
        knockback = player.GetComponent<PlayerController>().attackKB;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "MeleeEnemy") {
            other.GetComponent<MeleeEnemy>().Knockback(knockback);
        }
        if (other.tag == "RangedEnemy") {
            other.GetComponent<RangedEnemy>().Knockback(knockback);
        }
    }
}
