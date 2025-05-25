using UnityEngine;

public class weaponattachmanager1 : MonoBehaviour
{
    public static weaponattachmanager1 Instance;

    public Transform player;  // 플레이어
    public SimpleCooldown cooldown;

    public GameObject flamethrowerPrefab;
    public GameObject riflePrefab;
    public GameObject grenadeLauncherPrefab;
    public GameObject cartridgeBeltPrefab;
    //public GameObject flameskillPrefab;

    public float weaponDistanceFromPlayer = 1.0f; // 플레이어로부터 무기 거리

    private GameObject currentWeapon;

    private void Start()
    {
        Instance = this;
    }

    public GameObject AttachFlame(FireFlame fire)
    {
        float k = fire.damage;
        float m = fire.fireRate;
        Debug.Log($"AttachFlame : {k} : {m}");
        currentWeapon = Instantiate(flamethrowerPrefab);
        currentWeapon.transform.SetParent(player);
        currentWeapon.transform.localRotation = Quaternion.identity;
        currentWeapon.transform.localPosition = Vector3.zero;
        Flamethrower flamethrowerScript = currentWeapon.GetComponent<Flamethrower>();
        if (flamethrowerScript != null)
        {
            flamethrowerScript.SetStats(m, k);
        }
        return currentWeapon;
    }

    public GameObject AttachRiffle(Riffle riffle)
    {
        float k = riffle.damage;
        float m = riffle.fireRate;
        Debug.Log($"AttachFlame : {k} : {m}");
        currentWeapon = Instantiate(riflePrefab);
        currentWeapon.transform.SetParent(player);
        currentWeapon.transform.localRotation = Quaternion.identity;
        currentWeapon.transform.localPosition = Vector3.zero;
        rifle rifleScript = currentWeapon.GetComponent<rifle>();
        if (rifleScript != null)
        {
            rifleScript.SetStats(m, k);
        }
        return currentWeapon;
    }

    public GameObject AttachLauncher(Launcher launcher)
    {

        float k = launcher.damage;
        float m = launcher.fireRate;
        Debug.Log($"AttachFlame : {k} : {m}");
        currentWeapon = Instantiate(grenadeLauncherPrefab);
        currentWeapon.transform.SetParent(player);
        currentWeapon.transform.localRotation = Quaternion.identity;
        currentWeapon.transform.localPosition = Vector3.zero;
        grenade grenadeScript = currentWeapon.GetComponent<grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.SetStats(m, k);
        }
        return currentWeapon;
    }

    public GameObject AttachBelt(CartridgeBelt belt)
    {

        currentWeapon = Instantiate(cartridgeBeltPrefab);
        currentWeapon.transform.SetParent(player);
        currentWeapon.transform.localRotation = Quaternion.identity;
        currentWeapon.transform.localPosition = Vector3.zero;
        CartridgeBeltAction beltScript = currentWeapon.GetComponent<CartridgeBeltAction>();
        if (beltScript != null)
        {
            beltScript.SetAllDamage(belt.damagePumping);
        }
        return currentWeapon;
    }



    public void AttachWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon);

        currentWeapon = Instantiate(weaponPrefab);
        currentWeapon.transform.SetParent(player);
        currentWeapon.transform.localRotation = Quaternion.identity;
    }

    void UpdateWeaponPositionAndRotation()
    {
        Transform nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return;

        // 적 방향 계산 (플레이어 기준)
        Vector3 dirToEnemy = (nearestEnemy.position - player.position).normalized;

        // 무기 위치를 플레이어 기준 적 방향으로 설정
        Vector3 worldWeaponPos = player.position + dirToEnemy * weaponDistanceFromPlayer;

        currentWeapon.transform.position = worldWeaponPos;

        // 무기가 적을 향하게 회전
        Vector3 lookDir = (nearestEnemy.position - currentWeapon.transform.position).normalized;
        if (lookDir != Vector3.zero)
            currentWeapon.transform.rotation = Quaternion.LookRotation(lookDir);
    }

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(player.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }
}
