using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    private int selectedWeaponIndex = 0;
    public System.Collections.Generic.List<int> unlockedWeapons = new System.Collections.Generic.List<int>() { 0 };

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        if (unlockedWeapons.Count == 0) return;

        int previousSelected = selectedWeaponIndex;
        int currentIndexInList = unlockedWeapons.IndexOf(selectedWeaponIndex);
        if (currentIndexInList == -1) currentIndexInList = 0;

        // Scroll Wheel input
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            currentIndexInList++;
            if (currentIndexInList >= unlockedWeapons.Count) currentIndexInList = 0;
            selectedWeaponIndex = unlockedWeapons[currentIndexInList];
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            currentIndexInList--;
            if (currentIndexInList < 0) currentIndexInList = unlockedWeapons.Count - 1;
            selectedWeaponIndex = unlockedWeapons[currentIndexInList];
        }

        // Numeric keys input
        for (int i = 0; i < transform.childCount && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (unlockedWeapons.Contains(i)) {
                    selectedWeaponIndex = i;
                }
            }
        }

        if (previousSelected != selectedWeaponIndex)
        {
            SelectWeapon();
        }
    }

    public void UnlockWeapon(int index)
    {
        if (!unlockedWeapons.Contains(index))
        {
            unlockedWeapons.Add(index);
            unlockedWeapons.Sort();
            selectedWeaponIndex = index; // Auto equip
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (weapon.GetComponent<GunController>() != null)
            {
                weapon.gameObject.SetActive(i == selectedWeaponIndex && unlockedWeapons.Contains(i));
            }
            i++;
        }
    }
}
