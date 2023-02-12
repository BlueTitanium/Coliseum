using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float maxHP, curHP;

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

    public Animator spriteAnimator;
    public Animation wand;
    public Transform pivotPoint;
    public Transform shootPoint;
    public bool hasHit = false;
    public GameObject explosionPrefab;
    public bool disabled = false;
    void Start() {
        player = FindObjectOfType<PlayerController>().gameObject;
        if(ArenaManager.Instance != null)
        {
            maxHP *= ArenaManager.Instance.enemyHealthMultiplier;
        }
        curHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

        
        if (!PlayerController.p.disabled && !disabled)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            direction = player.transform.position - transform.position;
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            pivotPoint.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);
            if (distance > range && invuln <= 0)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                spriteAnimator.SetBool("Moving", true);
            }
            else if(invuln <= 0)
            {
                enemyRb.velocity = Vector2.zero;
                spriteAnimator.SetBool("Moving", false);
                if (timer <= 0 && !wand.isPlaying)
                {
                    StartCoroutine(ShootProjectile());
                    timer = maxTimer;
                }
            }
        }
        else
        {
            spriteAnimator.SetBool("Moving", false);
        }

        if (timer > 0) {
            timer -= Time.deltaTime;
        }

        if (invuln > 0) {
            invuln -= Time.deltaTime;
        }
        if (invuln <= 0)
        {
            enemyRb.velocity = Vector2.zero;
        }
    }
    IEnumerator ShootProjectile()
    {
        wand.Play();
        yield return new WaitForSeconds(.5f);
        GameObject newProjectile = Instantiate(testProjectile, shootPoint.position, Quaternion.identity);
        newProjectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);
        newProjectile.GetComponent<Projectile>().direction = direction;
        newProjectile.GetComponent<Rigidbody2D>().AddForce(direction * new Vector2(projectileSpeed, projectileSpeed));
    }
    public void Knockback(float kb, float iframes)
    {
        if (invuln <= 0)
        {
            enemyRb.velocity = -direction.normalized * kb;
            timer += 1f;
            invuln = iframes;
        }
    }

    public void TakeDamage(float damage)
    {
        if (invuln <= 0)
        {
            DamageNumberSpawner.Instance(transform.position, damage);
            curHP -= damage;
            spriteAnimator.SetTrigger("Damage");
        }
        if (curHP < 0)
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
