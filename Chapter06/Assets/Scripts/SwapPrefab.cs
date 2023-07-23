using UnityEngine;

public class SwapPrefab : MonoBehaviour
{
    public Food[] foods;
    private int currentFoodIndex = 0;
    private ARPlacePrefab arPlacePrefab;

    void Start()
    {
        arPlacePrefab = FindObjectOfType<ARPlacePrefab>();
        if (foods.Length > 0)
        {
            arPlacePrefab.objectToPlace = foods[0].prefab;
        }
    }

  public void SwapFoodPrefab()
{
    currentFoodIndex = (currentFoodIndex + 1) % foods.Length;
    arPlacePrefab.objectToPlace = foods[currentFoodIndex].prefab;
    arPlacePrefab.PlaceObject();  // Instantiate a new object immediately

    // Update the InfoText
    arPlacePrefab.InfoText.text = $"<b>Name:</b> {foods[currentFoodIndex].name}\n<b>Ingredients:</b> {foods[currentFoodIndex].ingredients}\n<b><color=red>Calories:</color></b> {foods[currentFoodIndex].calories}\n<b>Diet Type:</b> {foods[currentFoodIndex].dietType}";
}


public void SwapToPreviousFoodPrefab()
{
    currentFoodIndex--;
    if (currentFoodIndex < 0)
    {
        currentFoodIndex = foods.Length - 1; // Loop to the end if it goes below 0
    }
    arPlacePrefab.objectToPlace = foods[currentFoodIndex].prefab;
    arPlacePrefab.PlaceObject();  // Instantiate a new object immediately

    // Update the InfoText
    arPlacePrefab.InfoText.text = $"<b>Name:</b> {foods[currentFoodIndex].name}\n<b>Ingredients:</b> {foods[currentFoodIndex].ingredients}\n<b><color=red>Calories:</color></b> {foods[currentFoodIndex].calories}\n<b>Diet Type:</b> {foods[currentFoodIndex].dietType}";
}


    public Food GetCurrentFood()
    {
        return foods[currentFoodIndex];
    }
}


