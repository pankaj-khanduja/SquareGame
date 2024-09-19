using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using DigitalRuby.AnimatedLineRenderer;
public class LineRendererManager : MonoBehaviour
{
    private LineRenderer lineRenderer = null;
    GameObject square1, square2 , lastSelectedSquare;
    private float touchStayTime = Constant.range1; // Time in seconds to detect the touch stay
    private bool isTouching = false;
    private float touchDuration = 0.0f;

    void Start()
    {
        lastSelectedSquare = null;
        SquareController.Instance.onReset += RemoveLines;
        // Get the LineRenderer component attached to the same GameObject
    }

    private void OnDisable()
    {
        if(SquareController.Instance == null) return;
        SquareController.Instance.onReset -= RemoveLines;
    }
    void Update()
    {
        if (SquareController.Instance.isGameOver) return;
        // Check if there's at least one touch
        // if (Input.touchCount > 0)
        // {
        //     Touch touch = Input.GetTouch(0);

        //     // Check if the touch is stationary
        //     if (touch.phase == TouchPhase.Stationary)
        //     {
        //         Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //         RaycastHit hit;

        //         // Check if the ray hits this object
        //         if (Physics.Raycast(ray, out hit) && hit.collider == GetComponent<Collider>())
        //         {
        //             if (!isTouching)
        //             {
        //                 isTouching = true;
        //                 touchDuration = 0.0f; // Reset the touch duration
        //             }

        //             // Increment the touch duration
        //             touchDuration += Time.deltaTime;

        //             // Check if the touch has stayed long enough
        //             if (touchDuration >= touchStayTime)
        //             {
        //                 Debug.Log("Touch stayed over the object for " + touchStayTime + " seconds!");
        //                 // Perform your desired action here

        //                 // Optionally reset the touch duration if you want to detect it again
        //                 touchDuration = 0.0f;
        //             }
        //         }
        //         else
        //         {
        //             ResetTouch();
        //         }
        //     }
        //     else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        //     {
        //         ResetTouch();
        //     }
        // }
        // else
        // {
        //     ResetTouch();
        // }



        if (Input.GetMouseButtonDown(0))
        {
            CheckForSquareOne();
        }
        else if (Input.GetMouseButton(0) && lineRenderer != null )
        {
            Vector3 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousPos.z = 0;
            lineRenderer.SetPosition(1, mousPos);
            square2 = SquareController.Instance.IsPointerOverlappingAnySquare(mousPos);
            if (square1 != null && square2 != null && !square2.Equals(square1) )
            {
                touchDuration += Time.deltaTime;
               
                if (touchDuration >= touchStayTime)
                {
                    touchDuration = 0;
                    if (lastSelectedSquare != square2 &&  SquareController.Instance.GetManager().IsCorrectSquare(square2, true))
                    {
                        lastSelectedSquare = null;
                        lineRenderer.SetPosition(1, square2.transform.position);
                        lineRenderer = null;
                        square1 = null;
                        square2 = null;
                        SquareController.Instance.GetManager().CheckForRoundCompletion();
                    }
                    else
                    {
                        lastSelectedSquare = square2;
                    }
                }
            }
            else
            {
                touchDuration = 0;
                if (square1 == null)
                {
                    CheckForSquareOne();
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (square1 == null && square2 == null && lineRenderer == null)
            {
                CheckForSquareOne();
            }
        }
        
        else if (Input.GetMouseButtonUp(0) && lineRenderer != null)
        {
            Vector3 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousPos.z = 0;
            square2 = SquareController.Instance.IsPointerOverlappingAnySquare(mousPos);
            if (square1 != null && square2 != null && !square2.Equals(square1) && SquareController.Instance.GetManager().IsCorrectSquare(square2, true))
            {
                lineRenderer.SetPosition(1, square2.transform.position);
                lineRenderer = null;
                square1 = null;
                SquareController.Instance.GetManager().CheckForRoundCompletion();
            }
            else
            {
                SquareController.Instance.GetManager().UndoStep(square1);
                Destroy(lineRenderer.gameObject);
            }

        }
    }

    void CreateLine(Vector3 mousePos)
    {
        lineRenderer = new GameObject("line").AddComponent<LineRenderer>();
        lineRenderer.SetPosition(0, mousePos);
        lineRenderer.SetPosition(1, mousePos);
        // Set the number of positions in the LineRenderer
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.material = SquareController.Instance.lineRendererMaterial;
     
        // lineRenderer.SetColors(Color.black, Color.black);
        lineRenderer.sortingOrder = -1;
    }

    public void RemoveLines()
    {
        LineRenderer[] lineRenderers = (LineRenderer[])GameObject.FindObjectsOfType(typeof(LineRenderer));
        foreach (var item in lineRenderers)
        {
            Destroy(item.gameObject);
        }
        Destroy(this.gameObject);

    }

    private void ResetTouch()
    {
        isTouching = false;
        touchDuration = 0.0f;
    }

    void CheckForSquareOne()
    {
        Vector3 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousPos.z = 0;
        square1 = SquareController.Instance.IsPointerOverlappingAnySquare(mousPos);
        // if(square1.GetComponent<SquarePrefab>().isSelected) return;
        if (square1 != null && lineRenderer == null && SquareController.Instance.GetManager().IsCorrectSquare(square1, false))
        {
            CreateLine(square1.transform.position);
            // lineRenderer.SetPosition(0, mousPos);
        }
    }

}
