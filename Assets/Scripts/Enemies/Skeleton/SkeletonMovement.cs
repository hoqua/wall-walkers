using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemies.Skeleton
{
    public class SkeletonMovement : Enemy
    {
        private Tilemap _tilemap;                            // Tilemap по которому будет двигаться враг
        private SkeletonAttack _skeletonAttack;              // Ссылка на скрипт отвечающий за атаку врага
        private PlayerMovement _player;                      // Ссылка на скрипт игрока
        private SpawnManager _spawnManager;                  // Ссылка для получения радиуса от игрока на котором враг может двигаться
   
        public Vector3Int currentTile;                       // Текущий тайл врага
        private readonly float _moveSpeed = 3f;                        // Скорость движения врага
        private Vector3 _targetPosition;                     // Целевая позиция для перемещения
        private bool _isMoving;                              // Флаг, что враг в движении
    
        void Start()
        {
            _tilemap = FindObjectOfType<Tilemap>();
            _skeletonAttack = GetComponent<SkeletonAttack>();
            _player = FindObjectOfType<PlayerMovement>();
            _spawnManager = FindObjectOfType<SpawnManager>();
        
            _targetPosition = transform.position;
            currentTile = _tilemap.WorldToCell(transform.position);
            EnemyPositionManager.Instance.RegisterEnemy(currentTile);
        }

        public override void EnemyTurn()
        {
            MoveTowardsPlayer();
        }

        void OnDestroy()
        {
            EnemyPositionManager.Instance.UnregisterEnemy(currentTile);
        }

        public void SetCurrentTile(Vector3Int tilePosition, Tilemap map)
        {
            currentTile = tilePosition;
            _tilemap = map;
            transform.position = _tilemap.GetCellCenterWorld(currentTile);
            _targetPosition = transform.position;
        }
    
        private void MoveTowardsPlayer()
        {
            Vector3Int playerTile = _player.currentTile;

            if (Vector3Int.Distance(currentTile, playerTile) > _spawnManager.spawnRadius - 3)
            {
                return; // Игрок слишком далеко
            }

            if (_skeletonAttack.IsPlayerInRange(currentTile, playerTile))
            {
                _skeletonAttack.AttackPlayer();
                return;
            }

            Vector3Int direction = GetDirectionTowardsPlayer(playerTile);
            Vector3Int targetTile = currentTile + direction;

            if (_tilemap.GetTile(targetTile) != null && targetTile != playerTile && !IsTileOccupied(targetTile))
            {
                MoveToTile(targetTile);
            }
        }
        
        private Vector3Int GetDirectionTowardsPlayer(Vector3Int playerTile)
        {
            return new Vector3Int(
                (int)Mathf.Sign(playerTile.x - currentTile.x),
                (int)Mathf.Sign(playerTile.y - currentTile.y),
                0
            );
        }

        private void MoveToTile(Vector3Int targetTile)
        {
            if (!_isMoving)
            {
                _targetPosition = _tilemap.GetCellCenterWorld(targetTile);
                EnemyPositionManager.Instance.UnregisterEnemy(currentTile);
                currentTile = targetTile;
                EnemyPositionManager.Instance.RegisterEnemy(currentTile);

                _isMoving = true;
                if (this != null)
                {
                    StartCoroutine(MoveCoroutine());
                } 
                else return;
            }
        }

        private IEnumerator MoveCoroutine()
        {
            while (Vector3.Distance(transform.position, _targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
                yield return null;
            }
            
            var enemyStats = GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.UpdateCurrentTile();
            }

            _isMoving = false;
        }
    
        // Проверяем, занята ли клетка другим врагом
        private bool IsTileOccupied(Vector3Int tilePosition)
        {
            return EnemyPositionManager.Instance.IsTileOccupied(tilePosition) || _spawnManager.IsTileOccupiedByItem(tilePosition);
        }


    }
}
