using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBot : MonoBehaviour
{
    public Rigidbody2D rb2D;
    private float speed;
    private Vector3 offset = new Vector3(0, 1.2f, 0);
    public List<GameObject> mice = new List<GameObject>();
    public GameObject closestMice;
    public bool miceFound = false;
    public Transform ball;
    public GameObject bullet;
    public bool ready = true;
    public GameObject obs;


    // Start is called before the first frame update
    void Start()
    {
        speed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!avoidObstacle())
        {
            rotation();
            thrust();
        }

        
    }

    public bool avoidObstacle()
    {
        return false;
    }
    public IEnumerator CMice()
    {
        miceFound = false;
        yield return new WaitForSeconds(3f);
        miceFound = true;
    }
    public void FindMice()
    {
        GameObject closest = null;
        float distance = 1000.0f;
        Vector3 position = transform.position;
        foreach (GameObject g in mice)
        {
            Vector3 diff = g.transform.position - position;
            if (diff.sqrMagnitude < distance)
            {
                closest = g;
                distance = diff.sqrMagnitude;
            }
            
        }

        closestMice = closest;
    }


    public void rotation()
    {
        if (!miceFound || closestMice == null)
        {
            FindMice();
        }
        StartCoroutine(CMice());
        Vector3 dir = closestMice.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.x - 1.5f, dir.y) * Mathf.Rad2Deg;
        angle = 90 - angle;
        if (angle > 180)
        {
            angle -= 360;
        }

        angle -= transform.rotation.z * 180;
        if (angle > 0)
        {
            transform.Rotate(0f, 0f, 1f);
        }

        if (angle < 0)
        {
            transform.Rotate(0f, 0f, -1f);
        }

        if (angle > -2 && angle < 2)
        {
            shoot();
        }
    }
    
    void shoot()
    {
        if (!ready)
        {
            return;
        }
        StartCoroutine(CD());
    }
         
    public IEnumerator CD()
    {
        ready = false;
        GameObject clone = (GameObject) Instantiate(bullet, ball.position, ball.rotation);
        yield return new WaitForSeconds(Random.Range(0.25f, 2.5f));
        ready = true;
        Destroy(clone, 2f);
    }

    public void thrust() {
       rb2D.AddForce(transform.right * speed * Time.deltaTime, ForceMode2D.Impulse);
    }
}
