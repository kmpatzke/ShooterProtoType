using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour {

    [SerializeField] Transform bulletSpawn;
    [SerializeField] float hitAccuracy;
    [SerializeField] float dieDelay;
    public GameObject previousTarget; // Damit der CoverEnemieManager dieses Cover wieder als occupied = false setzen kann
    public GameObject target = null;

    public Transform player;

    [SerializeField] float rotationSpeed = 8;

    NavMeshAgent navMeshAgent;

    private enum Enemy_State { SEARCH_FOR_COVER, SHOOT };
    private Enemy_State currentState;
    private GameObject currentCover;


    #region MainFunctions ------------------------------------------------------------MAIN FUNCTIONS
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        CurrentState = Enemy_State.SEARCH_FOR_COVER;

    }

    void Update()
    {
        if (target)
        {
            if (transform.position.x == target.transform.position.x && transform.position.z == target.transform.position.z)
            {
                lookAtPlayer();
            }
        }


        if (target == null && CurrentState != Enemy_State.SHOOT)
        {
            CurrentState = Enemy_State.SHOOT;
        }

        if (target != null && CurrentState != Enemy_State.SEARCH_FOR_COVER)
        {
            CurrentState = Enemy_State.SEARCH_FOR_COVER;
        }
    }

    #endregion----------------------------------------------------------------------------------------



    #region States ----------------------------------------------------------------------STATES
    private IEnumerator SearchForCover()
    {


        while (CurrentState == Enemy_State.SEARCH_FOR_COVER)
        {


            searchForCover();


            yield return null;

        }
        yield break;
    }

    private IEnumerator Shoot()
    {
        while (CurrentState == Enemy_State.SHOOT)
        {
            //attackPlayer();

            yield return null;
        }
        yield break;
    }

    #endregion---------------------------------------------------------------------------------------------------

    #region StateProperties--------------------------------------------------------------------------STATE PROPERTIES
    private Enemy_State CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;

            StopAllCoroutines();

            switch (currentState)
            {
                case Enemy_State.SEARCH_FOR_COVER:
                    StartCoroutine(SearchForCover());
                    break;
                case Enemy_State.SHOOT:
                    StartCoroutine(Shoot());
                    break;
            }
        }
    }
    #endregion-----------------------------------------------------------------------------------------

    #region EnemyBehaviours--------------------------------------------------------ENEMY BEHAVIOURS
    void searchForCover()
    {
        if (target != null)
        {
            navMeshAgent.SetDestination(target.transform.position);
            previousTarget = target;

        }

    }

    void lookAtPlayer()
    {


        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        if (direction == Vector3.zero)
        {
            direction = Vector3.forward;
        }
        Quaternion rotateToPlayer = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateToPlayer, rotationSpeed);


    }

    public void Die()
    {
        StopAllCoroutines();
        navMeshAgent.isStopped = true;
        Destroy(gameObject, dieDelay);
    }

    //void attackPlayer()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(bulletSpawn.position, player.position, out hit, Mathf.Infinity))
    //    {
    //        if (hit.transform.CompareTag("Player"))
    //        {
    //            schießen
    //            treffen mit abweichender Wahrscheinlichkeit


    //        }
    //    }
    //}

    #endregion-------------------------------------------------------------------------------------
}
