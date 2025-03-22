using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player instance;

    public bool isFire;
    public bool isIce;
    bool isSkillDoneTime;

    public Vector3 dir = Vector3.zero;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f; 
    bool isGrounded = true;
    [SerializeField] bool isCloud;
    Rigidbody2D rb;
    public SpriteRenderer spr;
    [SerializeField] GameObject originalCookie;

    public Cookie cookie;  
    public float throwForce = 10f;

    [Header("플레이어 정보")]
    [SerializeField] int nowHP;
    int cloudHP = 120;
    int seoilHP = 100;

    public int nowDMG;
    int cloudDMG = 10;
    int seoilDMG = 25;

    [SerializeField] int currentCookie = 0;


    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        dir.x = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Horizontal"))
        {
            spr.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        this.transform.position += dir * Time.deltaTime * speed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && currentCookie > 0)
        {
            Instantiate(originalCookie, this.transform.position, Quaternion.identity);
            currentCookie--;
            
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.E)) //스킬 테스트
        {
            StartCoroutine(SkillDone());
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(isCloud) //서일화
            {
                isCloud = false;
                spr.color = Color.red;

                cloudHP = nowHP;
                nowHP = seoilHP;
                nowDMG = seoilDMG;
            }
            else //구름화
            {
                isCloud = true;
                spr.color = Color.white;

                seoilHP = nowHP;
                nowHP = cloudHP;
                nowDMG = cloudDMG;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlusCookie"))
        {
            currentCookie++;
            Destroy(collision.gameObject);
        }
    }

    IEnumerator SkillNormal()
    {
        Instantiate(originalCookie, this.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.3f);
        Instantiate(originalCookie, this.transform.position, Quaternion.identity);
    }

    IEnumerator SkillNyam()
    {
        nowHP += 30;
        if (isCloud) //구름이라면
        {
            if(nowHP > 120)
            {
                nowHP = 120;
            }
        }
        else //서일이라면
        {
            if(nowHP > 100)
            {
                nowHP = 100;
            }
        }
        yield return null;
    }

    IEnumerator SkillFire() //적 화상 미구현
    {
        isFire = true;
        Instantiate(originalCookie, this.transform.position, Quaternion.identity);
        yield return null;
        isFire = false;
    }

    IEnumerator SkillIce() //적 얼음 미구현
    {
        isIce = true;
        Instantiate(originalCookie, this.transform.position, Quaternion.identity);
        yield return null;
        isIce = false;
    }

    IEnumerator SkillSleep()
    {
        nowDMG += 10;
        yield return new WaitForSecondsRealtime(5f);
        nowDMG -= 10;
    }

    //SkillBath 미구현

    IEnumerator SkillPrinces() //2초 무적 미구현
    {
        nowHP += 10;
        if (isCloud) //구름이라면
        {
            if (nowHP > 120)
            {
                nowHP = 120;
            }
        }
        else //서일이라면
        {
            if (nowHP > 100)
            {
                nowHP = 100;
            }
        }
        yield return null;
    }

    //SkillPrince 미구현

    IEnumerator SkillArt()
    {
        speed = 10f;
        yield return new WaitForSecondsRealtime(3f);
        speed = 5f;
    }

    IEnumerator SkillDone()
    {
        isSkillDoneTime = true;
        StartCoroutine(SkillDoneTime());
        while (isSkillDoneTime)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && currentCookie > 0)
            {
                yield return new WaitForSecondsRealtime(0.3f);
                Instantiate(originalCookie, this.transform.position, Quaternion.identity);
            }
            yield return null;
        }
    }

    IEnumerator SkillDoneTime()
    {
        yield return new WaitForSecondsRealtime(5f);
        isSkillDoneTime = false;
    }
}
