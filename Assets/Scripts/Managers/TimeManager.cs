using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private float startTime;
    private float elapsedTime;

    void Awake()
    {
        // 싱글톤 (선택사항)
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        elapsedTime = Time.time - startTime;
        Debug.Log($"경과 시간: {elapsedTime:F2}초");
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}