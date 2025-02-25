using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField] private Image xpBar;      // UI-объект Image
    [SerializeField] private Sprite[] xpBarSprites; // 5 спрайтов опыта
    private PlayerStats playerStats;

    void Start()
    {
        StartCoroutine(FindPlayerStats());
    }

    private IEnumerator FindPlayerStats()
    {
        yield return new WaitUntil(() => playerStats = FindObjectOfType<PlayerStats>());
    }
    
    public void UpdateXPBar()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats не найден!");
            return;
        }
        
        float percentage = (float)playerStats.currentExp / playerStats.requiredExp;
        int index = Mathf.Clamp(Mathf.FloorToInt(percentage * xpBarSprites.Length), 0, xpBarSprites.Length - 1);
        xpBar.sprite = xpBarSprites[index];
    }
}