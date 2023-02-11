using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField]
    float knockback, timer, maxTimer, damage;

    GameObject player;
    public float speed;
    public Rigidbody2D enemyRb;
    public Collider2D enemyCol;
    Vector2 direction;

    // The following vars are for testing purposes only
    [SerializeField]
    int kb;
    [SerializeField]
    float cooldown = 0f;
    // The above are to be removed

    void Start() {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        direction = player.transform.position - transform.position;
        if (timer <= 0) {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            enemyCol.isTrigger = true;
        } else {
            timer -= Time.deltaTime;
            enemyCol.isTrigger = false;
        }

        // The following lines are for testing purposes only
        if (Input.GetKey("space")) {
            if (cooldown <= 0) {
                Knockback(kb);
                cooldown = 1f;
            }
        }

        if (cooldown > 0) {
            cooldown -= Time.deltaTime;
        }
        // The above is to be removed
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && timer <= 0) {
            player.GetComponent<PlayerController>().TakeKnockback(direction, knockback);
            player.GetComponent<PlayerController>().TakeDamage(damage);
            timer = maxTimer;
        }
    }

    public void Knockback(int kb) {
        enemyRb.AddForce(-1 * direction * new Vector2(kb, kb));
        timer += 1f;
    }
}
