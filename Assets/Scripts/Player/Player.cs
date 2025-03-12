
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
    }
    
    public bool IsTurnFinished()
    {
        return !_playerMovement.IsMoving() || !_playerAttack.HasAttacked();
    }
}
