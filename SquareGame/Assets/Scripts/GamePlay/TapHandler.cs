using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapHandler : MonoBehaviour
{
    void Start()
    {
        SquareController.Instance.onReset += DestroyTap;
        // Get the LineRenderer component attached to the same GameObject
    }

    private void OnDisable()
    {
        if (SquareController.Instance == null) return;
        SquareController.Instance.onReset -= DestroyTap;
    }

    void DestroyTap()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            DetectTap(Input.GetTouch(0).position);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            DetectTap(Input.mousePosition);
        }
    }

    void DetectTap(Vector3 inputPosition)
    {
        // Create a ray from the camera to the tap position
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        Vector2 worldPoint2D = new Vector2(worldPoint.x, worldPoint.y);

        // Perform a raycast in 2D space
        RaycastHit2D hit = Physics2D.Raycast(worldPoint2D, Vector2.zero);

        // Check if the raycast hit a collider
        if (hit.collider != null)
        {
            SquareController.Instance.GetManager().IsCorrectSquare(hit.collider.gameObject, false);
            Debug.Log("Tapped on 2D object: " + hit.collider.name);
            // You can perform some action on the hit object here
        }
    }
}
