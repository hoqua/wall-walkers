using TMPro;
using UnityEngine;

public class DynamicOutline : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;
    [SerializeField] private Camera mainCamera;
    private float baseOutlineWidth = 0.2f;
    private float scaleFactor = 0.015f; // Чем больше, тем сильнее будет влиять расстояние

    void Update()
    {
        if (textMeshPro == null || mainCamera == null) return;

        float distance = Vector3.Distance(textMeshPro.transform.position, mainCamera.transform.position);
        textMeshPro.outlineWidth = baseOutlineWidth + (distance * scaleFactor);
    }
}