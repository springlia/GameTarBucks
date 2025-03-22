using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    public SpriteRenderer spr;

    [SerializeField] float speed;
    [SerializeField] float destoyTime;

    public bool isRight;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(Destroythis());
        if (!Player.instance.spr.flipX) // 오른쪽을 보고 있을 때
        {
            isRight = true;
        }
        else // 왼쪽을 보고 있을 때
        {
            isRight = false;
        }
        if(Player.instance.isFire)
        {
            spr.color = Color.red;
        }
        else if (Player.instance.isIce)
        {
            spr.color = Color.blue;
        }
    }

    private void Update()
    {
        if (isRight) // 오른쪽을 보고 있을 때
        {
            this.transform.position += Vector3.right * Time.deltaTime * speed;
        }
        else // 왼쪽을 보고 있을 때
        {
            this.transform.position += Vector3.left * Time.deltaTime * speed;
        }
    }

    IEnumerator Destroythis()
    {
        yield return new WaitForSeconds(destoyTime);
        Destroy(gameObject);
    }
}
