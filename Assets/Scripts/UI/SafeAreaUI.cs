using UnityEngine;

public class SafeAreaUI : MonoBehaviour
{
    private RectTransform m_rectTransform;

    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();    
        ApplySafeArea();
    }

    void ApplySafeArea() {
        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        m_rectTransform.anchorMin = anchorMin;
        m_rectTransform.anchorMax = anchorMax;
    }
}
