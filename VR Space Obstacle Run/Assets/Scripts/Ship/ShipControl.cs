using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipControl : MonoBehaviour {

    protected abstract void Steer();

    protected virtual void Pitch(){ }
    protected virtual void Jaw(){ }
    protected virtual void Pitch(Vector3 controllerRotation) { }
    protected virtual void Jaw(Vector3 controllerRotation) { }

}
