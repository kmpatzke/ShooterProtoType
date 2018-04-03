using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMechanics : MonoBehaviour {

    [SerializeField] Transform animationMagazine;
    [SerializeField] Transform magazinPositionDown;
    [SerializeField] Transform magazinPositionUp;
    [SerializeField] int accuracy;
    Transform[] points;

    //ReloadAnimation reloadAnimation;
    Transform magazin;
    
    
    private enum loadinState { LOADING, NOTLOADING }
    private loadinState currentLoadingState;

    [SerializeField] GameObject animationControllPoint;
    [SerializeField] Transform controller;

    [SerializeField] private Colt colt;

    private void Start()
    {
        //reloadAnimation = GetComponent<ReloadAnimation>();
        points = new Transform[accuracy];
        CurrentLoadingState = loadinState.NOTLOADING;
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "loadTrigger" && colt.IsLoaded == false) // Check if it is a Trigger of a Magazine
        {
            Transform temp = other.transform.parent; // the parent is the actual Magazine
            if(temp.parent != null) // Check if Magazine is hold by a Controller, otherwhise we don't want interaction
            {
                magazin = temp;
                controller = magazin.parent;
                CurrentLoadingState = loadinState.LOADING;
            }
            
        }
    }


    IEnumerator loading()
    {
        #region switch magazins-----------------------------------------------------------------------------------------
        magazin.gameObject.SetActive(false); //Das aufgehobene Magazin unsichtbar machen und durch die Animation ersetzen
        animationMagazine.gameObject.SetActive(true);
        #endregion------------------------------------------------------------------------------------------------------

        #region create controllpoint------------------------------------------------------------------------------------
        if (animationControllPoint == null) // Creating a point that controlls the animations
        {
            animationControllPoint = new GameObject("controllPoint");
            animationControllPoint.transform.position = magazinPositionDown.position;
            animationControllPoint.transform.SetParent(controller);
        }
        #endregion------------------------------------------------------------------------------------------------------

        #region canel loading setup-------------------------------------------------------------------------------------

        float distance = Vector3.Distance(animationControllPoint.transform.position, magazinPositionUp.position);

        #endregion------------------------------------------------------------------------------------------------------

        DrawPoints(ref points, accuracy); // Draws the axis on which the 

        while (currentLoadingState == loadinState.LOADING)
        {
            Transform snapPoint = closestPoint(points, animationControllPoint.transform);
            animationMagazine.position = snapPoint.position; // Move the magazine


            if (Vector3.Distance(animationMagazine.position, magazinPositionUp.position) < 0.01f) // finishLoading
            {
                FinishLoading();
            }

            else if (Vector3.Distance(animationControllPoint.transform.position, magazinPositionUp.position) >  distance + 0.01f) // Cancel Loading if Controller moves away
            {
                CancelLoading();
                CurrentLoadingState = loadinState.NOTLOADING;
            }

            yield return null;
        }
       
        yield break;
    }



    IEnumerator notLoading()
    {
        while(currentLoadingState == loadinState.NOTLOADING)
        {
            yield return null;
        }

        yield break;
    }


    void DrawPoints(ref Transform[] array, int pointCount)
    {
        Vector3 direction = magazinPositionDown.position - magazinPositionUp.position;
        float distance = Vector3.Distance(magazinPositionUp.position, magazinPositionDown.position);

        for(int i = 0; i < accuracy; i++)
        {
            GameObject temp = new GameObject("TrackPoint");
            temp.transform.position = magazinPositionUp.position + (direction.normalized * distance / accuracy * i);
            temp.transform.SetParent(transform);
            //temp.hideFlags = HideFlags.HideInHierarchy;
            array[i] = temp.transform;
        }

        
    }

    void CancelLoading()
    {
        foreach(Transform trans in points)
        {
            Destroy(trans.gameObject);
        }
        controller = null;
        Destroy(animationControllPoint);
        animationControllPoint = null;
        magazin.gameObject.SetActive(true);
        animationMagazine.gameObject.SetActive(false);
        magazin = null;
    }

    void FinishLoading()
    {
        foreach (Transform trans in points)
        {
            Destroy(trans.gameObject);
        }
        controller = null;
        Destroy(animationControllPoint);
        animationControllPoint = null;
        animationMagazine.position = magazinPositionUp.position;
        colt.Reload(magazin.gameObject); // Give magazin To Colt -- still inactive
        magazin = null;
        CurrentLoadingState = loadinState.NOTLOADING;
    }

    Transform closestPoint(Transform[] array, Transform point)
    {
        float distance = Vector3.Distance(array[0].position, point.position);
        Transform closestPoint = array[0];
        foreach (Transform trans in array)
        {
            float tempDis = Vector3.Distance(trans.position, point.position);
            if (tempDis < distance)
            {
                distance = tempDis;
                closestPoint = trans;
            }
        }
        return closestPoint;
    }



    private loadinState CurrentLoadingState
    {
        get
        {
            return currentLoadingState;
        }

        set
        {
            currentLoadingState = value;
            StopAllCoroutines();
            switch (value)
            {
                case (loadinState.LOADING):
                    StartCoroutine(loading());
                    break;
                case (loadinState.NOTLOADING):
                    StartCoroutine(notLoading());
                    break;
            }
        }
    }
}
