using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VR_Controller : MonoBehaviour {

    public Collider enteredCollider;
    public LineRenderer uiLineRenderer;

    public void UIInteract(RaycastHit uiHit)
    {
        if (uiHit.transform != null)
        {
            if (uiHit.transform.GetComponent<Button>() != null)
            {
                Button b = transform.GetComponent<Button>();
                b.onClick.Invoke();
            }
        }
    }

    public void ShowLineRenderer(Vector3 hitPosition, Color lineColor)
    {
        //Uses the new Line Renderer's "Set Positions", though when tested resulted in a flickering line.
        //Vector3[] positions = new Vector3[] { transform.position, hitPosition };
        //uiLineRenderer.SetPositions(positions);

        float distance = Vector3.Distance(transform.position, hitPosition);
        uiLineRenderer.transform.localScale = new Vector3(uiLineRenderer.transform.localScale.x, uiLineRenderer.transform.localScale.y, distance);
        uiLineRenderer.startColor = lineColor;
        uiLineRenderer.endColor = lineColor;
    }

    public void ShowLineRenderer(Vector3 hitPosition, Color startColor, Color endColor)
    {
        //Uses the new Line Renderer's "Set Positions", though when tested resulted in a flickering line.
        //Vector3[] positions = new Vector3[] { transform.position, hitPosition };
        //uiLineRenderer.SetPositions(positions);

        float distance = Vector3.Distance(transform.position, hitPosition);
        uiLineRenderer.transform.localScale = new Vector3(uiLineRenderer.transform.localScale.x, uiLineRenderer.transform.localScale.y, distance);
        uiLineRenderer.startColor = startColor;
        uiLineRenderer.endColor = endColor;
    }

    public RaycastHit Shoot3DRaycast(float range, LayerMask mask)
    {
        RaycastHit toReturn;
        Physics.Raycast(transform.position, transform.forward, out toReturn, range);

        return toReturn;
    }

    public void TryToInteract(out bool success)
    {
        if (enteredCollider != null)
        {
            success = true;
            enteredCollider.GetComponent<InteractableObject>().Interact();
            return;
        }

        success = false;
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            enteredCollider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enteredCollider = null;
    }
}
