using UnityEngine;

public class SwapPrefab : MonoBehaviour
{
    public Food[] AvailableFoods;
    private int CurrentFoodIndex = 0;
    private ARPlacePrefab ARPrefabPlacement;

    void Start()
    {
        ARPrefabPlacement = FindObjectOfType<ARPlacePrefab>();
        if (AvailableFoods.Length > 0)
        {
            ARPrefabPlacement.ObjectToPlace = AvailableFoods[0].prefab;
        }
    }

    public void SwapFoodPrefab()
    {
        CurrentFoodIndex = (CurrentFoodIndex + 1) % AvailableFoods.Length;
        ARPrefabPlacement.ObjectToPlace = AvailableFoods[CurrentFoodIndex].prefab;
        ARPrefabPlacement.PlaceObject();

        // Update the InfoText
        ARPrefabPlacement.InfoText.text = $"<b>Name:</b> {AvailableFoods[CurrentFoodIndex].name}\n<b>Ingredients:</b> {AvailableFoods[CurrentFoodIndex].ingredients}\n<b><color=red>Calories:</color></b> {AvailableFoods[CurrentFoodIndex].calories}\n<b>Diet Type:</b> {AvailableFoods[CurrentFoodIndex].dietType}";
    }

    public void SwapToPreviousFoodPrefab()
    {
        CurrentFoodIndex--;
        if (CurrentFoodIndex < 0)
        {
            CurrentFoodIndex = AvailableFoods.Length - 1; 
        }
        ARPrefabPlacement.ObjectToPlace = AvailableFoods[CurrentFoodIndex].prefab;
        ARPrefabPlacement.PlaceObject(); 

        // Update the InfoText
        ARPrefabPlacement.InfoText.text = $"<b>Name:</b> {AvailableFoods[CurrentFoodIndex].name}\n<b>Ingredients:</b> {AvailableFoods[CurrentFoodIndex].ingredients}\n<b><color=red>Calories:</color></b> {AvailableFoods[CurrentFoodIndex].calories}\n<b>Diet Type:</b> {AvailableFoods[CurrentFoodIndex].dietType}";
    }

    public Food GetCurrentFood()
    {
        return AvailableFoods[CurrentFoodIndex];
    }
}
