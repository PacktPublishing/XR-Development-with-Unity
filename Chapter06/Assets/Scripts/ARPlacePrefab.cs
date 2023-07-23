using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

public class ARPlacePrefab : MonoBehaviour
{
    public GameObject ObjectToPlace;
    public SwapPrefab SwapPrefabScript;
    public GameObject NextPrefabButton;
    public GameObject PreviousPrefabButton;
    public TextMeshProUGUI InfoText;
    public Button PlaceFirstMealButton;
    public GameObject InfoPanel;

    private ARRaycastManager _arRaycastManager;
    private Pose _placementPose;
    private bool _placementPoseIsValid = false;
    private GameObject _placedObject;
    private GameObject _nextButton;
    private GameObject _previousButton;
    private Vector2 _oldTouchDistance;
    private GameObject _placementIndicator;
    private GameObject _placementGrid;

    void Start()
    {
        _arRaycastManager = FindObjectOfType<ARRaycastManager>();
        _nextButton = InstantiateButton(NextPrefabButton, SwapPrefabScript.SwapFoodPrefab);
        _previousButton = InstantiateButton(PreviousPrefabButton, SwapPrefabScript.SwapToPreviousFoodPrefab);
        _placementGrid = CreatePlacementGrid();
        PlaceFirstMealButton.onClick.AddListener(PlaceObjectIfNeeded);
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            PinchToScale();
        }

        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (_placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    public void PlaceObject()
    {
        if (_placedObject != null)
        {
            Destroy(_placedObject);
        }

        _placedObject = Instantiate(ObjectToPlace, _placementPose.position, _placementPose.rotation);
        _placedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        PositionButton(_nextButton, new Vector3(0.5f, 0f, 0f));
        PositionButton(_previousButton, new Vector3(-0.5f, 0f, 0f));

        UpdateFoodInfoText();

        _placementGrid.SetActive(false);
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        _arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        _placementPoseIsValid = hits.Count > 0;
        if (_placementPoseIsValid)
        {
            _placementPose = hits[0].pose;
            PositionGrid(hits[0].pose.position, hits[0].pose.rotation);
        }
        else
        {
            _placementGrid.SetActive(false);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (_placementPoseIsValid)
        {
            _placementIndicator.SetActive(true);
            _placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        }
        else
        {
            _placementIndicator.SetActive(false);
        }
    }

    private GameObject InstantiateButton(GameObject buttonPrefab, UnityEngine.Events.UnityAction onClickAction)
    {
        var button = Instantiate(buttonPrefab);
        button.SetActive(false);
        button.GetComponent<Button>().onClick.AddListener(onClickAction);
        return button;
    }

    private GameObject CreatePlacementGrid()
    {
        var grid = GameObject.CreatePrimitive(PrimitiveType.Plane);
        grid.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        grid.GetComponent<Renderer>().material = Resources.Load<Material>("GridMaterial");
        grid.SetActive(false);
        return grid;
    }

    private void PlaceObjectIfNeeded()
    {
        if (_placementPoseIsValid)
        {
            PlaceObject();
            InfoPanel.SetActive(false);
        }
    }

    private void PinchToScale()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
        {
            Vector2 touchDistance = touchOne.position - touchZero.position;
            float pinchDistanceChange = touchDistance.magnitude - _oldTouchDistance.magnitude;

            float pinchToScaleSensitivity = 0.001f;

            if (_placedObject != null)
            {
                ScalePlacedObject(pinchDistanceChange, pinchToScaleSensitivity);
            }

            _oldTouchDistance = touchDistance;
        }
    }

    private void ScalePlacedObject(float pinchDistanceChange, float pinchToScaleSensitivity)
    {
        Vector3 newScale = _placedObject.transform.localScale + new Vector3(pinchDistanceChange, pinchDistanceChange, pinchDistanceChange) * pinchToScaleSensitivity;
        newScale = Vector3.Max(newScale, new Vector3(0.1f, 0.1f, 0.1f));
        newScale = Vector3.Min(newScale, new Vector3(10f, 10f, 10f));
        _placedObject.transform.localScale = newScale;
    }

    private void PositionButton(GameObject button, Vector3 offset)
    {
        button.transform.position = _placedObject.transform.position + offset;
        button.SetActive(true);
    }

    private void UpdateFoodInfoText()
    {
        Food currentFood = SwapPrefabScript.GetCurrentFood();
        InfoText.text = $"<b>Name:</b> {currentFood.name}\n<b>Ingredients:</b> {currentFood.ingredients}\n<b><color=red>Calories:</color></b> {currentFood.calories}\n<b>Diet Type:</b> {currentFood.dietType}";
        InfoText.transform.position = _placedObject.transform.position + new Vector3(-0.2f, 0.3f, 0f);
        InfoText.transform.rotation = _placedObject.transform.rotation;
    }

    private void PositionGrid(Vector3 position, Quaternion rotation)
    {
        _placementGrid.transform.SetPositionAndRotation(position, rotation);
        _placementGrid.SetActive(true);
    }
}
