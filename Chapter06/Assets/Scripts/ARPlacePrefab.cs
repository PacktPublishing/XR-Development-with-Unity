using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ARPlacePrefab : MonoBehaviour
{
    public GameObject objectToPlace;
    public SwapPrefab swapPrefabScript;
    public GameObject buttonPrefab; // For next prefab
    public GameObject buttonPrefabPrev; // For previous prefab
    public TMPro.TextMeshProUGUI InfoText;
    public Button PlaceFirstMealButton;
    public GameObject InfoPanel;

    private ARRaycastManager arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private GameObject placedObject;
    private GameObject placedButtonNext;
    private GameObject placedButtonPrev;
    private Vector2 oldTouchDistance; // for pinch-to-scale
    private GameObject placementIndicator;
    private GameObject placementGrid;


    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();

        // Instantiate the button for next prefab
        placedButtonNext = Instantiate(buttonPrefab);
        placedButtonNext.SetActive(false);
        placedButtonNext.GetComponent<Button>().onClick.AddListener(swapPrefabScript.SwapFoodPrefab);

        // Instantiate the button for previous prefab
        placedButtonPrev = Instantiate(buttonPrefabPrev);
        placedButtonPrev.SetActive(false);
        placedButtonPrev.GetComponent<Button>().onClick.AddListener(swapPrefabScript.SwapToPreviousFoodPrefab);

        // Create your placement grid
        placementGrid = GameObject.CreatePrimitive(PrimitiveType.Plane);
        placementGrid.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        placementGrid.GetComponent<Renderer>().material = Resources.Load<Material>("GridMaterial");
        placementGrid.SetActive(false);

        // Add this line
        PlaceFirstMealButton.onClick.AddListener(() =>
        {
            if (placementPoseIsValid)
            {
                PlaceObject();
                InfoPanel.SetActive(false);

            }
        });
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
          
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
            {
                // Calculate new pinch distance
                Vector2 touchDistance = touchOne.position - touchZero.position;
                float pinchDistanceChange = touchDistance.magnitude - oldTouchDistance.magnitude;

                // You can tune this value to adjust the sensitivity
                float pinchToScaleSensitivity = 0.001f;

                // Scale the placed object relative to the pinch distance change
                if (placedObject != null)
                {
                    Vector3 newScale = placedObject.transform.localScale + new Vector3(pinchDistanceChange, pinchDistanceChange, pinchDistanceChange) * pinchToScaleSensitivity;
                    // Clamp the scale to reasonable values
                    newScale = Vector3.Max(newScale, new Vector3(0.1f, 0.1f, 0.1f));
                    newScale = Vector3.Min(newScale, new Vector3(10f, 10f, 10f));
                    placedObject.transform.localScale = newScale;
                }

                // Update old pinch distance
                oldTouchDistance = touchDistance;
            }
        }

        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    public void PlaceObject()
    {
        if (placedObject != null)
        {
            Destroy(placedObject);
        }

        placedObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
        placedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        placedButtonNext.transform.position = placedObject.transform.position + new Vector3(0.5f, 0f, 0f);
        placedButtonNext.SetActive(true);

        placedButtonPrev.transform.position = placedObject.transform.position + new Vector3(-0.5f, 0f, 0f); // adjust as needed
        placedButtonPrev.SetActive(true);

        // Get the current food
        Food currentFood = swapPrefabScript.GetCurrentFood();

        // Update the InfoText
        InfoText.text = $"<b>Name:</b> {currentFood.name}\n<b>Ingredients:</b> {currentFood.ingredients}\n<b><color=red>Calories:</color></b> {currentFood.calories}\n<b>Diet Type:</b> {currentFood.dietType}";

        // Position the InfoText above the prefab
        InfoText.transform.position = placedObject.transform.position + new Vector3(-0.2f, 0.3f, 0f); // adjust the y offset as needed
        InfoText.transform.rotation = placedObject.transform.rotation;


        // Hide the placement grid
        placementGrid.SetActive(false);
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            placementGrid.transform.SetPositionAndRotation(hits[0].pose.position, hits[0].pose.rotation);
            placementGrid.SetActive(true); // Show the grid when the placement pose is valid
        }
        else
        {
            placementGrid.SetActive(false); // Hide the grid when the placement pose is not valid
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }
}
