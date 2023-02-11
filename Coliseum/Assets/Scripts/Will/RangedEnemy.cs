using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField]
    float timer, maxTimer, range;

    [SerializeField]
    float speed, projectileSpeed;
    public GameObject player;
    public GameObject testProjectile;
    public Rigidbody2D enemyRb;

    [SerializeField]
    float distance;
    public Vector2 direction;

    // The following vars are for testing purposes only
    [SerializeField]
    int kb;
    [SerializeField]
    float cooldown = 0f;
    // The above are to be removed

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

    public void Knockback(int kb) {
        enemyRb.AddForce(-1 * direction * new Vector2(kb, kb));
        timer += 1f;
    }
}
