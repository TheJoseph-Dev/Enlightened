using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCutsceneTrigger : MonoBehaviour
{

    public RootCutscene cutscene;
    public Transform playerTranform;
    public PlayerState playerState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTranform.position.x >= this.transform.position.x + 2)
        {
            cutscene.Trigger();
            print("Triggered");
        }
    }


    private void OnCollisionStay(Collision collision)
    {

        if (collision.collider.CompareTag("Player")) { playerState.swapable = false; }
    }
}
