using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    //加载场景
    public void LoadScene(int number)
    {
        SceneManager.LoadScene(number);       
    }

    //退出游戏
    public void ExitGame()
    {
        Application.Quit();
    }
}
