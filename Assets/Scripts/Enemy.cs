using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public static UnityEvent OnKillEnemy = new UnityEvent();
    [SerializeField] float scoreValue = 50.0f;
    static float _scoreMultiplier = 1.0f;

    private void Start()
    {
        void BulletReset()
        {
            gameObject.SetActive(true);
            _scoreMultiplier = 1.0f;
        }
        BulletTimeManager.OnBulletReset.AddListener(BulletReset);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        ScoreManager.AddScore(scoreValue * _scoreMultiplier);
        _scoreMultiplier += 1.0f;
        gameObject.SetActive(false);
        OnKillEnemy.Invoke();
    }
}
