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
        if (!Player.instance.spr.flipX) // �������� ���� ���� ��
        {
            isRight = true;
        }
        else // ������ ���� ���� ��
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
        if (isRight) // �������� ���� ���� ��
        {
            this.transform.position += Vector3.right * Time.deltaTime * speed;
        }
        else // ������ ���� ���� ��
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
