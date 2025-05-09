using UnityEngine;

public class BillboardTopView : MonoBehaviour
{
    //탑다운 시점에서는 hp바가 보이지 않기 때문에 체력바가
    //항상 카메라 정면을 보고 있도록 하기 위해 추가한 스크립트입니다.
    //모든 Update가 끝난 후 호출되는 LateUpdate
    void LateUpdate()
    {
        //오브젝트의 로컬회전을 항상 90, 0 ,0으로 고정 즉, x축으로 90도 회전.
        transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
