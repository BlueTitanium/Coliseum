using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float knockback, lifetime, damage;

    public Vector2 direction;
    GameObject player;

    void Start() {
        player = GameObject.FindWithTag("Player");
    }

    void Update() {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            player.GetComponent<PlayerController>().TakeKnockback(direction, knockback);
            player.GetComponent<PlayerController>().TakeDamage(damage);
        }
        if (other.tag != "Enemy" && other.tag != "Projectile") {
            Destroy(gameObject);
        }
    }
}
