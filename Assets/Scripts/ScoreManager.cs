using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static UnityEvent OnAddScore = new UnityEvent();
    static float _score = 0.0f;
    [SerializeField] float scorePerSecond = 1.0f;
    [Range(0.01f, 1.0f)]
    [SerializeField] float scoreTickRate = 1.0f;

    Coroutine Tick = null;
    private void Start()
    {
        _score = 0.0f;
        void Fire()
        {
            IEnumerator TickScore()
            {
                while (true)
                {
                    yield return new WaitForSeconds(scoreTickRate);
                    _score += scorePerSecond;
                }
            }
            Tick = StartCoroutine(TickScore());
        }
        void BulletReset()
        {
            StopCoroutine(Tick);
            SaveScore();
            _score = 0.0f;
        }
        PlayerController.OnFire.AddListener(Fire);
        BulletTimeManager.OnBulletReset.AddListener(BulletReset);
    }

    public static void AddScore(float score)
    {
        _score += score;
        OnAddScore.Invoke();
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    void SaveScore()
    {
        float hs = PlayerPrefs.GetFloat("Highscore", -1.0f);
        if (hs != -1.0f && _score > hs)
        {
            PlayerPrefs.SetFloat("Highscore", _score);
            PlayerPrefs.Save();
        }
    }

    public static float GetScore()
    {
        return _score;
    }
}
