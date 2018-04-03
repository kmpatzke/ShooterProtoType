using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickableObject : MonoBehaviour {

    public bool stayInHand; // Ob ein Object nach dem greifen in der Hand bleibt oder nicht
    public bool hasRelativePosition;
    public Vector3 relativePosition; // Wird vom Controller GameObject abgefragt
    public Vector3 relativeRotation;

    public UnityEvent interaction;

    private bool physics = true;

    public bool isHeld;



    private void Update()
    {
        if (transform.parent == null)
        {
            isHeld = false;
        }
        else
        {
            isHeld = true;
        }
    }

    public void takePosition()
    {
        if (hasRelativePosition) // Die Funktion verändert die Position nur, wenn eine Korrektur benötigt wird
        {
            transform.localPosition = relativePosition;
            transform.localRotation = Quaternion.Euler(relativeRotation);
        }
        
    }

    public void switchPhysics()
    {
        if (physics == true)
        {
            GetComponentInChildren<Rigidbody>().isKinematic = true;
            physics = false;

            

            physics = false;
        }
        else
        {
            GetComponentInChildren<Rigidbody>().isKinematic = false;
            physics = true;

        }
        
    }

    public void switchCollider()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            if(col.enabled == true)
            {
                col.enabled = false;
                Debug.Log("Collider switched");
            }
            else
            {
                col.enabled = true;
            }
            
        }
    }


    public void addVelocity(Vector3 vel, Vector3 angVel)
    {
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = vel;
        rigidBody.angularVelocity = angVel;
        rigidBody.maxAngularVelocity = rigidBody.angularVelocity.magnitude;
    }

    public void interact()
    {
        
        interaction.Invoke();
    }

    public void switchVisibility()
    {
        var mesh = GetComponent<MeshRenderer>();
        
        if(mesh.enabled == true)
        {
            mesh.enabled = false;
        }

        else
        {
            mesh.enabled = true;
        }
    }
}
