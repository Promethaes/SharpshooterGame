using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField] bool getHighScore = false;
    [Header("References")]
    [SerializeField] TMPro.TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        if (!getHighScore)
            text.text = ScoreManager.GetScore().ToString();
        else
            text.text = PlayerPrefs.GetFloat("Highscore",0.0f).ToString();
    }
}
