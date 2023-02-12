using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{

    public float maxHP, curHP;

    [SerializeField]
    public float knockback, timer, maxTimer, damage, invuln, range;

    GameObject player;
    public float speed;
    public Rigidbody2D enemyRb;
    public Collider2D enemyCol;
    public Vector2 direction;

    [SerializeField]
    float distance;

    public Animator spriteAnimator;
    public Animation sword;
    public Transform pivotPoint;
    public bool hasHit = false;
    public GameObject explosionPrefab;
    public bool disabled = false;
    void Start() {
        player = PlayerController.p.transform.gameObject;
        curHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = PlayerController.p.transform.gameObject;
        }
        if (!PlayerController.p.disabled && !disabled)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            direction = player.transform.position - transform.position;
            if(direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            } else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            pivotPoint.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);
            if (distance > range && invuln <= 0)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                spriteAnimator.SetBool("Moving", true);
            }
            else if (invuln <= 0)
            {
                spriteAnimator.SetBool("Moving", false);
                if (timer <= 0)
                {

                    //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                    //DO ATTACK HERE
                    hasHit = false;
                    sword.Play();
                    timer = maxTimer;
                    //enemyCol.isTrigger = true;
                }
            }
        }
        else
        {
            spriteAnimator.SetBool("Moving", false);
        }
        


        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (invuln > 0) {
            invuln -= Time.deltaTime;
        }
        if(invuln <= 0)
        {
            enemyRb.velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        //if (other.gameObject.CompareTag("Player") && timer <= 0) {
        //    PlayerController.p.TakeKnockback(direction, knockback);
        //    PlayerController.p.TakeDamage(damage);
        //    timer = maxTimer;
        //}
    }

    public void Knockback(float kb, float iframes) {
        if (invuln <= 0) {
            enemyRb.velocity = -direction.normalized * kb;
            timer += 1f;
            invuln = iframes;
        }
    }

    public void TakeDamage(float damage)
    {
        if(invuln <= 0)
        {
            curHP -= damage;
            damageNumberSpawner.Instance(transform.position, damage);
            spriteAnimator.SetTrigger("Damage");
        }
        if(curHP < 0)
        {
            Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
            Destroy(gameObject);
        }
    }
    public void KillSelf()
    {
        Destroy(gameObject);
    }
}
