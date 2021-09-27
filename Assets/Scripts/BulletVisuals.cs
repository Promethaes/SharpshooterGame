using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletVisuals : MonoBehaviour
{
    [Tooltip("References")]
    [SerializeField] Slider bulletTimeBar = null;
    [SerializeField] Image bulletTimeBarImage = null;
    [SerializeField] SpriteRenderer bulletSpriteRenderer = null;
    [SerializeField] TrailRenderer bulletTrail = null;

    Coroutine _updateFillAndTrail = null;

    Color _originalTrailColor = Color.blue;
    private void Start()
    {
        bulletSpriteRenderer.enabled = false;
        bulletTimeBar.gameObject.SetActive(false);
        _originalTrailColor = bulletTrail.startColor;
        void Die()
        {
            IEnumerator Fade()
            {
                float x = 0.0f;
                while (x < 1.0f)
                {
                    yield return new WaitForEndOfFrame();
                    x += Time.deltaTime;
                    var col = bulletSpriteRenderer.color;
                    col.a = Mathf.Lerp(1.0f, 0.0f, x);
                    bulletSpriteRenderer.color = col;
                }
            }
            StartCoroutine(Fade());
            StopCoroutine(_updateFillAndTrail);
        }
        void ResetBullet()
        {
            var col = bulletSpriteRenderer.color;
            col.a = 1.0f;
            bulletSpriteRenderer.color = col;
            bulletSpriteRenderer.enabled = false;
            bulletTimeBar.gameObject.SetActive(false);
            bulletTrail.startColor = _originalTrailColor;

        }
        void Fire()
        {

            bulletSpriteRenderer.enabled = true;
            var temp = bulletSpriteRenderer.color;
            temp.a = 1.0f;
            bulletSpriteRenderer.color = temp;
            bulletTimeBar.gameObject.SetActive(true);
            IEnumerator FillAndTrail()
            {
                while (true)
                {
                    yield return new WaitForEndOfFrame();
                    var u = bulletTimeBar.value = BulletTimeManager.GetBulletTime() / BulletTimeManager.GetMaxBulletTime();
                    bulletTimeBarImage.color = Color.Lerp(Color.red, Color.green, u);
                    bulletTrail.startColor = Color.Lerp(Color.red, _originalTrailColor, u);

                    var col = bulletTimeBarImage.color;
                    col.a = 1.0f;
                    bulletTimeBarImage.color = col;

                    col = bulletTrail.startColor;
                    col.a = 1.0f;
                    bulletTrail.startColor = col;
                }
            }
            _updateFillAndTrail = StartCoroutine(FillAndTrail());
        }


        PlayerController.OnFire.AddListener(Fire);
        BulletTimeManager.OnBulletDie.AddListener(Die);
        BulletTimeManager.OnBulletReset.AddListener(ResetBullet);
    }
}
