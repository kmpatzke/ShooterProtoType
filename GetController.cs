using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetController : MonoBehaviour {

    int controllerLeftID = -1;
    int controllerRightID = -1;

    public SteamVR_Controller.Device rightController;
    public SteamVR_Controller.Device leftController;

   
	void Update () {

        getControllers();
    
	}

    public void getControllers()
    {
        if(controllerLeftID == -1)
        {
            controllerLeftID = (int)GetComponentInChildren<SteamVR_ControllerManager>().left.GetComponent<SteamVR_TrackedObject>().index;
            
        }
        if (controllerRightID == -1)
        {
            controllerRightID = (int)GetComponentInChildren<SteamVR_ControllerManager>().right.GetComponent<SteamVR_TrackedObject>().index;
        }

        if(controllerLeftID != -1)
        {
            this.leftController = SteamVR_Controller.Input(controllerLeftID);
        }
        if(controllerRightID != -1)
        {
            this.rightController = SteamVR_Controller.Input(controllerRightID);
        }
    }

   


    
    
}
