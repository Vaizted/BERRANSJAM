using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform transform;
        public SpriteRenderer renderer;
        public float parallaxSpeed;

        private List<GameObject> activeSegments = new List<GameObject>();
        public List<GameObject> ActiveSegments => activeSegments;
        public float SpriteWidth => renderer.bounds.size.x;
    }

    public ParallaxLayer[] layers;
    private Transform cam;
    private Vector3 previousCamPos;

    void Start()
    {
        foreach (ParallaxLayer layer in layers)
        {
            InitializeLayer(layer);
        }

        cam = Camera.main.transform;
        previousCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - previousCamPos;

        foreach (ParallaxLayer layer in layers)
        {
            UpdatePosition(layer, deltaMovement);

            if (cam.position.x + layer.SpriteWidth > layer.ActiveSegments.Last().transform.position.x)
            {
                Vector3 pos = layer.ActiveSegments.Last().transform.position + new Vector3(layer.SpriteWidth, 0, 0);
                CreateSegment(layer, pos, layer.ActiveSegments.Count);
                RemoveSegment(layer, layer.ActiveSegments.First());
            }
            else if (cam.position.x - layer.SpriteWidth < layer.ActiveSegments.First().transform.position.x)
            {
                Vector3 pos = layer.ActiveSegments.First().transform.position - new Vector3(layer.SpriteWidth, 0, 0);
                CreateSegment(layer, pos, 0);
                RemoveSegment(layer, layer.ActiveSegments.Last());
            }
        }

        previousCamPos = cam.position;
    }

    private void CreateSegment(ParallaxLayer layer, Vector3 pos, int index)
    {
        GameObject segment = Instantiate(layer.renderer.gameObject, pos, Quaternion.identity, layer.transform);
        layer.ActiveSegments.Insert(index, segment);
    }

    private void RemoveSegment(ParallaxLayer layer, GameObject segment)
    {
        Destroy(segment);
        layer.ActiveSegments.Remove(segment);
    }


    private void UpdatePosition(ParallaxLayer layer, Vector3 deltaMovement)
    {
        Vector3 newPos = layer.transform.position;
        newPos += deltaMovement * layer.parallaxSpeed;
        layer.transform.position = newPos;
    }

    private void InitializeLayer(ParallaxLayer layer)
    {
        float spriteWidth = layer.SpriteWidth;
        float screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
        int loopsNeeded = LoopsNeeded(spriteWidth, screenWidth);
        float offsetX = loopsNeeded * spriteWidth / 2;

        for (int i = 0; i < loopsNeeded; i++)
        {
            Vector3 pos = layer.transform.position - new Vector3(offsetX - spriteWidth / 2, 0, 0) + Vector3.right * spriteWidth * i;
            CreateSegment(layer, pos, layer.ActiveSegments.Count);
        }
    }

    private int LoopsNeeded(float spriteWidth, float screenWidth)
    {
        float margin = spriteWidth * 2f;
        return Mathf.CeilToInt((screenWidth + margin) / spriteWidth);
    }
}

