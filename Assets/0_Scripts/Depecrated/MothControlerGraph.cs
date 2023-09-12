using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MothControlerGraph : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float  acceleration = 2f;
    [SerializeField] private float  maxSpeed = 20f;
    [SerializeField] private float  bouncePower = 2f;

    [SerializeField] private KeyCode keycodeUp = KeyCode.Y;
    [SerializeField] private KeyCode keycodeDown = KeyCode.B;
    [SerializeField] private KeyCode keycodeRight = KeyCode.M;
    [SerializeField] private KeyCode keycodeLeft = KeyCode.Q;

    [SerializeField] private GameObject haut;
    [SerializeField] private GameObject bas;
    [SerializeField] private GameObject gauche;
    [SerializeField] private GameObject droite;

    [SerializeField] private bool useBounce;


    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        haut.SetActive(true);
        bas.SetActive(true);
        gauche.SetActive(true);
        droite.SetActive(true);

        Vector2 pos = new Vector2(0, 0);

        if (Input.GetKey(keycodeUp))
        {
            pos = new Vector2(pos.x, pos.y - 1);
            haut.SetActive(false);
        }
        if (Input.GetKey(keycodeDown))
        {
            pos = new Vector2(pos.x, pos.y +1 );
            bas.SetActive(false);
        }
        if (Input.GetKey(keycodeRight))
        {
            pos = new Vector2(pos.x-1, pos.y);
            droite.SetActive(false);
        }
        if (Input.GetKey(keycodeLeft))
        {
            pos = new Vector2(pos.x+1, pos.y);
            gauche.SetActive(false);
        }

        pos = pos * Time.deltaTime * acceleration;
        //Vector3 pos = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical")).normalized * Time.deltaTime * speed;
        _rb.AddForce(new Vector3(pos.x,0,pos.y));

        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!useBounce) return;

        _rb.AddForce(Vector3.Reflect(_rb.velocity, collision.contacts[0].normal) * bouncePower* _rb.velocity.magnitude);
        Debug.Log("aaaaa");
    }
}
