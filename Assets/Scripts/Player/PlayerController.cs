using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    public void StartPlayersTurn()
    {
        _playerMovement.hasMoved = false;
        _playerAttack.ResetAttack();
    }

    public bool HasMovedOrAttacked()
    {
        return _playerMovement.hasMoved || _playerAttack.hasAttacked;
    }
}
