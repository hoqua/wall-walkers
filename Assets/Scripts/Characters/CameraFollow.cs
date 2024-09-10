using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Ссылка на персонажа, за которым нужно следить
    public Vector3 offset; // Смещение камеры от персонажа
    public float speed = 0.01f; // Скорость премещения камеры
    private const float FixedZ = -10; // Фиксированное значение Z для камеры

    public float checkInterval = 0.10f; // Интервал проверки на наличие персонажа для слежки
    private bool _targetFound;
    
    void Start()
    {
        StartCoroutine(FindTarget());
    }

    private System.Collections.IEnumerator FindTarget()
    {
        while (!_targetFound) 
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                target = player.transform;
                _targetFound = true;
                Debug.Log("Player found, camera is now following.");
            }
            else
            {
                Debug.Log("Player not found, trying again...");
            }
            
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed);
            smoothedPosition.z = FixedZ;
            
            transform.position = smoothedPosition;
        }
    }
}
