using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiceAI : MonoBehaviour
{
    public GameObject Manager;
    public GameObject Center;
    public GameObject Cat;
    public GameObject Cheese;
    public Rigidbody2D rb2D;
    public ParticleSystem dust;
    private float speed;
    private float size;
    private float HP;
    private float rotateRate1;
    private float rotateRate2;
    private bool runAway;
    private bool runTo;
    private bool boundrybreach;


    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.Find("Spawner");
        Cat = GameObject.Find("Cat");
        Cheese = GameObject.Find("Cheese(Clone)");
        Center = GameObject.Find("Center");

        speed = Random.Range(5f, 15f);
        size = 10f / speed;
        transform.localScale = new Vector2(size, size);
        HP =  size * 3f;

        rotateRate1 = Random.Range(0.25f, 0.50f);
        rotateRate2 = Random.Range(2.5f, 5.0f);

        runAway = false;
        runTo = false;
        boundrybreach = false;
       
        transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        InvokeRepeating("RotateSmall", 1f, rotateRate1);
        InvokeRepeating("RotateBig", 1f, rotateRate2);
    }

    // Update is called once per frame
    void Update()
    {
        thrust();
        Flee();
        Feed();
        Breach();

        if (HP <= 0f)
        {
            Cat.GetComponent<CatBot>().mice.Remove(gameObject);
            Destroy(gameObject);
        }

    }
    void RotateSmall()
    {
        if (!runAway || !runTo)
        {
            transform.Rotate(0f, 0f, Random.Range(-22.5f, 22.5f));
        }
    }

    void RotateBig()
    {
        if (!runAway || !boundrybreach || !runAway)
        {
            transform.Rotate(0f, 0f, Random.Range(-45, 45));  
        }
    }

    public void thrust()
    {
        rb2D.AddForce(transform.up * speed * Time.deltaTime, ForceMode2D.Impulse);
        dust.Play();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Cat")
        {
            HP -= 2f;
        }

        if (col.gameObject.tag == "Bullet")
        {
            HP -= 1f;
            speed /= 2f;
            StartCoroutine(slowDown(3f));
        }

        if (col.gameObject.tag == "Cheese")
        {
            Destroy(col.gameObject);
            Manager.GetComponent<Spawner>().numCheese--;
            speed *= 2f;
            StartCoroutine(speedDown(3f));
        }

        if (col.gameObject.tag == "Border")
        {
            boundrybreach = true;
            transform.Rotate(0f, 0f, 180);
            StartCoroutine(Normalize(Random.Range(0.5f, 2f)));
        }

        else if (col.gameObject.tag == "Bot" || col.gameObject.tag == "Mouse")
        {
            transform.Rotate(0f, 0f, 90);
        }
    }

    public void Flee()
    {
        if (Vector2.Distance(Cat.transform.position, transform.position) <= (Random.Range(5f, 10f)) && !runTo)
        {
            runAway = true;
            transform.Rotate(0f, 0f, Random.Range(0f, 1f));
            transform.position = Vector2.MoveTowards(transform.position, Cat.transform.position, Time.deltaTime * ((Random.Range(-1f, -0.25f)) * speed));
        }

        else
        {
            runAway = false;
        }
    }

    public void Feed() {

        Cheese = GameObject.FindGameObjectWithTag("Cheese");
        if (Vector2.Distance(Cheese.transform.position, transform.position) <= (Random.Range(5f, 10f)) && !runAway)
        {
            runTo = true;
            transform.position = Vector2.MoveTowards(transform.position, Cheese.transform.position, Time.deltaTime * ((Random.Range(0.25f, 1f)) * speed));
        }

        else
        {
            runTo = false;
        }

    }

    public void Breach()
    {
        if (boundrybreach)
        {
            transform.position = Vector2.MoveTowards(transform.position, Center.transform.position, Time.deltaTime * ((Random.Range(0.05f, 0.50f)) * speed));
        }
    }

    IEnumerator Normalize(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        boundrybreach = false;
    }


    IEnumerator slowDown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        speed *= 2f;
    }

    IEnumerator speedDown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        speed /= 2f;
    }


}
