using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MothController : MonoBehaviour {
    [Header("Debug Value")]
    [SerializeField] private string receivedString;
    [SerializeField] private string[] datas = new string[4];

    [Header("Value that can be modified")]
    [SerializeField] private float sensivity = .1f;
    [SerializeField] private int minClampValue = 80;
    [SerializeField] private int maxClampValue = 500;
    [SerializeField] private int minimumMovementTreshold = 20;
    [SerializeField] private float smoothRotationValue = 666.0f;
    [SerializeField] private float maxStreetLampAlpha = 0.3f;

    [SerializeField] private Image rightLight;
    [SerializeField] private Image leftLight;
    [SerializeField] private Image topLight;
    [SerializeField] private Image downLight;
    
    private SerialController _serialController;
    private Rigidbody _rb;
    private int[] _incomeValues = new int[4];
    private Vector3 _moveValue;
    private int _finalValueX;
    private int _finalValueZ;
    
    private void Start() {
        _rb = GetComponent<Rigidbody>();
        _serialController = GetComponent<SerialController>();
    }

    private void Update() {
        receivedString = _serialController.ReadSerialMessage();
        if (string.IsNullOrEmpty(receivedString) || receivedString is "__Connected__" or "__Disconnected__") return;
        datas = receivedString.Split(',');

        //receiving value order --> right/left/top/bottom
        for (int i = 0; i < datas.Length; i++) {
            _incomeValues[i] = int.Parse(datas[i]);
            _incomeValues[i] = Mathf.Clamp(_incomeValues[i], minClampValue, maxClampValue);
        }

        UpdateLights();

        _finalValueX = _incomeValues[0] - _incomeValues[1];
        _finalValueZ = _incomeValues[2] - _incomeValues[3];
        if (Mathf.Abs(_finalValueX) < minimumMovementTreshold) _finalValueX = 0;
        if (Mathf.Abs(_finalValueZ) < minimumMovementTreshold) _finalValueZ = 0;
        
        //apply move values to directional vector
        _moveValue = new Vector3(_finalValueX, 0, _finalValueZ).normalized * (Time.deltaTime * sensivity);
        _rb.AddForce(_moveValue);

        // Rotation
        Quaternion targetRotation = Quaternion.LookRotation(_rb.velocity * -1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, smoothRotationValue * Time.deltaTime);
    }

    void UpdateLights() {
        Color tempColor;
        tempColor = rightLight.color;
        tempColor.a = Remap(_incomeValues[0], minClampValue, maxClampValue, 0f, maxStreetLampAlpha);
        rightLight.color = tempColor;

        tempColor = leftLight.color;
        tempColor.a = Remap(_incomeValues[1], minClampValue, maxClampValue, 0f, maxStreetLampAlpha);
        leftLight.color = tempColor;

        tempColor = topLight.color;
        tempColor.a = Remap(_incomeValues[2], minClampValue, maxClampValue, 0f, maxStreetLampAlpha);
        topLight.color = tempColor;

        tempColor = downLight.color;
        tempColor.a = Remap(_incomeValues[3], minClampValue, maxClampValue, 0f, maxStreetLampAlpha);
        downLight.color = tempColor;
    }

    float Remap(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}