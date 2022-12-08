using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Properies")]
    public Vector2 direction;
    public Rigidbody2D rigidbody2D;

    [Range(1.0f, 100.0f)]
    public float force;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        direction = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized;
        Activate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Rotate();
    }

    public void Activate()
    {
        Rotate();
        Move();
        Invoke("DestroyYourself", 2.0f);
    }

    private void Rotate()
    {
        rigidbody2D.AddTorque(Random.Range(5.0f, 15.0f) * direction.x * -1.0f, ForceMode2D.Impulse);
        //transform.RotateAround(transform.position, Vector3.forward, 6.0f);
    }

    private void Move()
    {
        rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void DestroyYourself()
    {
        if (gameObject.activeInHierarchy)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DestroyYourself();
        }
        
    }
}
