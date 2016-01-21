using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed;
    public GameObject spawnParticles;

    private float maxSpeed = 5f;
    private Vector3 spawn;

    private Vector3 input;

	void Start () {
        spawn = transform.position;
	}
	
	void FixedUpdate ()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if(GetComponent<Rigidbody>().velocity.magnitude < maxSpeed) {
            GetComponent<Rigidbody>().AddRelativeForce(input * moveSpeed);
        }

        if(transform.position.y < -2)
        {
            Die();
        }
             
	}

    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Enemy")
        {
            Die();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Goal")
        {
            GameManager.CompleteLevel();
        }
        if (other.transform.tag == "Enemy")
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(spawnParticles, transform.position, Quaternion.Euler(270,0,0));
        transform.position = spawn;
    }
}
