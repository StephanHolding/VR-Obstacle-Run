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
                Button b = uiHit.transform.GetComponent<Button>();
                b.onClick.Invoke();
            }
        }
    }

    public void ShowLineRenderer(Vector3 hitPosition, Color lineColor)
    {
        Vector3[] positions = new Vector3[] { transform.position, hitPosition };
        uiLineRenderer.SetPositions(positions);
        uiLineRenderer.startColor = lineColor;
        uiLineRenderer.endColor = lineColor;
    }

    public void ShowLineRenderer(Vector3 hitPosition, Color startColor, Color endColor)
    {
        Vector3[] positions = new Vector3[] { transform.position, hitPosition };
        uiLineRenderer.SetPositions(positions);
        uiLineRenderer.startColor = startColor;
        uiLineRenderer.endColor = endColor;
    }

    public void DisableLineRenderer()
    {
        Vector3[] positions = new Vector3[] { Vector3.zero, Vector3.zero };
        uiLineRenderer.SetPositions(positions);
    }

    public RaycastHit Shoot3DRaycast(float range, LayerMask mask)
    {
        RaycastHit toReturn;
        Physics.Raycast(transform.position, transform.forward, out toReturn, range, mask);

        return toReturn;
    }

    public RaycastHit Shoot3DRaycast(float range, int layer)
    {
        LayerMask mask = (1 << layer);
        RaycastHit toReturn;
        Physics.Raycast(transform.position, transform.forward, out toReturn, range, mask);

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
