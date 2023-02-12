using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController p;

    private Rigidbody2D rb2d;
    public Transform pivotPoint;
    public Animator playerBodyAnimator;
    
    public GameObject head;
    public GameObject torso;
    public Transform[] rotatePoints;

    public float maxHP = 5f;
    float originalMaxHP;
    float curHP = 5f;
    public float invincibilityTime = .3f;
    float canTakeDamage = 0f;
    public Image playerHPBar;
    public TextMeshProUGUI curHPText, maxHPText, dashReadyText;
    public Image playerStaminaBar;

    public float speed = 5f;
    float originalSpeed;
    public float dashTime = .3f;
    public float dashLeft = 0f;
    public float dashCDTime = .5f;
    float dashCDLeft = 0f;
    public DashTrail[] dashTrails;
    public float knockBackTimeLeft = 0f;

    float horizontal;
    float vertical;

    public bool attacking = false;
    public Animator attackAnim;
    public float attackSpeed = 1;
    float originalAttackSpeed;
    public float attackCD = .5f;
    float originalCD;
    public float attackCDLeft = 0f;
    public float attackDamage;
    float originalDamage;
    public float attackKB;

    public bool disabled = false;

    // Start is called before the first frame update
    void Start()
    {
        p = this;
        rb2d = GetComponent<Rigidbody2D>();
        curHP = maxHP;
        playerHPBar.fillAmount = curHP / maxHP;
        curHPText.text = "" + curHP;
        maxHPText.text = "" + maxHP;

        originalMaxHP = maxHP;
        originalSpeed = speed;
        originalAttackSpeed = attackSpeed;
        originalCD = attackCD;
        originalDamage = attackDamage;
    }

    public void SetStats()
    {
        ArenaManager a = ArenaManager.Instance;
        float increment = originalMaxHP * a.healthMultiplier - maxHP;
        IncreaseMaxHealth(increment);
        speed = originalSpeed * a.dashSpeedMultiplier;
        attackSpeed = originalAttackSpeed * a.attackSpeedMultiplier;
        attackCD = originalCD * a.attackCDMultiplier;
        if(attackCD < 0)
        {
            attackCD = 0;
        }
        attackDamage = originalDamage * a.damageMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (dashCDLeft > 0f)
        {
            dashCDLeft -= Time.deltaTime;
        }
        if (canTakeDamage > 0)
        {
            canTakeDamage -= Time.deltaTime;
        }
        if (!disabled)
        {
            if (!attacking)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pivotPoint.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

                if (mousePos.x < (rotatePoints[0].position).x)
                {
                    head.transform.localScale = new Vector3(-1 * Mathf.Abs(head.transform.localScale.x), head.transform.localScale.y, head.transform.localScale.z);
                    pivotPoint.localScale = new Vector3(-1 * Mathf.Abs(pivotPoint.transform.localScale.x), pivotPoint.transform.localScale.y, pivotPoint.transform.localScale.z);
                }
                else if (mousePos.x > (rotatePoints[1].position).x)
                {
                    head.transform.localScale = new Vector3(Mathf.Abs(head.transform.localScale.x), head.transform.localScale.y, head.transform.localScale.z);
                    pivotPoint.localScale = new Vector3(Mathf.Abs(pivotPoint.transform.localScale.x), pivotPoint.transform.localScale.y, pivotPoint.transform.localScale.z);
                }
            }

            if (knockBackTimeLeft <= 0 && dashLeft <= 0)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
                rb2d.velocity = new Vector2(horizontal, vertical).normalized * speed;
            }
            else
            {
                dashLeft -= Time.deltaTime;
                knockBackTimeLeft -= Time.deltaTime;
                rb2d.velocity = new Vector2(horizontal, vertical).normalized * speed * 2;
            }

            if (knockBackTimeLeft <= 0 && dashCDLeft <= 0f && (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0) && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1)))
            {
                dashLeft = dashTime;
                dashCDLeft = dashCDTime;
                StartCoroutine(handleDashTrails(dashTime));
            }

            playerStaminaBar.fillAmount = (dashCDTime - dashCDLeft) / dashCDTime;
            dashReadyText.alpha = playerStaminaBar.fillAmount;
            if (Input.GetMouseButtonDown(0) && !attacking && attackCDLeft <= 0)
            {
                StartCoroutine(Attack());
            }
            if (attackCDLeft > 0)
            {
                attackCDLeft -= Time.deltaTime;
            }
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
        playerBodyAnimator.SetBool("Moving", !Mathf.Approximately(rb2d.velocity.magnitude, 0));
        if(horizontal < 0)
        {
            torso.transform.localScale = new Vector3(-1 * Mathf.Abs(torso.transform.localScale.x), torso.transform.localScale.y, torso.transform.localScale.z);
        } else if (horizontal > 0)
        {
            torso.transform.localScale = new Vector3(Mathf.Abs(torso.transform.localScale.x), torso.transform.localScale.y, torso.transform.localScale.z);
        }
        
    }
    IEnumerator handleDashTrails(float time)
    {
        foreach(DashTrail d in dashTrails)
        {
            d.mbEnabled = true;
        }
        yield return new WaitForSeconds(time);
        foreach (DashTrail d in dashTrails)
        {
            d.mbEnabled = false;
        }
    }

    IEnumerator Attack()
    {
        
        attackCDLeft = attackCD;
        if (dashLeft > 0)
        {
            attacking = true;
            attackAnim.SetTrigger("DashAttack");
            yield return new WaitUntil(() => attackAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_DashAttack"));
            yield return new WaitUntil(() => attackAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            attacking = false;
        }
        else
        {
            attacking = true;
            attackAnim.SetFloat("AttackSpeed", attackSpeed);
            attackAnim.SetTrigger("Attack");
            yield return new WaitUntil(() => attackAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_RegAttack"));
            yield return new WaitUntil(() => attackAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            attacking = false;
        }
    }

    public void IncreaseMaxHealth(float increment)
    {
        maxHP = maxHP + increment;
        if(maxHP >= 9999)
        {
            maxHP = 9999;
        }
        curHP = maxHP;
        playerHPBar.fillAmount = curHP / maxHP;
        curHPText.text = "" + curHP;
        maxHPText.text = "" + maxHP;
    }

    public void TakeKnockback(Vector2 dir, float time)
    {
        if (dashLeft <= 0)
        {
            knockBackTimeLeft = time;
            horizontal = dir.x;
            vertical = dir.y;
            rb2d.velocity = dir;
        }
        
    }

    public void TakeDamage(float amount)
    {
        if (curHP > 0 && dashLeft <= 0)
        {
            curHP -= amount;
            if(curHP < 0)
            {
                curHP = 0;
            }
            canTakeDamage = invincibilityTime;
            playerBodyAnimator.SetTrigger("Damage");
            playerHPBar.fillAmount = curHP / maxHP;
            curHPText.text = "" + curHP;
            maxHPText.text = "" + maxHP;
        } 
        
        if(curHP <= 0)
        {
            Die();
        }
        
    }

    public void Die()
    {
        //do something
    }
}
