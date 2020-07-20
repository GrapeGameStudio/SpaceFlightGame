using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public Text victoryTxt;
    public GameObject panel;
    public bool isVictory;
    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        if (SpaceCraft._instance.isDie)
        {
            panel.SetActive(true);
            victoryTxt.text = "游戏失败";
            Time.timeScale = 0;
        }
        if (!SpaceCraft._instance.isDie&&isVictory)
        {
            panel.SetActive(true);
            victoryTxt.text = "游戏成功";
            Time.timeScale = 0;
        }
    }
}
