using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HandMovement : MonoBehaviour
{
    [SerializeField]
    float jitter = 1f;

    [SerializeField]
    float MaximumDistance = 5;

    [SerializeField]
    float speed = 1f;

    [SerializeField]
    int mode = 0;

    [SerializeField]
    float cooldown = 10f;


    Vector3 transformScreenSpace = new Vector2(0, 0);
    Vector3 screenPoint = new(0, 0, 0);
    Vector3 direction = new(0, 0, 0);
    Vector3 randomJitter = new(0, 0, 0);

    float interpolatedDistance = 0f;
    float trueDistance = 0f;
    float currentSpeed = 0f;


    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentSpeed = rb.velocity.magnitude;

        cooldown -= .25f;
        randomJitter.x = Random.Range(-jitter, jitter);
        randomJitter.y = Random.Range(-jitter, jitter);
        randomJitter.z = 0f;

        screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        screenPoint.z = 5f;

        transformScreenSpace = Camera.main.ScreenToWorldPoint(transform.position);
        screenPoint = Camera.main.ScreenToWorldPoint(screenPoint);


        direction = (screenPoint - transform.position).normalized;

          JazzHand();
    }


    private void JazzHand()
    {
        trueDistance = Vector3.Distance(this.transform.position, screenPoint);
        interpolatedDistance = Mathf.Lerp(0f, trueDistance, Time.deltaTime * speed);
        interpolatedDistance = ApplyMinMaxDistancing(trueDistance, interpolatedDistance);
        direction.z = 0;
        transform.Translate(direction * interpolatedDistance * 0.02f , Space.World);


        if (cooldown <= 0f)
        {
            rb.AddForce(randomJitter, ForceMode.Impulse);
            cooldown = 10f;
        }
    }




    float ApplyMinMaxDistancing(float trueDistance, float interpolatedDistance)
    {
        if (trueDistance - interpolatedDistance >= MaximumDistance)
        {
            interpolatedDistance = trueDistance - MaximumDistance;
        }
        return interpolatedDistance;
    }
}
