using UnityEngine;

public class CameraHueShifter : MonoBehaviour
{
    [SerializeField] private float _hueShiftSpeed = 0.2f;
    [SerializeField, Range(0,1)] private float _saturation = 1f;
    [SerializeField, Range(0,1)] private float _value = 1f;
 
    private Camera _mainCamera;
 
    private void Awake()
    {
        // get a reference to the main camera
        _mainCamera = Camera.main;
 
        // make sure the camera is using the solid color background instead of skybox
        _mainCamera.clearFlags = CameraClearFlags.SolidColor;
    }
 
    private void Update()
    {
        if (GameManager.s_gameInstance.music.isPlaying)
        {
            float amountToShift = _hueShiftSpeed * Time.deltaTime;
            Color newColor = ShiftHueBy(_mainCamera.backgroundColor, amountToShift);
            _mainCamera.backgroundColor = newColor;   
        }
    }
 
    private Color ShiftHueBy(Color color, float amount)
    {
        // convert from RGB to HSV
        Color.RGBToHSV(color, out float hue, out float sat, out float val);
 
        // shift hue by amount
        hue += amount;
        sat = _saturation;
        val = _value;
 
        // convert back to RGB and return the color
        return Color.HSVToRGB(hue, sat, val);
    }
}
