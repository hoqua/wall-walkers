using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsBusy { get; private set; }

    public void SetBusy(bool state)
    {
        IsBusy = state;
    }
}
