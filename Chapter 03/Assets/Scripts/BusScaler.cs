using UnityEngine;
using UnityEngine.UI;

public class BusScaler : MonoBehaviour
{
    public GameObject bus; // Assign your bus GameObject in inspector
    public Slider slider; // Assign your slider in inspector

    private void Awake()
    {
        // Initial size for the bus or you can set it up in the Inspector
        bus.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void Update()
    {
        // Adjust the scale of the bus according to the slider's value
        float scaleValue = slider.value;
        bus.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    }
}
