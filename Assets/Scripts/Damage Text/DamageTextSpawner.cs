using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;  // Префаб текста урона
    [SerializeField] private Transform canvasTransform;    // Canvas, в котором будет текст
    [SerializeField] private float verticalOffset = 25;    // Смещение текста по вертикали

    public void SpawnDamageText(Vector3 position, int damage)
    {
        if (damageTextPrefab == null || canvasTransform == null) return;
        
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        screenPosition.y += verticalOffset;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;
        
        GameObject damageText = Instantiate(damageTextPrefab, worldPosition, Quaternion.identity, canvasTransform);
        damageText.GetComponent<DamageText>().SetDamage(damage);
    }
}