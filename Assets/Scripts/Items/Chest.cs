using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool _isOpened = false;

    public void OpenChest()
    {
        if (_isOpened) return;

        _isOpened = true;
        Debug.Log("Chest opened! Player received a reward.");

        GiveReward();
        Destroy(gameObject);
    }

    private void GiveReward()
    {
        int rewardType = Random.Range(0, 3); // 0 - золото, 1 - зелье, 2 - опыт

        switch (rewardType)
        {
            case 0:
                Debug.Log("Player received 50 gold!");
                // playerStats.AddGold(50);
                break;
            case 1:
                Debug.Log("Player received a Health Potion!");
                // playerStats.HealToFull();
                break;
            case 2:
                Debug.Log("Player received EXP!");
                // playerStats.GainExp(10);
                break;
        }
    }
}