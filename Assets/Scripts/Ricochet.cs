﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ricochet : MonoBehaviour
{
    [SerializeField] float scoreValue = 0.0f;
    [SerializeField] float timeValue = 0.0f;
    [SerializeField] UnityEvent RicochetEvent;
    

    bool _hitCooldown = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || BulletTimeManager.GetBulletTime() < 0.0f || _hitCooldown)
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

        //reflect velocity
        var rigid = other.gameObject.GetComponent<Rigidbody2D>();
        var vel = rigid.velocity;

        List<Vector3> sides = new List<Vector3>();
        sides.Add(new Vector3(other.bounds.min.x, other.bounds.min.y));
        sides.Add(new Vector3(other.bounds.max.x, other.bounds.max.x));
        sides.Add(new Vector3(other.bounds.min.x, other.bounds.max.y));
        sides.Add(new Vector3(other.bounds.max.x, other.bounds.min.y));

        rigid.velocity = -vel;

        RicochetEvent.Invoke();
    }
}
