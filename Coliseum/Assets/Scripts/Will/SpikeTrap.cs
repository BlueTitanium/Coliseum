using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Animator spriteAnimator;
    [SerializeField]
    float damage, time, maxInterval;
    float interval;

    /*
    Summary:
    Spike has a set timer when it activates and deactivates.
    When it activates, it set triggers of activate which will turn on box collider trigger.
    The time interval will also be reset to full and wait for the next activation time.
    */
    void Start() {
        interval = maxInterval;
    }

    void Update() {
        if (interval >= 0) {
            interval -= Time.deltaTime;
        } else {
            spriteAnimator.SetTrigger("Activate");
            Invoke("setMaxInterval", 1f); // can adjust this number
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController.p.TakeDamage(damage);
            PlayerController.p.TakeKnockback(new Vector2(Random.Range(-1,1), Random.Range(-1,1)).normalized, time);
        }
    }

    void setMaxInterval() {
        interval = maxInterval;
    }
}
