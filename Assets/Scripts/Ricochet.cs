using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ricochet : MonoBehaviour
{
    [SerializeField] float scoreValue = 0.0f;
    [SerializeField] float timeValue = 0.0f;
    [SerializeField] UnityEvent RicochetEvent;


    bool _hitCooldown = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") || BulletTimeManager.GetBulletTime() < 0.0f || _hitCooldown)
            return;
        _hitCooldown = true;
        IEnumerator HitCooldown()
        {
            yield return new WaitForSeconds(0.25f);
            _hitCooldown = false;
        }
        StartCoroutine(HitCooldown());

        if (timeValue != 0.0f)
        {
            if (timeValue > 0.0f)
                BulletTimeManager.AddBulletTime(timeValue);
            else
                BulletTimeManager.SubtractBulletTime(timeValue);
        }
        if (scoreValue != 0.0f)
        {
            ScoreManager.AddScore(scoreValue);
        }

        RicochetEvent.Invoke();
    }
}
