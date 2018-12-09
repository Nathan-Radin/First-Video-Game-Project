using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public AudioClip fireSound;

    public float speed = 10.0f;
    public float padding = 0.1f;
    public float projectileSpeed;
    public float firingRate = 0.2f;
    public float health = 250f;

    public GameObject projectile;

    float xMin = -5;
    float xMax = 5;

	// Use this for initialization
	void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftMost.x + padding;
        xMax = rightMost.x - padding;
    }
	
    void Fire()
    {
        Vector3 offset = transform.position + new Vector3(0, 1, 0);
        GameObject beam = Instantiate(projectile, offset, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }

		if (Input.GetKey(KeyCode.LeftArrow)) { 
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow)){
            this.transform.position += Vector3.right * speed * Time.deltaTime;
        }

        //restricts player to game space
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missle = collider.gameObject.GetComponent<Projectile>();
        if (missle)
        {
            health -= missle.getDamage();
            missle.Hit();
            if (health <= 0)
            {
                Death();
            }

        }
    }
    void Death() {
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen");
        Destroy(gameObject);

    }
}
