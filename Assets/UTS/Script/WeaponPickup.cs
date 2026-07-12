using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class WeaponPickup : MonoBehaviour
{
    public int weaponIndexToUnlock;
    private bool isPlayerNear = false;
    private InventoryBehaviour currentInventory = null;

    void Update()
    {
        if (isPlayerNear && currentInventory != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentInventory.UnlockWeapon(weaponIndexToUnlock);
                Debug.Log("Senjata " + weaponIndexToUnlock + " diambil!");
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentInventory = other.GetComponentInChildren<InventoryBehaviour>();
            if (currentInventory != null)
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
            currentInventory = null;
        }
    }
}
