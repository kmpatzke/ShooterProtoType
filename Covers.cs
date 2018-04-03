using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Covers : MonoBehaviour {


    [SerializeField] bool ocuppied = false;

    public bool Ocuppied
    {
        get
        {
            return ocuppied;
        }

        set
        {
            ocuppied = value;
        }
    }
}
