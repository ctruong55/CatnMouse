using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject botPrefab;
    public GameObject cheesePrefab;
    public GameObject catPrefab;
    public float numBots;
    public float numCheese;
    public float maxCheese;

    // Start is called before the first frame update
    void Start()
    {
        numCheese = 0f;
        spawnBots();
    }

    // Update is called once per frame
    void Update()
    {
        spawnCheese();
    }


    void spawnBots() {

        for (int i = 0; i < numBots; i++)
        {
            //Instantiate(botPrefab, new Vector2(Random.Range(-26f, 26f), Random.Range(-17f, 17f)), Quaternion.identity);
            catPrefab.GetComponent<CatBot>().mice.Add(Instantiate(botPrefab, new Vector2(Random.Range(-26f, 26f), Random.Range(-17f, 17f)), Quaternion.identity));
        }

    }

    void spawnCheese() {

        if ((numCheese < maxCheese) && (numCheese >= 0)) {
            Instantiate(cheesePrefab, new Vector2(Random.Range(-26f, 26f), Random.Range(-17f, 17f)), Quaternion.identity);
            numCheese++;
        }
    
    }
}
