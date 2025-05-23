using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // HpBar Slider�� �����ϱ� ���� Slider ��ü
    [SerializeField] private Slider _hpBar;
    //���� ���� ȭ������ ����� UI ������Ʈ
    [SerializeField] private GameObject gameOverUI;

    // �÷��̾��� HP
    private int _hp;

    public int Hp
    {
        get => _hp;
        // Math.Clamp �Լ��� ����ؼ� hp�� 0���� �Ʒ��� �������� �ʵ��� ��.
        private set => _hp = Math.Clamp(value, 0, _hp);
    }
    void Start()
    {
        //���� ������ �� GameOver UI�� ��Ȱ��ȭ
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }
    private void Awake()
    {
        // �÷��̾��� HP ���� 100���� �ʱ� ����
        _hp = 100;
        // MaxValue�� �����ϴ� �Լ�
        SetMaxHealth(_hp);
    }

    public void SetMaxHealth(int health) //�ִ� ü�� �� ���� ���� �����ϴ� �Լ�
    {
        _hpBar.maxValue = health;
        _hpBar.value = health;
    }

    // �÷��̾ �������� ������ ������ ���� ���� �޾� HP�ٿ� �ݿ�
    public void GetDamage(int damage)
    {
        int getDamagedHp = Hp - damage;
        Hp = getDamagedHp;
        _hpBar.value = Hp;
        //ü�� 0 ���ϸ� gameover �Լ� ȣ��
        if (Hp <= 0)
            GameOver();
    }
    private void GameOver()//���� ���� ó�� �Լ�
    {
        Debug.Log("Game Over");
        //���� ���� UI Ȱ��ȭ
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        Time.timeScale = 0; // ���� �Ͻ�����
    }
    public void Heal(int amount)
    {
        Hp += amount;
        _hpBar.value = Hp;
    }

}