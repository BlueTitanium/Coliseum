using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float knockback, lifetime, damage;

    public Vector2 direction;
    GameObject player;
    public GameObject explosion;

    void Start() {
        print("spawned");
        player = GameObject.FindWithTag("Player");
        if (ArenaManager.Instance != null)
        {
            damage *= ArenaManager.Instance.enemyDamageMultiplier;
        }
        Destroy(gameObject, lifetime);
    }

    void Update() {
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if(PlayerController.p.dashLeft <= 0)
            {
                PlayerController.p.TakeKnockback(direction, knockback);
                PlayerController.p.TakeDamage(damage);
                Instantiate(explosion, transform.position, explosion.transform.rotation);
                print("player");
                Destroy(gameObject);
            }
        }
        if (other.gameObject.CompareTag("Wall")){
            print("wall");
            Destroy(gameObject);
        }
    }
}
