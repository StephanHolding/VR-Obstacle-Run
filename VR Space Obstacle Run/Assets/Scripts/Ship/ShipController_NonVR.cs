using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController_NonVR : ShipControl {

    [Header("Control proporties")]
    public Ship ship;
    public Transform rotationXYObject;
    public Transform rotationObjectOrigin;
    public float shipRotateSpeed;
    public float shipTranslateSpeed;


    private void FixedUpdate()
    {
        Steer();
    }

    protected override void Steer()
    {
        Pitch();
        Jaw();

        ship.transform.rotation = Quaternion.LookRotation(rotationXYObject.position);

        Vector3 posClamp = ship.transform.position;
        posClamp.x = Mathf.Clamp(posClamp.x, -20, 20);
        posClamp.y = Mathf.Clamp(posClamp.y, -20, 20);

        ship.transform.position = posClamp;
    }

    protected override void Pitch()
    {
        float pitch = Input.GetAxis("Pitch");

        rotationXYObject.position = Vector3.MoveTowards(rotationXYObject.position, new Vector3(rotationXYObject.position.x, 5 * pitch, rotationXYObject.position.z), shipRotateSpeed * Time.deltaTime);
        ship.transform.Translate(new Vector3(0, 1, 0) * shipTranslateSpeed * pitch * Time.deltaTime, Space.World);
    }

    protected override void Jaw()
    {
        float jaw = Input.GetAxis("Jaw");

        rotationXYObject.position = Vector3.MoveTowards(rotationXYObject.position, new Vector3(5 * jaw, rotationXYObject.position.y, rotationXYObject.position.z), shipRotateSpeed * Time.deltaTime);
        ship.transform.Translate(new Vector3(1, 0, 0) * shipTranslateSpeed * jaw * Time.deltaTime, Space.World);
    }

}
