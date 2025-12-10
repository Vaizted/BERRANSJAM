using System.Collections.Generic;
using Mono.Cecil;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        }

        previousCamPos = cam.position;
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
        int loopsNeeded = LoopsNeeded(spriteWidth);

        float screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect / 2;

        for (int i = 0; i < loopsNeeded; i++)
        {
            Vector3 pos = layer.transform.position - new Vector3(screenWidth, 0, 0) + Vector3.right * spriteWidth * i;
            GameObject segment = Instantiate(layer.renderer.gameObject, pos - Vector3.left, Quaternion.identity, layer.transform);
            layer.ActiveSegments.Add(segment);
        }
    }

    private int LoopsNeeded(float spriteWidth) 
    {
        float screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
        float margin = spriteWidth * 2f;
        return Mathf.CeilToInt((screenWidth + margin) / spriteWidth);
    }
}

