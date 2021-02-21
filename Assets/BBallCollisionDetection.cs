using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBallCollisionDetection : MonoBehaviour
{
    //Collider col;

    // Start is called before the first frame update
    void Start()
    {
        //float colliderRadius = GetComponent<SphereCollider>().radius;
        //colliderRadius = 0.2f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("col detected");

        switch (collision.collider.tag)
        {
            case "Teammate":
                PlayerStateMachine playerState = collision.collider.GetComponent<PlayerStateMachine>();
                playerState.playerState = PlayerStateMachine.PlayerState.BBInPossesion;

                
                this.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        switch (collision.collider.tag)
        {
            case "Teammate":
                PlayerStateMachine playerState = collision.collider.GetComponent<PlayerStateMachine>();
                playerState.playerState = PlayerStateMachine.PlayerState.NoBB;
                this.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
