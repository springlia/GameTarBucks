using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHP = 50;
    [SerializeField] int currentHP;
    [SerializeField] TextMeshProUGUI dmgText;
    [SerializeField] Transform canvusPos;
    TextMeshProUGUI temp;
    [SerializeField] SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        currentHP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cookie"))
        {
            StartCoroutine(AttackEnemy());
            Destroy(collision.gameObject); //ÄíÅ° Á¦°Å
            currentHP -= Player.instance.nowDMG;
        }
        else if (collision.gameObject.CompareTag("Enemy Move Limit"))
        {
            if (spr.flipX == false)
            {
                spr.flipX = true;
            } 
            else
            {
                spr.flipX = false;
            }
                
        }

    }
    private void Update()
    {
        if (spr.flipX == false)
        {
            this.transform.position += Vector3.left * Time.deltaTime * 2f;
        }
        else
        {
            this.transform.position += Vector3.right * Time.deltaTime * 2f;
        }
        
        if (currentHP <= 0)
        {
            Player.instance.score += 1;
            Destroy(gameObject);
        }
    }
    IEnumerator AttackEnemy()
    {
        spr.color = Color.red;
        yield return new WaitForSecondsRealtime(0.3f);
        spr.color = Color.white;
    }

}
