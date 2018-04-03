using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolMagazine : MonoBehaviour {


    [SerializeField] private int bullets = 15;


    public void DecreaseBullets()
    {
        if(bullets > 0)
        {
            bullets -= 1;
        }
        
    }


    public int Bullets
    {
        get
        {
            return bullets;
        }

    }


}
