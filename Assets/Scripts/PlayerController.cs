using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    public static UnityEvent OnFire = new UnityEvent();


    [SerializeField] float bulletSpeed = 10.0f;
    [Range(1.0f, 10.0f)]
    [SerializeField] float bulletTurnForceScalar = 0.1f;
    [Tooltip("References")]
    [SerializeField] Rigidbody2D rigidbody = null;
    [SerializeField] Crosshair crosshair = null;
    [SerializeField] Camera camera = null;
    [SerializeField] Transform startPoint = null;

    bool _hasFired = false;
    bool _canSteer = false;

    Vector2 _pointerPos = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        void FireBullet()
        {
            _hasFired = true;
            _canSteer = true;
            var direction = (crosshair.transform.position - transform.position).normalized;
            rigidbody.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
        }
        void BulletDie()
        {
            rigidbody.velocity = Vector3.zero;
            _canSteer = false;
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

    private void FixedUpdate()
    {
        if (!_canSteer)
            return;
        var direction = _pointerPos - (Vector2)transform.position;
        direction = direction.normalized;
        var vel = rigidbody.velocity;
        vel = Vector3.RotateTowards(vel,direction,Time.deltaTime*bulletTurnForceScalar,0.0f);
        rigidbody.velocity = vel;
        //rigidbody.AddForce(direction * bulletTurnForceScalar);
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
