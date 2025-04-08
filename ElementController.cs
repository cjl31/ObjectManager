using UnityEngine;
using UnityEngine.UI;

public class ElementController : MonoBehaviour
{
    public ElementsMenu elementsMenu;
    public Toggle visibleToggle;
    public Toggle selectionToggle;
    public ColorChangeableObject referenceObject;
    public CameraController cameraController;
    public bool isSelected = false;
    public bool isVisible = true;
    public float currentTransparencyLevel = 4;
    [SerializeField] Text elementText;
    [SerializeField] private GameObject bound;

    public void SetVisible(bool value)
    {
        isVisible = value;
        referenceObject.gameObject.SetActive(value);
    }
    
    public void InvertVisible()
    {
        SetVisible(!isVisible);
    }

    public void SetSelection(bool value)
    {
        isSelected = value;
        bound.SetActive(value);
        cameraController.UpdateFocus();
        elementsMenu.ValidateTransparencyLevel();
    }
   
    public void InvertSelection()
    {
        SetSelection(!isSelected);
    }
   
    public void SetLabel(string str)
    {
        elementText.text = str;
    }
}
