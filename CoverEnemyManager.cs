using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverEnemyManager : MonoBehaviour {

    //Notiz: Der CoverEnemyManager vergibt die freien Deckungen, hinter denen sich die Feinde verstecken können an eben die Feinde. Warum? Das ist die einfachste Methode,
    // um zu verhindern, dass zwei Feinde zur gleichen Zeit eine Deckung besetzen wollen. 

    // FUnktionsweise: ..................
    [SerializeField] string EnemyTag;
    [SerializeField] string CoverTag;
    [SerializeField] int randomTime;
    [SerializeField] GameObject[] covers;
    [SerializeField] GameObject[] enemies;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

        covers = GameObject.FindGameObjectsWithTag(CoverTag);

        StartCoroutine(SetTargets());

    }

    IEnumerator SetTargets()
    {
        while (true)
        {
            enemies = GameObject.FindGameObjectsWithTag(EnemyTag); // Gegner Array muss immer aktuell sein, denn es können neue dazukommen oder andere verschwinden
            enemies = shuffleArray(enemies);

            //foreach(GameObject en in enemies)
            //{
            //    Debug.Log(en);
            //}


            foreach (GameObject enemy in enemies)
            {
                var enemy_Script = enemy.GetComponent<Enemy_AI>();
                GameObject newTarget = null;
                while (newTarget == null)
                {
                    newTarget = covers[Random.Range(0, covers.Length)];
                    if (newTarget.GetComponent<Covers>().Ocuppied == true)
                    {
                        newTarget = null;
                    }
                }
                enemy_Script.target = newTarget;
                newTarget.GetComponent<Covers>().Ocuppied = true;

                if (enemy_Script.previousTarget)
                {
                    enemy_Script.previousTarget.GetComponent<Covers>().Ocuppied = false;
                }

                yield return new WaitForSeconds(Random.Range(0, randomTime)); // Deswegen schmiert das Programm ab, wenn kein Gegner auf dem Feld ist
            }

            if(enemies.Length == 0)
            {
                yield break;
            }
        }
        

    }

    GameObject[] shuffleArray(GameObject[] array)
    {
        List<GameObject> tempList = new List<GameObject>();
        foreach (GameObject element in array)
        {
            tempList.Add(element);
        }

        GameObject[] shuffledArray = new GameObject[array.Length];
        int counter = 0;

        while (tempList.Count > 0)
        {
            int ran = Random.Range(0, tempList.Count);
            shuffledArray[counter] = tempList[ran];
            tempList.Remove(tempList[ran]);
            counter++;
        }
        return shuffledArray;
    }

}
