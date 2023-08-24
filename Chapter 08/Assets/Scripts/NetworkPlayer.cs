using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.InputSystem;
public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    private PhotonView photonView;

    public InputActionAsset xriInputActions; 
    private InputActionMap headActionMap;
    private InputActionMap leftHandActionMap;
    private InputActionMap rightHandActionMap;

    private InputAction headPositionAction;
    private InputAction headRotationAction;
    private InputAction leftHandPositionAction;
    private InputAction leftHandRotationAction;
    private InputAction rightHandPositionAction;
    private InputAction rightHandRotationAction;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        // Get the Action Maps
        headActionMap = xriInputActions.FindActionMap("XRI Head");
        leftHandActionMap = xriInputActions.FindActionMap("XRI LeftHand");
        rightHandActionMap = xriInputActions.FindActionMap("XRI RightHand");

        // Get the Position and Rotation actions for each action map
        headPositionAction = headActionMap.FindAction("Position");
        headRotationAction = headActionMap.FindAction("Rotation");

        leftHandPositionAction = leftHandActionMap.FindAction("Position");
        leftHandRotationAction = leftHandActionMap.FindAction("Rotation");

        rightHandPositionAction = rightHandActionMap.FindAction("Position");
        rightHandRotationAction = rightHandActionMap.FindAction("Rotation");

        // Enable actions
        headPositionAction.Enable();
        headRotationAction.Enable();
        leftHandPositionAction.Enable();
        leftHandRotationAction.Enable();
        rightHandPositionAction.Enable();
        rightHandRotationAction.Enable();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            rightHand.gameObject.SetActive(false);
            leftHand.gameObject.SetActive(false);
            head.gameObject.SetActive(false);

            MapPosition(head, XRNode.Head);
            MapPosition(leftHand, XRNode.LeftHand);
            MapPosition(rightHand, XRNode.RightHand);
        }
    }

    void MapPosition(Transform target, XRNode node)
    {
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        if (node == XRNode.Head)
        {
            position = headPositionAction.ReadValue<Vector3>();
            rotation = headRotationAction.ReadValue<Quaternion>();
        }
        else if (node == XRNode.LeftHand)
        {
            position = leftHandPositionAction.ReadValue<Vector3>();
            rotation = leftHandRotationAction.ReadValue<Quaternion>();
        }
        else if (node == XRNode.RightHand)
        {
            position = rightHandPositionAction.ReadValue<Vector3>();
            rotation = rightHandRotationAction.ReadValue<Quaternion>();
        }

        target.position = position;
        target.rotation = rotation;
    }

    void OnDestroy()
    {
        headPositionAction.Disable();
        headRotationAction.Disable();
        leftHandPositionAction.Disable();
        leftHandRotationAction.Disable();
        rightHandPositionAction.Disable();
        rightHandRotationAction.Disable();
    }
}