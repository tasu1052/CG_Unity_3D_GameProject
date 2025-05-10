using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // HpBar Slider를 연동하기 위한 Slider 객체
    [SerializeField] private Slider _hpBar;
    //게임 오버 화면으로 사용할 UI 오브젝트
    [SerializeField] private GameObject gameOverUI;

    // 플레이어의 HP
    private int _hp;

    public int Hp
    {
        get => _hp;
        // Math.Clamp 함수를 사용해서 hp가 0보다 아래로 떨어지지 않게 합니다.
        private set => _hp = Math.Clamp(value, 0, _hp);
    }
    void Start()
    {
        //게임 시작할 때 GameOver UI는 비활성화
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }
    private void Awake()
    {
        // 플레이어의 HP 값을 100으로 초기 세팅
        _hp = 100;
        // MaxValue를 세팅하는 함수
        SetMaxHealth(_hp);
    }

    public void SetMaxHealth(int health) //최대 체력 및 현재 값을 설정하는 함수
    {
        _hpBar.maxValue = health;
        _hpBar.value = health;
    }

    // 플레이어가 데미지를 받으면 데미지 값을 전달 받아 HP바에 반영
    public void GetDamage(int damage)
    {
        int getDamagedHp = Hp - damage;
        Hp = getDamagedHp;
        _hpBar.value = Hp;
        //체력 0 이하면 gameover 함수 호출
        if (Hp <= 0)
            GameOver();
    }
    private void GameOver()//게임 오버 처리 함수
    {
        Debug.Log("Game Over");
        //게임 오버 UI 활성화
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        Time.timeScale = 0; // 게임 일시정지
    }
}