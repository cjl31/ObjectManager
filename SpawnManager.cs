using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject panelToHide;
    [SerializeField] private ElementsMenu elementsMenu;
    [SerializeField] private Vector3 spawnAreaMin;
    [SerializeField] private Vector3 spawnAreaMax;
    [SerializeField] private Transform parent;
    [SerializeField] private InputField inputField;

    public void SpawnCubeButton()
    {
        int num;
        if (int.TryParse(inputField.text, out num))
        {   
            num = int.Parse(inputField.text);
            for(int i =0; i< num; i++)
            {
                float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
                float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
                float randomZ = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
                Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
                GameObject newObject = Instantiate(cubePrefab, randomPosition, Quaternion.identity);
                newObject.transform.SetParent(parent);
                newObject.name = "Cube "  +i;
            }

            elementsMenu.Awake();
            panelToHide.SetActive(false);        
        }
    }

    private void Start()
    {
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }
    private void OnInputValueChanged(string input)
    {
        if (!isInteger(input)|| !IsLessThan100(input))
        {
            inputField.text = input.Length > 0 ? input.Substring(0, input.Length - 1) : "";
        }        
    }

    private bool isInteger(string str)
    {
        return int.TryParse(str, out _);
    }
    
    private bool IsLessThan100(string str)
    {
        if (int.TryParse(str, out int number))
        {
            return number < 100;
        }
        return true;
    }
}
