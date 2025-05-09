using UnityEngine;

public class WeaponAttachManager : MonoBehaviour
{
    public Transform player;  // 플레이어

    public GameObject flamethrowerPrefab;   // 화염방사기 프리팹
    public GameObject riflePrefab;          // 소총 프리팹
    public GameObject grenadeLauncherPrefab;// 그레네이드 발사기 프리팹

    // 각 무기의 부착 위치 오프셋 (플레이어 기준, 로컬좌표)
    public Vector3 flamethrowerOffset = new Vector3(0, 0, 1);  
    public Vector3 rifleOffset = new Vector3(0.5f, 0, 1);  
    public Vector3 grenadeLauncherOffset = new Vector3(-0.5f, 0, 1);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AttachWeapon(flamethrowerPrefab, flamethrowerOffset);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AttachWeapon(riflePrefab, rifleOffset);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AttachWeapon(grenadeLauncherPrefab, grenadeLauncherOffset);
        }
    }

    void AttachWeapon(GameObject weaponPrefab, Vector3 localOffset)
    {
        // 무기 생성
        GameObject weapon = Instantiate(weaponPrefab);

        // 부모를 플레이어로 설정
        weapon.transform.SetParent(player);

        // 로컬 위치/회전 설정 (플레이어 앞쪽)
        weapon.transform.localPosition = localOffset;
        weapon.transform.localRotation = Quaternion.identity;
    }
}
