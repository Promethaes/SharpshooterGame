using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource fireSound;
    [SerializeField] AudioSource collectSound;
    [SerializeField] AudioSource crashSound;
    // Start is called before the first frame update
    void Start()
    {
        void BulletDie()
        {
            deathSound.PlayOneShot(deathSound.clip);
        }
        void Fire()
        {
            fireSound.PlayOneShot(fireSound.clip);
        }
        void HitTarget()
        {
            collectSound.PlayOneShot(collectSound.clip);
        }
        void RicochetSound()
        {
            crashSound.PlayOneShot(crashSound.clip);
        }
        BulletTimeManager.OnBulletDie.AddListener(BulletDie);
        PlayerController.OnFire.AddListener(Fire);
        Enemy.OnKillEnemy.AddListener(HitTarget);
        Ricochet.OnAnyRicochet.AddListener(RicochetSound);
    }
}
