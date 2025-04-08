using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenuAnimation : MonoBehaviour
{
    public Animator menuAnimator;
    public bool isMenuOpen = true;
    private void setBoolMenuAnimator(bool value)
    {
        menuAnimator.SetBool("isOpen", value);
    }
    public void InverseScrollMenu()
    {
        setBoolMenuAnimator(!isMenuOpen);
        isMenuOpen = !isMenuOpen;
    }

}
