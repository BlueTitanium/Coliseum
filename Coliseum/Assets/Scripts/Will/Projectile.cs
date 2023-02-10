using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float knockback, lifetime;

    public Vector2 direction;

    void Update() {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * new Vector2(knockback, knockback));
            Debug.Log(direction * new Vector2(knockback, knockback));
        }
        if (other.tag != "Enemy" && other.tag != "Projectile") {
            Destroy(gameObject);
        }
    }
}
