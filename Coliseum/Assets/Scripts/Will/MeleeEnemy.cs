using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField]
    float knockback, timer, maxTimer;

    public GameObject player;
    public float speed;
    //float distance;
    Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0) {
            //distance = Vector2.Distance(transform.position, player.transform.position);
            direction = player.transform.position - transform.position;

            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        } else {
            timer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * new Vector2(knockback, knockback));
            Debug.Log(direction * new Vector2(knockback, knockback));
            timer = maxTimer;
        }
    }
}
