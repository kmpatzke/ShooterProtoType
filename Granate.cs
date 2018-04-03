using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granate : MonoBehaviour {

    public AudioClip explSound;
    [SerializeField] float delay = 5f;
    [SerializeField] int damage = 100;
    [SerializeField] float explosionDelay;
    [SerializeField] PickableObject scriptPickable;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float explosionForce = 700f;
    [SerializeField] AudioSource audioS;
    [SerializeField] bool countDownStarted = false;

    [SerializeField] GameObject explosionFX;
    float countdown;

    bool isExploded = false;
	// Use this for initialization
	void Start () {
        countdown = delay;
	}
	
	// Update is called once per frame
	void Update () {
        if (scriptPickable.isHeld)
        {
            countDownStarted = true;
        }

        if (countDownStarted)
        {
            countdown -= Time.deltaTime;

            if (countdown <= 0f && !isExploded)
            {
                isExploded = true;
                Explode();

            }
        }
        

    }

    void Explode()
    {
        audioS.PlayOneShot(explSound);
        var boom = Instantiate(explosionFX, transform.position, transform.rotation);
        
        Collider[]colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            HealthManager manager = col.GetComponent<HealthManager>();
            if(manager != null)
            {
                manager.GetHit(damage);
            }
        }
        Destroy(boom, 2.5f);
        Destroy(gameObject, 1f);
    }
}
