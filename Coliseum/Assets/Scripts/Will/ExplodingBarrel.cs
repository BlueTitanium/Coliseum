using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    //public Animator spriteAnimator;
    public GameObject explosion;
    public Rigidbody2D rb;

    [SerializeField]
    float damage, time, explodeTime, blastRadius, speed;

    void Start() {
        explodeTime = Random.Range(5, 10);
        // Following line(s) are for rolling barrels
        transform.rotation = new Quaternion(0, 0, Random.Range(0,2) * 90, 0);

        if (transform.rotation.z == 0) {
            rb.velocity = new Vector2(Random.Range(-1 * speed, speed), 0);
        } else {
            rb.velocity = new Vector2(0, Random.Range(-1 * speed, speed));
        }
    }

    void Update() {
        if (explodeTime >= 0) {
            explodeTime -= Time.deltaTime;
        } else {
            Explode();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" || other.tag == "PlayerAttack" || other.tag == "Projectile") {
            //animation/trigger could be added here
            Explode();
            Destroy(gameObject);
        }

    }

    public void Explode() {
        Instantiate(explosion, transform.position, transform.rotation);
        Debug.Log("test");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach (Collider2D other in colliders) {
            if (other.tag == "Player") {
                PlayerController player = other.GetComponent<PlayerController>();
                player.TakeDamage(damage);
                player.TakeKnockback(new Vector2(Random.Range(-1,1), Random.Range(-1,1)), time);
            }
            // Following if statements are optional if we want enemy to be damaged by explosion
            // Placeholder values are put for the enemy knockbacks;
            /*
            if (other.tag == "MeleeEnemy") {
                other.GetComponent<MeleeEnemy>().TakeDamage(damage);
                other.GetComponent<MeleeEnemy>().Knockback(5f, time);
            }
            if (other.tag == "RangedEnemy") {
                other.GetComponent<RangedEnemy>().TakeDamage(damage);
                other.GetComponent<RangedEnemy>().Knockback(5f, time);
            }
            */
        }
    }
}
