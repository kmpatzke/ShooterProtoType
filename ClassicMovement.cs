using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicMovement : MonoBehaviour {

    [SerializeField] Transform headset;
    private GetController getController;
    private SteamVR_Controller.Device currentController;
    private CharacterController charCon;

    public float walkSpeed = 2;
    public float strafeSpeed = 2;

    
	


	void Start () {

        charCon = GetComponentInParent<CharacterController>(); // Weil dieses Skript kommt auf die einzelnen Controller
        getController = GetComponentInParent<GetController>();

        
        
	}
	
	
	void Update () {

        if(currentController == null) // Ist im ersten Frame noch nicht da, deshalb muss das in die Update
        {
            GetCurrentController();
            
        }

        if (currentController != null)
        {
            Vector2 touchInput = currentController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            float sideStep = touchInput.x;
            float forward = touchInput.y;

            Vector3 move = headset.forward * forward * walkSpeed + headset.right * sideStep * strafeSpeed;
            charCon.SimpleMove(move);


        }

    }

   

    void GetCurrentController()
    {
        try
        {
            if (gameObject.name.Contains("right"))
            {
                currentController = getController.rightController;
            }
            else if (gameObject.name.Contains("left"))
            {
                currentController = getController.leftController;
            }
        }
        catch
        {
            currentController = null;
        }
        
    }
}
