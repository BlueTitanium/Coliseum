using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyV1 : MonoBehaviour
{
    [SerializeField]
    float knockback;

    public GameObject player;
    public float speed;
    float distance;
    Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * new Vector2(knockback, knockback));
            Debug.Log(direction * new Vector2(knockback, knockback));
        }
    }
}
