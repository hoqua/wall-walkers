using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
    private Transform _target;          // Ссылка на персонажа, за которым нужно следить
    public Vector3 offset;            // Смещение камеры от персонажа
    public float speed = 0.01f;       // Скорость премещения камеры
    private const float FixedZ = -10; // Фиксированное значение Z для камеры

    private void Start() {
        FindPlayer(); // Пытаемся найти игрока при старте
    }

    private void LateUpdate() {
        if (_target == null) {
            FindPlayer(); // Если игрока до сих пор нет, продолжаем искать
            return;
        }

        var desiredPosition = _target.position + offset;
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed);
        smoothedPosition.z = FixedZ;

        transform.position = smoothedPosition;
    }

    private void FindPlayer() {
        var player = GameObject.FindWithTag("Player");
        if (player != null) {
            _target = player.transform; // Устанавливаем target как игрока
        }
    }
}