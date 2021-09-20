using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletTimeManager : MonoBehaviour
{
    public static UnityEvent OnAddBulletTime = new UnityEvent();
    public static UnityEvent OnSubtractBulletTime = new UnityEvent();
    public static UnityEvent OnBulletDie = new UnityEvent();
    public static UnityEvent OnBulletReset = new UnityEvent();
    [SerializeField] float startingBulletTime = 5.0f;
    [SerializeField] float respawnTime = 1.0f;
    static float _bulletTime = 0.0f;
    static float _highestBulletTime = 0.0f;

    bool _hasFired = false;
    private void Start()
    {
        void Fire()
        {
            _bulletTime = startingBulletTime;
            _highestBulletTime = _bulletTime;
            _hasFired = true;
        }
        void Die()
        {
            _hasFired = false;
        }
        PlayerController.OnFire.AddListener(Fire);
        OnBulletDie.AddListener(Die);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasFired)
            return;
        _bulletTime -= Time.deltaTime;
        if (_bulletTime <= 0.0f)
        {
            OnBulletDie.Invoke();
            IEnumerator Wait()
            {
                Debug.Log("Respawning");
                yield return new WaitForSeconds(respawnTime);
                OnBulletReset.Invoke();
            }
            StartCoroutine(Wait());
        }
    }

    public static void AddBulletTime(float time)
    {
        _bulletTime += time;
        if (_bulletTime > _highestBulletTime)
            _highestBulletTime = _bulletTime;
        OnAddBulletTime.Invoke();
    }
    public static void SubtractBulletTime(float time)
    {
        _bulletTime -= Mathf.Abs(time);
        OnSubtractBulletTime.Invoke();
    }

    public static float GetBulletTime()
    {
        return _bulletTime;
    }
    public static float GetMaxBulletTime()
    {
        return _highestBulletTime;
    }
}
