using System.IO.Ports;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class MothController : MonoBehaviour
{
    SerialPort data_stream = new SerialPort("COM3", 9600);
    
    [Header("Debug Value")]
    public string receivedString;
    
    [SerializeField] private string[] datas = new string[4];

    [Header("Value that can be modified")]
    [SerializeField] private float sensivity = .1f;
    [SerializeField] private int minClampValue = 80;
    [SerializeField] private int maxClampValue = 500;

    private Rigidbody _rb;
    private int[] _incomeValues = new int[4];
    private Vector3 _moveValue;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //data_stream.Open();
    }

    private void Update()
    {
        // receivedString = data_stream.ReadLine();
        // datas = receivedString.Split(',');
        //
        // //receiving value order --> left/right/top/bottom
        // for (int i = 0; i < datas.Length; i++)
        // {
        //     _incomeValues[i] = int.Parse(datas[i]);
        //     _incomeValues[i] = Mathf.Clamp(_incomeValues[i], minClampValue, maxClampValue);
        // }
        
        // //apply move values to directional vector
        // _moveValue = new Vector3(_incomeValues[0] - _incomeValues[1], 0, _incomeValues[3] - _incomeValues[4]);
        // _moveValue *= Time.deltaTime * sensivity;
        // _rb.AddForce(_moveValue);
    }
}