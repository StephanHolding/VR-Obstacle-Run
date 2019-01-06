using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController_VR : ShipControl {

    [Header("Control proporties")]
    public Ship ship;
    public Transform rotationXYObject;
    public Transform rotationObjectOrigin;
    public float shipRotateSpeed;
    public float shipTranslateSpeed;

    private bool active;


    private void FixedUpdate()
    {
        if (active)
        Steer();
    }

    protected override void Steer()
    {
        Vector3 controllerRotation = GameManager.instance.activeController.transform.localEulerAngles;

        Pitch(controllerRotation);
        Jaw(controllerRotation);

        ship.transform.rotation = Quaternion.LookRotation(rotationXYObject.position);

        Vector3 posClamp = ship.transform.position;
        posClamp.x = Mathf.Clamp(posClamp.x, -20, 20);
        posClamp.y = Mathf.Clamp(posClamp.y, -20, 20);

        ship.transform.position = posClamp;
    }

    protected override void Pitch(Vector3 controllerRotation)
    {
        //float pitch = SteamVR_Input.Spaceship.inActions.Flying.GetAxis(GameManager.instance.activeController).y;

        float pitch = HierarchyHelp.CreateAxis(controllerRotation.x, -50, 50);

        rotationXYObject.position = Vector3.MoveTowards(rotationXYObject.position, new Vector3(rotationXYObject.position.x, 5 * -pitch, rotationXYObject.position.z), shipRotateSpeed * Time.deltaTime);
        ship.transform.Translate(new Vector3(0, 1, 0) * shipTranslateSpeed * pitch * Time.deltaTime, Space.World);
    }

    protected override void Jaw(Vector3 controllerRotation)
    {
        //float jaw = SteamVR_Input.Spaceship.inActions.Flying.GetAxis(GameManager.instance.activeController).x;

        float jaw = HierarchyHelp.CreateAxis(controllerRotation.z, -50, 50);

        rotationXYObject.position = Vector3.MoveTowards(rotationXYObject.position, new Vector3(5 * -jaw, rotationXYObject.position.y, rotationXYObject.position.z), shipRotateSpeed * Time.deltaTime);
        ship.transform.Translate(new Vector3(1, 0, 0) * shipTranslateSpeed * jaw * Time.deltaTime, Space.World);
    }

    public void LockSteering(bool toggle)
    {
        if (toggle)
            active = false;
        else
            active = true;
    }
}
