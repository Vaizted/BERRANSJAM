using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float parallaxFactor;
    }

    public ParallaxLayer[] layers;
    private Transform cam;
    private Vector3 previousCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - previousCamPos;

        foreach (ParallaxLayer layer in layers)
        {
            Vector3 newPos = layer.layerTransform.position;
            newPos += deltaMovement * layer.parallaxFactor;
            layer.layerTransform.position = newPos;
        }

        previousCamPos = cam.position;
    }
}

