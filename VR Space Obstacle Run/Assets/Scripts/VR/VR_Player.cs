using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VR_Player : InteractionHandler {

    public Ship ship;

    private void FixedUpdate()
    {
        switch (interactionMode)
        {
            case InteractionMode.Looking:
                LookingMode();
                break;
            case InteractionMode.UI:
                UIMode();
                break;
            case InteractionMode.Flying:
                FlyingMode();
                break;
        }
    }

    public void UIMode()
    {
        RaycastHit uiHit = GameManager.instance.activeController.Shoot3DRaycast(10, 5);

        if (uiHit.transform != null)
        {
            print("not null");
            GameManager.instance.activeController.ShowLineRenderer(uiHit.point, Color.blue);

            if (SteamVR_Input.Spaceship.inActions.UI_Interact.GetStateDown(GameManager.instance.activeInput))
            {
                print("Button Pressed");
                GameManager.instance.activeController.UIInteract(uiHit);
            }
        }
        else
        {
            GameManager.instance.activeController.DisableLineRenderer();
        }

        if (SteamVR_Input.Spaceship.inActions.Pause.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (GameManager.instance.paused)
            {
                GameManager.instance.TogglePause(false);
                interactionMode = InteractionMode.Flying;
            }
        }
    }

    public void LookingMode()
    {
        if (SteamVR_Input.Spaceship.inActions.Interact.GetStateDown(GameManager.instance.activeInput))
        {
            bool success = false;
            GameManager.instance.activeController.TryToInteract(out success);

            if (success)
            {
                GameManager.instance.TogglePause(false);
                interactionMode = InteractionMode.Flying;
            }
        }
    }

    public void FlyingMode()
    {
        if (SteamVR_Input.Spaceship.inActions.Pause.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (!GameManager.instance.paused)
            {
                GameManager.instance.TogglePause(true);
                interactionMode = InteractionMode.UI;
            }
        }

        if (SteamVR_Input.Spaceship.inActions.Firing.GetStateDown(GameManager.instance.activeInput))
        {
            print("Shoot");
            ship.Shoot();
        }
    }
}
