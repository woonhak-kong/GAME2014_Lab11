using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    
    public PlatformDirection direction;

    [Header("Movement Properties")]
    [Range(1f, 20f)]
    public float horizontalDistance = 8.0f;
    [Range(1f, 20f)]
    public float horizontalSpeed = 1.0f;
    [Range(1f, 20f)]
    public float verticalDistance = 0.0f;
    [Range(1f, 20f)]
    public float verticalSpeed = 1.0f;

    [Header("Platform Path Points")]
    public List<Vector2> points;
    [Range(0.0f, 2.0f)]
    public float customSpeedFactor = 1.0f;

    private float lerpInterp4Custom = 0.0f;
    private Vector2 startPoint;

    private float timer = 0.0f;

    private int currentPointIdx = 0;
    private int nextPointIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        currentPointIdx = 0;
        nextPointIdx = currentPointIdx + 1 >= points.Count ? 0 : currentPointIdx + 1;
    }

    // Update is called once per frame
    void Update()
    {
        CalculCustomInterp();
        Move();
    }

    private void CalculCustomInterp()
    {
        if (lerpInterp4Custom > 1.0f)
        {
            lerpInterp4Custom = 0.0f;
            currentPointIdx++;
            if (currentPointIdx >= points.Count)
            {
                currentPointIdx = 0;
            }
            nextPointIdx = currentPointIdx + 1 >= points.Count ? 0 : currentPointIdx + 1;
        }
    }

    private void Move()
    {
        
        switch (direction)
        {
            case PlatformDirection.HORIZONTAL:
                transform.position = new Vector3(Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPoint.x, startPoint.y, 0.0f);
                break;
            case PlatformDirection.VERTICAL:
                transform.position = new Vector3(startPoint.x, Mathf.PingPong(verticalSpeed * Time.time, horizontalDistance) + startPoint.y, 0.0f);
                break;
            case PlatformDirection.DIAGONAL_UP:
                transform.position = new Vector3(
                    Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPoint.x,
                    Mathf.PingPong(verticalSpeed * Time.time, horizontalDistance) + startPoint.y, 0.0f);
                break;
            case PlatformDirection.DIAGONAL_DOWN:
                transform.position = new Vector3(
                    Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPoint.x,
                    startPoint.y - Mathf.PingPong(verticalSpeed * Time.time, horizontalDistance), 0.0f);
                break;
            case PlatformDirection.CUSTOM:
                lerpInterp4Custom += customSpeedFactor * Time.deltaTime;
                //transform.position = new Vector3(
                //    Mathf.Lerp(startPoint.x, startPoint.x + points[currentPointIdx].x, lerpInterp4Custom),
                //    Mathf.Lerp(startPoint.y, startPoint.y + points[currentPointIdx].y, lerpInterp4Custom),
                //    0.0f);
                transform.position = Vector2.Lerp(points[currentPointIdx], points[nextPointIdx], lerpInterp4Custom);

                break;
        }
    }
}
