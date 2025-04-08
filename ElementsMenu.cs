using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ElementsMenu : MonoBehaviour
{
    public List<ElementController> elements;

    [SerializeField] private List<Image> transparencyButtonsBounds;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject prefabElement;
    [SerializeField] private Transform CubesContainer;
    private bool isVisibleAll = true;
    private bool isSelectedAll = false;
    public void Awake() 
    {
        CreateElements();
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void CreateElements()
    {
        foreach (Transform child in CubesContainer)
        {
            if (prefabElement != null)
            {
                GameObject newObject = Instantiate(prefabElement);
                    newObject.transform.SetParent(transform);
                ElementController newElementController = newObject.GetComponent<ElementController>();
                    newElementController.referenceObject = child.GetComponent<ColorChangeableObject>();
                    newElementController.SetLabel(child.name);
                    newElementController.elementsMenu = this;
                    newElementController.cameraController = cameraController;
                elements.Add(newElementController);
            }
        }
    }
    
    public void SelectAll()
    {
        foreach(var elem in elements)
        {
            elem.selectionToggle.isOn = !isSelectedAll;
        }
        isSelectedAll = !isSelectedAll;
        cameraController.UpdateFocus();
    }
    
    public void VisibleAll()
    {
        foreach(var elem in elements)
        {
            elem.visibleToggle.isOn = !isVisibleAll;
        }
        isVisibleAll = !isVisibleAll;
    }
   
    public void SetColorToObjects(Color color)
    {
        foreach(var elem in elements)
        {
            if(elem.isSelected)
            {
                elem.referenceObject.SetColor(color);
            }
        }
    }
    
    public void SetTransparencyToObjects(int value)
    {
        foreach(var elem in elements)
        {
            if(elem.isSelected)
            {
                elem.currentTransparencyLevel = value;
                elem.referenceObject.SetTransparency(value/4.0f);
            }
        }
        ValidateTransparencyLevel();
    }

    public void ValidateTransparencyLevel()
    {
        ElementController firstAciveElem = elements.FirstOrDefault(e=>e.isSelected);
        float levelOfFirstSelected = -1;
        if(firstAciveElem!=null)
        {
            levelOfFirstSelected = firstAciveElem.currentTransparencyLevel;
        }
        SetTransparencyLevel(-1);
        foreach(var elem in elements)
        {
            if(elem.isSelected)
            {
                if(elem.currentTransparencyLevel != levelOfFirstSelected)
                {
                    return;
                }
            }
        }
        SetTransparencyLevel((int)levelOfFirstSelected);
    }
   
    private void SetTransparencyLevel(int value)
    {
        if(value==-1)
        {
            foreach(var boundImg in transparencyButtonsBounds)
            {
                boundImg.enabled = false;
            }
        }
        else 
        {
            transparencyButtonsBounds[value].enabled = true;
        }
    }
}
