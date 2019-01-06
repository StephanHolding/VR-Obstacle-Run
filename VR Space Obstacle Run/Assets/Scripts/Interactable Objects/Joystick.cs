using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : InteractableObject {

    public Ship ship;
    private bool active;

    public override void Interact()
    {
        ship.LockControls(false);
        active = true;
    }

    private void Update()
    {
        if (active)
        {
            transform.localRotation = ship.transform.localRotation;
        }
    }
}
