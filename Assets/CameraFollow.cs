using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // 따라갈 플레이어
    public Vector3 offset = new Vector3(0f, 10f, -1f);  // 카메라 위치 (Top View)
    void LateUpdate()
    {
        //대상이 존재하면 실행됨
        if (target != null)
        {
            //카메라 위치를 대상(플레이어)의 위치 + 오프셋으로 설정
            transform.position = target.position + offset;
            transform.LookAt(target);  // 카메라가 항상 플레이어를 바라보게
        }
    }
}