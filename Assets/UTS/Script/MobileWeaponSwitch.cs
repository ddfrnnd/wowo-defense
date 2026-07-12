using UnityEngine;
using UnityEngine.EventSystems;
using InfimaGames.LowPolyShooterPack;

public class MobileWeaponSwitch : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int weaponIndex;
    private CharacterBehaviour playerCharacter;

    public void SetWeaponIndex(int index)
    {
        weaponIndex = index;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerCharacter == null || !playerCharacter.gameObject.activeInHierarchy)
        {
            playerCharacter = Object.FindAnyObjectByType<CharacterBehaviour>();
        }

        if (playerCharacter != null)
        {
            playerCharacter.MobileWeaponSwitch(weaponIndex);
        }
    }
}
