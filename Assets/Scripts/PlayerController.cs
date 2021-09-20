using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    public static UnityEvent OnFire = new UnityEvent();


    [SerializeField] float bulletSpeed = 10.0f;
    [Range(0.1f, 1.0f)]
    [SerializeField] float bulletTurnResistance = 0.1f;
    [Tooltip("References")]
    [SerializeField] Rigidbody2D rigidbody = null;
    [SerializeField] Crosshair crosshair = null;
    [SerializeField] Camera camera = null;
    [SerializeField] Transform startPoint = null;

    bool _hasFired = false;

    Vector2 _pointerPos = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        void FireBullet()
        {
            _hasFired = true;
            var direction = (crosshair.transform.position - transform.position).normalized;
            rigidbody.AddForce(direction * bulletSpeed,ForceMode2D.Impulse);
        }
        void BulletDie()
        {
            rigidbody.velocity = Vector3.zero;
        }
        void BulletReset()
        {
            transform.position = startPoint.position;
            _hasFired = false;
        }

        OnFire.AddListener(FireBullet);
        BulletTimeManager.OnBulletReset.AddListener(BulletReset);
        BulletTimeManager.OnBulletDie.AddListener(BulletDie);
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
