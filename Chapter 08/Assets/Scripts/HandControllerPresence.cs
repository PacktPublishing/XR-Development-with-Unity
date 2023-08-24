using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandControllerPresence : MonoBehaviour
{
    public GameObject handVisualizationPrefab;

    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;

    private GameObject instantiatedHandVisual;
    private Animator handMotionController;

    // Called before the first frame update
    void Awake()
    {
        InitializeHandController();
    }

    void InitializeHandController()
    {
        instantiatedHandVisual = Instantiate(handVisualizationPrefab, transform);
        handMotionController = instantiatedHandVisual.GetComponent<Animator>();
    }

    void AdjustHandMotion()
    {
        float triggerIntensity = triggerAction.action.ReadValue<float>();
        float gripIntensity = gripAction.action.ReadValue<float>();

        handMotionController.SetFloat("Trigger", triggerIntensity);
        handMotionController.SetFloat("Grip", gripIntensity);
    }

    // Called every frame
    void Update()
    {
        AdjustHandMotion();
    }
}