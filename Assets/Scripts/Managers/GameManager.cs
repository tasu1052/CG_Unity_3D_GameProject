using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager
{
    public float extraDamage = 1;
    public int beltCount = 0;

    //게임 상태를 나눠서 상태에 따라 스크립트들이 돌아가게 함
    public enum GameState
    {
        Battle,
        Store,
        Bless,

    }
    public GameState currentState;
    //플레이어 죽을 때 실행시킬 함수
    public void PlayerDied()
    {
       
    }
    //인게임 데이터 초기화 
    public void GameStart()
    {
        InventoryManager.Instance.nowUpgradeNumber = 0;
        KillManager.Instance.killCount = 0;

        InventoryManager.Instance.OpenInventory();
    }

    public void Upgrade()
    {
        Time.timeScale = 0;

    }

}
