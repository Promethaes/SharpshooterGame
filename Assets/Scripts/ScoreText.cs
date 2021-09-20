using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMPro.TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        text.text = ScoreManager.GetScore().ToString();
    }
}
