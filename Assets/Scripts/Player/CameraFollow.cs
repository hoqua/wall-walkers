using UnityEngine;

public class CameraFollow : MonoBehaviour {
  public Transform target;          // Ссылка на персонажа, за которым нужно следить
  public Vector3 offset;            // Смещение камеры от персонажа
  public float speed = 0.01f;       // Скорость премещения камеры
  private const float FixedZ = -10; // Фиксированное значение Z для камеры

  private void Start() {
    var player = GameObject.FindWithTag("Player");
    if (player == null) {
      throw new System.Exception("Player not found");
    }

    target = player.transform;
  }


  private void LateUpdate() {
    if (target == null) return;

    var desiredPosition = target.position + offset;
    var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed);
    smoothedPosition.z = FixedZ;

    transform.position = smoothedPosition;
  }
}