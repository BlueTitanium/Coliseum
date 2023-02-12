using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField]
    float timer, maxTimer, range, invuln;

    [SerializeField]
    float speed, projectileSpeed;
    public GameObject testProjectile;
    public Rigidbody2D enemyRb;

    GameObject player;

    [SerializeField]
    float distance;
    public Vector2 direction;

    void Start() {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position;

        if (distance > range) {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        } else {
            if (timer <= 0) {
                GameObject newProjectile = Instantiate(testProjectile, transform.position, Quaternion.identity);
                newProjectile.GetComponent<Projectile>().direction = direction;
                newProjectile.GetComponent<Rigidbody2D>().AddForce(direction * new Vector2(projectileSpeed, projectileSpeed));
                timer = maxTimer;
            }
        }

        if (timer > 0) {
            timer -= Time.deltaTime;
        }

        if (invuln > 0) {
            invuln -= Time.deltaTime;
        }
    }

    public void Knockback(float kb, float iframes) {
        if (invuln <= 0) {
            enemyRb.AddForce(-1 * direction * new Vector2(kb, kb));
            timer += 1f;
            invuln += iframes;
        }
    }
}
