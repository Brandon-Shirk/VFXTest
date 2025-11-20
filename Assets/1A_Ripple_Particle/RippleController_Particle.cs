using UnityEngine;

public class RippleController_Particle : MonoBehaviour
{
    public RectTransform rippleTransform;
    public ParticleSystem particles;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            rippleTransform.anchoredPosition = screenPos;

            particles.Clear();
            particles.Emit(1);
        }
    }
}
