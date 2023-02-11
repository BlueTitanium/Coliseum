using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField]
    float knockback, timer, maxTimer, damage, invuln;

    GameObject player;
    public float speed;
    public Rigidbody2D enemyRb;
    public Collider2D enemyCol;
    Vector2 direction;

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

        if (invuln > 0) {
            invuln -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && timer <= 0) {
            player.GetComponent<PlayerController>().TakeKnockback(direction, knockback);
            player.GetComponent<PlayerController>().TakeDamage(damage);
            timer = maxTimer;
        }
    }

    public void Knockback(float kb, float iframes) {
        if (invuln <= 0) {
            enemyRb.AddForce(-1 * direction * new Vector2(kb, kb));
            Debug.Log("test");
            timer += 1f;
            invuln = iframes;
        }
    }
}
