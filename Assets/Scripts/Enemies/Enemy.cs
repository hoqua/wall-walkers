using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        public abstract void EnemyTurn();
        
        protected static Tilemap Tilemap; // Один tilemap для всех врагов

        protected virtual void Awake()
        {
            if (Tilemap == null)
            {
                Tilemap = FindObjectOfType<Tilemap>();

                if (Tilemap == null)
                {
                    Debug.LogError(gameObject.name + ": Tilemap не найден в сцене!");
                }
                else
                {
                    Debug.Log("Tilemap найден и установлен в Enemy.");
                }
            }
            
        }
    }
}
