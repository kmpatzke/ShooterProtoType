using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class ColliderManager : MonoBehaviour {

    public Transform headtransform;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 pos = transform.InverseTransformPoint(headtransform.position);

        characterController.height = pos.y;

        Vector3 center = Vector3.zero; // Eigentlich ein bisschen dicke für des Center extra ne neue var anzulegen
        center.x = pos.x;
        center.z = pos.z;

        center.y = characterController.height / 2;
        characterController.center = center;
    }
}
