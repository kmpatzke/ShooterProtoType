using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerGrap : MonoBehaviour {

    private GetController getController;
    private SteamVR_Controller.Device currentController;
    public Transform pickup;
    private bool stayInHand; // Wird das Objekt nur so lange gehalten, wie der Trigger gedrückt wird?



    private void Start()
    {
        getController = GetComponentInParent<GetController>();
    }




    private void Update()
    {

        GetCurrentController(); // Erstma wieder Controller festlegen


        
        #region interact with object
        if (currentController != null && pickup != null && stayInHand && currentController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) // Muss am Anfang hier stehen, da sonst der Trigger bereits ein Mal ausgelöst würde
        {
            if (currentController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                var scriptAccess = pickup.GetComponent<PickableObject>();
                scriptAccess.interact();
            }
        }

        #endregion//_______________________________________________________________________



        if (currentController != null)
        {
            #region controllerVisible
            #endregion
            #region das Objekt aufheben
            // Objekt aufnehmen und festlegen, ob es ein gehaltenes Objekt ist

            if (pickup == null && currentController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
               
                reachableObjects();
                if (pickup != null)
                {

                    var scriptAccess = pickup.GetComponent<PickableObject>();
                    stayInHand = scriptAccess.stayInHand; // gleich loslassen oder halten
                    scriptAccess.switchPhysics();
                    pickup.SetParent(transform);
                    scriptAccess.takePosition();

                }
            }
            #endregion//_____________________________________________________________________________

            #region Objekt fallen lassen, wenn es nicht interaktiv ist
            // Wenn das Objekt kein interaktives ist, dann muss der Trigger losgelassen werden, um das Objekt fallen zu lassen
            else if (pickup != null && !stayInHand && currentController.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (pickup != null)
                {

                    var scriptAccess = pickup.GetComponent<PickableObject>();
                    dropObject();
                    scriptAccess.addVelocity(currentController.velocity, currentController.angularVelocity); // Methode des Objects
                    

                }
            }
            #endregion//___________________________________________________________________________________

            #region Objekt fallen lassen, wenn es interaktiv ist

            //Wenn das Objekt ein interaktives ist, dann muss der Grip Button gedrückt werden, um das Objekt fallen zu lassen
            else if (pickup!= null && stayInHand && currentController.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                if (pickup != null) // Überprüft, ob es ein Gegenstand ist, der gleich wieder losgelassen wird
                {

                    var scriptAccess = pickup.GetComponent<PickableObject>();
                    dropObject();
                    scriptAccess.addVelocity(currentController.velocity, currentController.angularVelocity); // Methode des Objects
                    

                }
            }
            #endregion//_____________________________________________________________________________________
        }



    }



    //Mit dieser Funktion wird überprüft, ob beim Drücken des Triggers eine greifbares Objekt da ist __ Alternative war mit Triggern arbeiten... hat nicht gut geklappt
    private void reachableObjects()
    {
        
        Collider[] reachables = Physics.OverlapSphere(transform.position, 0.09f);
        foreach (Collider col in reachables)
        {
            var scriptInParent = col.GetComponentInParent<PickableObject>();
            var scriptInChildren = col.GetComponent<PickableObject>();
            
            if(scriptInParent != null)
            {
                pickup = scriptInParent.gameObject.transform;
                break;
            }

            else if(scriptInChildren != null)
            {
                pickup = scriptInChildren.gameObject.transform;
                break;
            }
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

    public void dropObject()
    {
        foreach(Transform child in transform)
        {
            
            if(child.tag != "controllerModel")
            {
                child.transform.SetParent(null);
                var script = child.GetComponentInChildren<PickableObject>();
                script.switchPhysics();
                pickup = null;
            }
        }
    }

    










}
