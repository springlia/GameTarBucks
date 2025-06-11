using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance2;

    [SerializeField] TextMeshProUGUI nowName;
    [SerializeField] TextMeshProUGUI nowHP;
    [SerializeField] TextMeshProUGUI nowDMG;

    [SerializeField] GameObject SkillUI;
 
    private void Awake()
    {
        if (instance2 == null)
        {
            instance2 = this;
        }
    }

    private void Update()
    {
        if (Player.instance.isCloud == true)
        {
            nowName.text = "Cloud";
        }
        else
        {
            nowName.text = "Seoil";
        }

        nowHP.text = "HP " + Player.instance.nowHP.ToString();
        nowDMG.text = "DMG " + Player.instance.nowDMG.ToString();
    }

    public void SkillUIOpen()
    {
        SkillUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void SkillUIClose()
    {
        SkillUI.SetActive(false);
        Time.timeScale = 1f;
    }
    
}
