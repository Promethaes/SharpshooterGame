using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletVisuals : MonoBehaviour
{
    [Tooltip("References")]
    [SerializeField] Slider bulletTimeBar;
    [SerializeField] Image bulletTimeBarImage;
    [SerializeField] SpriteRenderer bulletSpriteRenderer;

    Coroutine UpdateFill = null;
    private void Start()
    {
        bulletSpriteRenderer.enabled = false;
        bulletTimeBar.gameObject.SetActive(false);
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
            StopCoroutine(UpdateFill);
        }
        void ResetBullet()
        {
            var col = bulletSpriteRenderer.color;
            col.a = 1.0f;
            bulletSpriteRenderer.color = col;
            bulletSpriteRenderer.enabled = false;
            bulletTimeBar.gameObject.SetActive(false);

        }
        void Fire()
        {

            bulletSpriteRenderer.enabled = true;
            bulletTimeBar.gameObject.SetActive(true);
            IEnumerator Fill()
            {
                while (true)
                {
                    yield return new WaitForEndOfFrame();
                    bulletTimeBar.value = BulletTimeManager.GetBulletTime() / BulletTimeManager.GetMaxBulletTime();
                    bulletTimeBarImage.color = Color.Lerp(Color.red, Color.green, bulletTimeBar.value);
                    var col = bulletTimeBarImage.color;
                    col.a = 1.0f;
                }
            }
            UpdateFill = StartCoroutine(Fill());
        }


        PlayerController.OnFire.AddListener(Fire);
        BulletTimeManager.OnBulletDie.AddListener(Die);
        BulletTimeManager.OnBulletReset.AddListener(ResetBullet);
    }
}
