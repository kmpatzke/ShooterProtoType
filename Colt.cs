using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colt : MonoBehaviour {


   [SerializeField] List<AudioClip> clips;
   [SerializeField] GameObject muzzleFlash;
   [SerializeField] Transform muzzleSpawn;
   [SerializeField] int damage;


   private GameObject magazin;
   PistolMagazine scriptMagazine;
   [SerializeField] Transform magazinReleasePoint;
   [SerializeField] float releaseSpeed = 5.0f;
   [SerializeField] Transform animationMagazin;

   [SerializeField] private bool isLoaded;

    private void Update()
    {
        if (magazin)
        {
            isLoaded = true;
        }
        else
        {
            isLoaded = false;
        }
    }


    public void Reload(GameObject magazin)
    {
        this.magazin = magazin;
        scriptMagazine = magazin.GetComponent<PistolMagazine>();
        ReloadFX();
        Debug.Log("Loaded");
    }

    public void Fire()
    {
        if (magazin)
        {
            if(scriptMagazine.Bullets > 0)
            {
                scriptMagazine.DecreaseBullets();
                ShootFX();
                HitTarget();
 
            }
            else
            {
                GunEmpyFX();
                ReleaseMagazine();
            }
        }

        else
        {
            GunEmpyFX();
        }
    }

    public void ReleaseMagazine()
    {
        ReleaseMagazinFX();
        StartCoroutine(Release());
    }

    void ReloadFX()
    {
        var audio = GetComponent<AudioSource>();
        audio.PlayOneShot(clips[1]);
    }

    void ReleaseMagazinFX()
    {
        var audio = GetComponent<AudioSource>();
        audio.PlayOneShot(clips[2]);
    }

    void GunEmpyFX()
    {
        var audio = GetComponent<AudioSource>();
        audio.PlayOneShot(clips[3]);
    }

    void ShootFX()
    {

        var muzzle = Instantiate(muzzleFlash, muzzleSpawn.transform.position, muzzleSpawn.transform.rotation);
        var audio = GetComponent<AudioSource>();
        audio.PlayOneShot(clips[0]);
        Destroy(muzzle, 0.5f);
    }

    void HitTarget()
    {
        RaycastHit hit;

        if (Physics.Raycast(muzzleSpawn.position, muzzleSpawn.forward, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.transform.gameObject);
            if (hit.transform.GetComponent<HealthManager>())
            {
                var manager = hit.transform.GetComponent<HealthManager>();
                manager.GetHit(this.damage);
            }
        }
    }

    public bool IsLoaded
    {
        get
        {
            return isLoaded;
        }

        set
        {
            isLoaded = value;
        }
    }


    IEnumerator Release()
    {
        while (Vector3.Distance(animationMagazin.position, magazinReleasePoint.position) > 0.02f)
        {
            animationMagazin.position = Vector3.Lerp(animationMagazin.position, magazinReleasePoint.position, releaseSpeed);
            yield return null;
        }

        animationMagazin.gameObject.SetActive(false);
        magazin.transform.position = magazinReleasePoint.position;
        magazin.transform.rotation = magazinReleasePoint.rotation;
        magazin.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); // Otherwhise it has the velocity from the last time it was active
        magazin.SetActive(true);
        magazin = null;

        yield break;
        
    }



}
