using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillManager : MonoBehaviour
{
    public static KillManager Instance; // 싱글톤
    public int killCount = 0;
    public TextMeshProUGUI killText; // UI 텍스트

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddKill()
    {
        killCount++;
        if (killText != null)
            killText.text = "Kill Count: " + killCount;
        //Debug.Log("현재 Kill 수: " + killCount);
        //10 30 60 100 200 300
        if (killCount == 10||killCount==30||killCount==60||killCount%100==0)
        {
            InventoryManager.Instance.OpenInventory();
        }
    }
}
