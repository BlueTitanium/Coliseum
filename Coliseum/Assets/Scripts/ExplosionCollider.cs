using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCollider : MonoBehaviour
{
    public float damage = 20;
    public float knockback = 3;
    // Start is called before the first frame update
    void Start()
    {
        if (ArenaManager.Instance != null)
        {
            damage *= ArenaManager.Instance.enemyDamageMultiplier;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var direction = collision.transform.position - transform.position;
            PlayerController.p.TakeKnockback(direction.normalized * knockback, .3f);
            PlayerController.p.TakeDamage(damage);
        }
    }
}
