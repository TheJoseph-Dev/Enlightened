using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Formats.Alembic.Importer;

public class RootCutscene : MonoBehaviour
{
    private Camera cam;

    public Transform mainCamera;
    //public Animator anim;

    public CutsceneHandler uiHandler;

    public SceneTransition transition;

    public bool trigger { get; private set; }

    public PlayerState playerState;

    public AlembicStreamPlayer rootAnim;
    public AudioSource rootSFX;

    private void Start()
    {
        this.trigger = false;
        this.cam = transform.GetComponent<Camera>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (trigger)
        {
            print("Triggered");

            transform.position = mainCamera.transform.position;
            transform.rotation = mainCamera.transform.rotation;

            mainCamera.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
            mainCamera.GetComponent<Camera>().enabled = false;

            cam.enabled = true;


            //Time.timeScale = 0.8f;
            float delay = 3.0f;
            StartCoroutine(PlayAnimation(delay + 0.1f));

            if (rootAnim.CurrentTime == 0) { rootSFX.pitch = 1.25f; rootSFX.Play(); }
            if (rootAnim.CurrentTime < 1.33f) rootAnim.CurrentTime += Time.deltaTime / 2;

            uiHandler.SetCamera(cam);
            uiHandler.SetHUDOpacity(0);

            this.trigger = false;
        }
    }

    private IEnumerator PlayAnimation(float delay)
    {
        playerState.swapable = false;

        Vector3 startPosition = new Vector3(141, 52, 13.2f);
        Quaternion startRotation = Quaternion.Euler(new Vector3(42, 90, 0));

        StartCoroutine(MoveWithDelay(transform.position, startPosition, delay / 2));
        StartCoroutine(RotateWithDelay(transform.rotation, startRotation, delay / 2));

        yield return new WaitForSeconds(4);

        //anim.enabled = true; // Takes 5 seconds

        Vector3 nextPos = startPosition + new Vector3(5.5f, -8, -2);
        Quaternion nextRot = Quaternion.Euler(new Vector3(42, 90, 0) + new Vector3(38, 0, 0));

        StartCoroutine(MoveWithDelay(transform.position, nextPos, delay));
        StartCoroutine(RotateWithDelay(transform.rotation, nextRot, delay));

        yield return new WaitForSeconds(delay + 1);
        playerState.swapable = true;

        
        transition.Play();
    }


    private IEnumerator MoveWithDelay(Vector3 posA, Vector3 posB, float delay)
    {

        float counter = 0f;
        while (counter < delay)
        {
            Vector3 move = Vector3.Lerp(posA, posB, counter / delay);
            transform.position = move;

            counter += Time.deltaTime;


            yield return null;
        }

        transform.position = posB;

        yield return null;
    }

    // Quaternions > Euler
    private IEnumerator RotateWithDelay(Quaternion rotA, Quaternion rotB, float delay)
    {

        float counter = 0f;
        while (counter < delay)
        {

            Quaternion delta = Quaternion.Lerp(rotA, rotB, counter / delay);
            transform.rotation = delta;

            counter += Time.deltaTime;


            yield return null;
        }

        transform.rotation = rotB;

        yield return null;
    }


    public void Trigger()
    {
        this.trigger = true;
    }
}
