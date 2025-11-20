using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class RippleController_RT : MonoBehaviour, IPointerDownHandler
{
    public RenderTexture sceneTex;
    public ParticleSystem explosionParticles;
    public RectTransform explosionTransform;
    public int burstCount;
    public Material rippleMat;

    public float rippleStart = -.1f;
    public float rippleEnd = 3.0f;
    public float rippleDuration = 1.0f;

    private int lastWidth;
    private int lastHeight;

    public Camera sceneCam;

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayRippleEffect(eventData.position);
    }

    private void PlayRippleEffect(Vector2 clickPos)
    {
        // Activate particle burst
        explosionParticles.Clear();
        explosionParticles.gameObject.SetActive(true);
        explosionTransform.anchoredPosition = clickPos;
        explosionParticles.Emit(burstCount);

        // Get click position in UV space
        float x = clickPos.x / lastWidth;
        float y = clickPos.y / lastHeight;

        // Set starting shader values
        rippleMat.SetVector("_SphereCenter", new Vector2(x, y));
        rippleMat.SetFloat("_TimeProgress", rippleStart);

        // Start the animation
        StopAllCoroutines();
        StartCoroutine(SetRippleValues());
    }

    private IEnumerator SetRippleValues()
    {
        float elapsed = 0;
        while (elapsed < rippleDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rippleDuration;
            float value = Mathf.Lerp(rippleStart, rippleEnd, t);

            rippleMat.SetFloat("_TimeProgress", value);
            yield return null;
        }
        rippleMat.SetFloat("_TimeProgress", rippleStart);
        explosionParticles.gameObject.SetActive(false);
    }

    void Start()
    {
        UpdateRenderTexture();
        rippleMat.SetFloat("_TimeProgress", rippleStart);
    }

    void Update()
    {
        // Watch screen resolution for changes
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateRenderTexture();
        }
    }

    // Resize the render texture to match screen resolution
    private void UpdateRenderTexture()
    {
        int width = Screen.width;
        int height = Screen.height;

        lastWidth = width;
        lastHeight = height;

        sceneTex.Release();
        sceneTex.width = width;
        sceneTex.height = height;
        sceneCam.ResetAspect();
    }
}
