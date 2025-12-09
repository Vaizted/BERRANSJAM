using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helper
{
    private static Camera camera;

    public static Camera Camera
    {
        get
        { 
            if (camera == null) { camera = Camera.main; }
            return camera; 
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> m_WaitDictionary = new Dictionary<float, WaitForSeconds>();
    /// <summary>
    /// Is storing waitforseconds, can take up alot of garbage space
    /// </summary>
    public static WaitForSeconds GetWait(float time)
    {
        if(m_WaitDictionary.TryGetValue(time, out var wait)) return wait;

        m_WaitDictionary[time] = new WaitForSeconds(time);
        return m_WaitDictionary[time];
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _result;
    /// <summary>
    /// Cheking if input is over ui
    /// </summary>
    public static bool IsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.GetTouch(0).position };
        _result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _result);
        return _result.Count > 0;
    }

    /// <summary>
    /// Returns world position from canvas position
    /// </summary>
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }

    /// <summary>
    /// Deletes all children under a transform
    /// </summary>
    public static void DeleteChildren(this Transform t)
    {
        foreach (Transform child in t)
        {
            Object.Destroy(child.gameObject);
        }
    }
}
