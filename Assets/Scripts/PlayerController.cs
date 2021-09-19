using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    public static UnityEvent OnFire = new UnityEvent();
    public static UnityEvent OnBulletDie = new UnityEvent();
    public static UnityEvent OnBulletReset = new UnityEvent();

    [SerializeField] float bulletSpeed = 10.0f;
    [Range(0.1f, 1.0f)]
    [SerializeField] float bulletTurnResistance;
    [SerializeField] float startingBulletTime = 5.0f;
    [SerializeField] float respawnTime = 1.0f;
    [Tooltip("References")]
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Crosshair crosshair;
    [SerializeField] Camera camera;
    [SerializeField] Transform startPoint;

    float _bulletTime = 0.0f;
    bool _hasFired = false;

    Vector2 _pointerPos = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        void FireBullet()
        {
            _hasFired = true;
            var direction = (crosshair.transform.position - transform.position).normalized;
            Debug.Log(direction*bulletSpeed);
            rigidbody.AddForce(direction * bulletSpeed, ForceMode.Impulse);
            _bulletTime = startingBulletTime;
        }
        void BulletReset()
        {
            transform.position = startPoint.position;
            _hasFired = false;
            rigidbody.velocity = Vector3.zero;
        }

        OnFire.AddListener(FireBullet);
        OnBulletReset.AddListener(BulletReset);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasFired || _bulletTime <= 0.0f)
            return;

        _bulletTime -= Time.deltaTime;
        if (_bulletTime <= 0.0f)
        {
            OnBulletDie.Invoke();
            IEnumerator Wait()
            {
                yield return new WaitForSeconds(respawnTime);
                OnBulletReset.Invoke();
            }
            StartCoroutine(Wait());
        }
    }

    public void OnPointerMove(CallbackContext ctx)
    {
        _pointerPos = ctx.ReadValue<Vector2>();
        _pointerPos = camera.ScreenToWorldPoint(_pointerPos);
    }

    public void Fire()
    {
        if (!_hasFired)
            OnFire.Invoke();
    }
}
