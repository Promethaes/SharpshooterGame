using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ricochet : MonoBehaviour
{
    [SerializeField] float scoreValue = 0.0f;
    [SerializeField] float timeValue = 0.0f;
    [SerializeField] UnityEvent RicochetEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
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

        //reflect velocity
        var rigid = other.gameObject.GetComponent<Rigidbody>();
        var rigidVel = rigid.velocity;
        var direction = (other.transform.position - transform.position).normalized;
        if (Mathf.Abs(direction.x) > 0.5f)
            rigidVel.x = -rigidVel.x;
        if (Mathf.Abs(direction.y) > 0.5f)
            rigidVel.y = -rigidVel.y;

        rigid.velocity = rigidVel;

        RicochetEvent.Invoke();
    }
}
