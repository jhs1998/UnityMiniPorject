using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScrept : MonoBehaviour
{
    public void ButtonClick()
    {
        SceneManager.LoadScene("BattleMapScene");
    }
}
