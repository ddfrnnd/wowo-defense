using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public int weaponIndexToUnlock;
    private bool isPlayerNear = false;
    private WeaponSwitcher currentSwitcher = null;

    void Update()
    {
        if (isPlayerNear && currentSwitcher != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentSwitcher.UnlockWeapon(weaponIndexToUnlock);
                Debug.Log("Senjata " + weaponIndexToUnlock + " diambil!");
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentSwitcher = other.GetComponentInChildren<WeaponSwitcher>();
            if (currentSwitcher != null)
            {
                isPlayerNear = true;
                Debug.Log("Tekan E untuk mengambil senjata.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            currentSwitcher = null;
        }
    }
}
