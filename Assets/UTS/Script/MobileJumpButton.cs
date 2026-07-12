using UnityEngine;
using UnityEngine.EventSystems;
using InfimaGames.LowPolyShooterPack;

public class MobileJumpButton : MonoBehaviour, IPointerDownHandler
{
    private CharacterBehaviour playerCharacter;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerCharacter == null || !playerCharacter.gameObject.activeInHierarchy)
        {
            playerCharacter = Object.FindAnyObjectByType<CharacterBehaviour>();
        }

        if (playerCharacter != null)
        {
            playerCharacter.MobileJump();
        }
    }
}
