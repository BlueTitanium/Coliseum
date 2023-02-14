using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBomb : MonoBehaviour
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

    public void explodeBomb()
    {
        GetComponent<Animator>().SetTrigger("Explode");
    }

    public void ExplodeEffect()
    {
        CameraShake.cs.cameraShake(.3f, 2.5f);
    }

    public void Die()
    {
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            explodeBomb();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
