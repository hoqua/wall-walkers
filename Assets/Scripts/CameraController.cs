using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _target;                   // Ссылка на персонажа, за которым нужно следить
    public Vector3 offset;                       // Смещение камеры от персонажа
    public float followSpeed = 0.01f;            // Скорость перемещения камеры при следовании за игроком
    public float cameraMoveSpeed = 5f;           // Скорость перемещения камеры при свободном движении
    public float maxDistanceFromPlayer = 10f;    // Максимальное расстояние, на которое камера может отойти от игрока
    private const float FixedZ = -10f;           // Фиксированное значение Z для камеры

    private bool isDetached = false;             // Флаг для проверки, отсоединена ли камера
    private Vector3 originalCameraPosition;      // Исходная позиция камеры перед отсоединением

    void Start()
    {
        FindPlayer(); // Пытаемся найти игрока при старте
    }

    void LateUpdate()
    {
        if (_target == null)
        {
            FindPlayer();
            return;
        }

        
        if (Input.GetMouseButton(1))
        {
            if (!isDetached)
            {
                DetachCamera(); 
            }

            FreeMoveCamera(); 
        }
        else 
        {
            if (isDetached)
            {
                ReturnCameraToPlayer(); 
            }
            else
            {
                FollowPlayer(); 
            }
        }
        
        FixZPosition();
    }
    

    private void FollowPlayer()
    {
        var desiredPosition = _target.position + offset;
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed);
        transform.position = smoothedPosition;
    }
    
    private void DetachCamera()
    {
        isDetached = true;
        originalCameraPosition = transform.position;  // Сохраняем текущую позицию камеры
    }
    
    private void FreeMoveCamera()
    {
        float h = Input.GetAxis("Mouse X") * cameraMoveSpeed;
        float v = Input.GetAxis("Mouse Y") * cameraMoveSpeed;
        
        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x += h;
        newCameraPosition.y += v;
        
        Vector3 directionFromPlayer = newCameraPosition - _target.position;
        if (directionFromPlayer.magnitude > maxDistanceFromPlayer)
        {
            newCameraPosition = _target.position + directionFromPlayer.normalized * maxDistanceFromPlayer;
        }
        
        transform.position = newCameraPosition;
    }
    
    private void ReturnCameraToPlayer()
    {
        isDetached = false;
       
        var desiredPosition = _target.position + offset;
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cameraMoveSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    // Фиксируем камеру по Z
    private void FixZPosition()
    {
        Vector3 position = transform.position; 
        position.z = FixedZ; 
        transform.position = position; 
    }
    
    // Пытаемся найти игрока
    private void FindPlayer()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _target = player.transform; // Устанавливаем target как игрока
        }
    }
}
