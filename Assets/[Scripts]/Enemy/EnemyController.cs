using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("Player Movement")]
    public float horizontalSpeed;

    public Transform inFrontPoint;
    public Transform aheadPoint;
    public Transform groundPoint;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public bool isGroundAhead;
    public bool isObstacleAhead;
    public bool isGrounded;

    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        isObstacleAhead = Physics2D.Linecast(groundPoint.position, inFrontPoint.position, groundLayerMask);
        isGroundAhead = Physics2D.Linecast(groundPoint.position, aheadPoint.position, groundLayerMask);
        Collider2D hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;

        if (isGrounded)
        {
            Move();
        }

        if (!isGroundAhead   || isObstacleAhead)
        {
            Flip();
        }
        
    }

    public void Move()
    {
        transform.position += new Vector3(direction.x * horizontalSpeed * Time.deltaTime, 0.0f);
    }

    public void Flip()
    {
        float x = transform.localScale.x * -1.0f;
        direction *= -1.0f;
        transform.localScale = new Vector3(x, 1.0f, 1.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
        Gizmos.DrawLine(groundPoint.position, inFrontPoint.position);
        Gizmos.DrawLine(groundPoint.position, aheadPoint.position);
    }
}
