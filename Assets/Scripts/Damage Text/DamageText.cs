using TMPro;
using UnityEngine;

namespace Damage_Text
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1.5f;  // Скорость движения вверх
        [SerializeField] private float displayTime = 0.75f;  // Время отображения текста
        [SerializeField] private float fadeTime = 0.25f;   // Время исчезновения
        [SerializeField] private TextMeshProUGUI damageText;

        private Color _textColor;
        private float _timeAlive;  // Таймер жизни текста

        private void Start()
        {
            damageText = GetComponent<TextMeshProUGUI>();
            _timeAlive = 0f;
            _textColor = damageText.color;
        }

        public void SetDamage(int damage)
        {
            damageText.text = damage.ToString();
            _textColor.a = 1f; 
            _timeAlive = 0f; 
            Destroy(gameObject, displayTime + fadeTime);  // Уничтожаем объект по истечении времени
        }

        private void Update()
        {
            _timeAlive += Time.deltaTime;

            // Двигаем текст вверх
            transform.position += Vector3.up * (moveSpeed * Time.deltaTime);

            // Если прошло время для исчезновения, начинаем уменьшать альфа-канал
            if (_timeAlive > displayTime)
            {
                float fadeProgress = (_timeAlive - displayTime) / fadeTime;
                _textColor.a = Mathf.Lerp(1f, 0f, fadeProgress);  // Постепенное уменьшение прозрачности
            }

            damageText.color = _textColor;
        }
    }
}