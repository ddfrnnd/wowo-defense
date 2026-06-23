using UnityEngine;

public class MainMenuEnemy : MonoBehaviour
{
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("Walk Forward In Place");
        }
    }
}
