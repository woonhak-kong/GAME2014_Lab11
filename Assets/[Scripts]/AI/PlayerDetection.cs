using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public LayerMask CollisionLayerMask;
    public bool playerDetected;
    public bool LOS;
    public Transform playerTransform;

    public Vector2 DirectionToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindObjectOfType<PlayerBehavior>().transform;
        LOS = false;
        playerDetected = false;
        DirectionToPlayer = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            LOS = Physics2D.Linecast(transform.position, playerTransform.position, CollisionLayerMask);

            Vector2 direction = playerTransform.position - transform.position;
            DirectionToPlayer = direction.x < 0 ? Vector2.left : Vector2.right;
            
            if (DirectionToPlayer.x != transform.parent.localScale.x)
            {
                LOS = true;
            }
            else
            {
                LOS = false;
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            playerDetected = false;
            LOS = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = playerDetected ? Color.red : Color.green;

        Gizmos.DrawWireSphere(transform.position, 15.0f);

        if (LOS)
        {
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }

    }
}
