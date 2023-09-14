using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField] Vector3 rotationAxis = new Vector3(0,1,0);
    [SerializeField] private float rotaSpeed=3f;

    private float t;

    void Update()
    {
        transform.Rotate(rotationAxis, rotaSpeed);
    }
}
