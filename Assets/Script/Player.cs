using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player instance;
    public Vector3 dir = Vector3.zero;

    [Header("��ų ����")]
    public bool isFire;
    public bool isIce;
    bool isSkillDoneTime;

    [Header("��Ű ����")]
    [SerializeField] GameObject originalCookie;
    public Cookie cookie;  
    public float throwForce = 10f;

    [Header("�÷��̾� ����")]
    public int nowHP;
    int cloudHP = 120;
    int seoilHP = 100;

    public int nowDMG;
    int cloudDMG = 10;
    int seoilDMG = 25;

    [SerializeField] int currentCookie = 0;

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    bool isGrounded = true;
    public bool isCloud;
    Rigidbody2D rb;
    public SpriteRenderer spr;

    public float score;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] TextMeshProUGUI ClearMsg;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        dir.x = Input.GetAxis("Horizontal"); //�̵�
        this.transform.position += dir * Time.deltaTime * speed;
        scoreText.text = score.ToString();

        if (Input.GetButtonDown("Horizontal")) //ĳ���� �¿����
        {
            spr.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && currentCookie > 0) //�⺻����
        {
            Instantiate(originalCookie, this.transform.position, Quaternion.identity);
            currentCookie--;
            
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded) //����
        {
            rb.AddForce(Vector3.up * jumpForce);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.E)) //��ų �׽�Ʈ
        {
            StartCoroutine(SkillDone());
        }

        if(Input.GetKeyDown(KeyCode.Q)) //���� ���� ��ȯ
        {
            if(isCloud) //����ȭ
            {
                isCloud = false;
                spr.color = Color.gray;

                cloudHP = nowHP;
                nowHP = seoilHP;
                nowDMG = seoilDMG;
            }
            else //����ȭ
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
        if (collision.gameObject.CompareTag("Ground")) //���� �ʱ�ȭ
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Enemy")) //���� ����
        {
            StartCoroutine(GetDamage());
            nowHP -= 10;
            Vector3 targetPos = collision.transform.position;
            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
            rb.AddForce(new Vector2(dirc, 1) * 5, ForceMode2D.Impulse);

            if (nowHP <= 0)
            {
                nowHP = 0;
                ClearMsg.text = "Fail ...".ToString();
                ClearMsg.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlusCookie")) //��Ű ����
        {
            currentCookie++;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Skill Store")) //��ų UI ����
        {
            GameManager.instance2.SkillUIOpen();
        }
        else if (collision.gameObject.CompareTag("Clear"))
        {
            ClearMsg.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else if (collision.gameObject.CompareTag("void"))
        {
            this.transform.position = new Vector3(0f, -3.18f, 0f);

            nowHP -= 10;
        }
    }
    IEnumerator SkillNormal()
    {
        Instantiate(originalCookie, this.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.3f);
        Instantiate(originalCookie, this.transform.position, Quaternion.identity);
    }

    IEnumerator GetDamage()
    {
        spr.color = Color.red;
        yield return new WaitForSecondsRealtime(0.3f);
        if (isCloud)
            spr.color = Color.white;
        else
            spr.color = Color.gray;
    }

    IEnumerator SkillNyam()
    {
        nowHP += 30;
        if (isCloud) //�����̶��
        {
            if(nowHP > 120)
            {
                nowHP = 120;
            }
        }
        else //�����̶��
        {
            if(nowHP > 100)
            {
                nowHP = 100;
            }
        }
        yield return null;
    }

    IEnumerator SkillFire() //�� ȭ�� �̱���
    {
        isFire = true;
        Instantiate(originalCookie, this.transform.position, Quaternion.identity);
        yield return null;
        isFire = false;
    }

    IEnumerator SkillIce() //�� ���� �̱���
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

    //SkillBath �̱���

    IEnumerator SkillPrinces() //2�� ���� �̱���
    {
        nowHP += 10;
        if (isCloud) //�����̶��
        {
            if (nowHP > 120)
            {
                nowHP = 120;
            }
        }
        else //�����̶��
        {
            if (nowHP > 100)
            {
                nowHP = 100;
            }
        }
        yield return null;
    }

    //SkillPrince �̱���

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
