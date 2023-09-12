using UnityEngine;

public class MothController : MonoBehaviour
{
    [Header("Debug Value")]
    [SerializeField] private string receivedString;
    [SerializeField] private string[] datas = new string[4];

    [Header("Value that can be modified")]
    [SerializeField] private float sensivity = .1f;
    [SerializeField] private int minClampValue = 80;
    [SerializeField] private int maxClampValue = 500;
    [SerializeField] private int minimumMovementTreshold = 20;

    private SerialController _serialController;
    private Rigidbody _rb;
    private int[] _incomeValues = new int[4];
    private Vector3 _moveValue;
    private int _finalValueX;
    private int _finalValueZ;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _serialController = GetComponent<SerialController>();
    }

    private void Update()
    {
        receivedString = _serialController.ReadSerialMessage();
        if(string.IsNullOrEmpty(receivedString) || receivedString is "__Connected__" or "__Disconnected__") return;
        datas = receivedString.Split(',');
        
        //receiving value order --> left/right/top/bottom
        for (int i = 0; i < datas.Length; i++)
        {
            _incomeValues[i] = int.Parse(datas[i]);
            _incomeValues[i] = Mathf.Clamp(_incomeValues[i], minClampValue, maxClampValue);
        }

        _finalValueX = _incomeValues[0] - _incomeValues[1];
        _finalValueZ = _incomeValues[2] - _incomeValues[3];
        if (_finalValueX < minimumMovementTreshold) _finalValueX = 0;
        if (_finalValueZ < minimumMovementTreshold) _finalValueZ = 0;
        
        //apply move values to directional vector
        _moveValue = new Vector3(_finalValueX, 0, _finalValueZ) * (Time.deltaTime * sensivity);
        _rb.AddForce(_moveValue);
    }
}