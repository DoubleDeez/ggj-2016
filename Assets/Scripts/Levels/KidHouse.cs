using UnityEngine;
using System.Collections;

public class KidHouse : MonoBehaviour
{

    public GameObject OpenDoor;
    public float OpenDoorTime = 15.0f;
    public float distance = 5.0f;
    public GameObject Box;
    public GameObject Medal;
    public GameObject Bottlecaps;
    public GameObject MedicalBills;

    private SpriteRenderer OpenDoorRenderer;
    private float StartTime = 0.0f;
    public bool IsPlayingScript;
    private GameObject SpawnedBox;

    public AudioClip boxShuffleSound;
    private AudioSource audioSource;
    private bool soundPlayed = false;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (OpenDoor == null)
        {
            Debug.Log("Error loading " + this.name);
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Has the object been told to do something.
        if (IsPlayingScript)
        {
            if (!audioSource.isPlaying && !soundPlayed)
            {
                audioSource.clip = boxShuffleSound;
                audioSource.Play();
                soundPlayed = true;
            }

            if (StartTime < 0.001f)
            {
                StartTime = Time.time;
                //Open the Door
                OpenDoor.SetActive(true);
            }


            //Close the door when ready
            if (StartTime + OpenDoorTime < Time.time)
            {
                OpenDoor.SetActive(false);
            }

            //Throw a Box
            if (!Box.activeInHierarchy)
            {
                Box.SetActive(true);
            }

            if (distance > 0 && Box != null)
            {
                Box.transform.Translate(Time.deltaTime * distance, 0, 0);
                distance -= Time.deltaTime * distance;
            }

            //Throw stuff out of the box
            if (distance < 0.1f)
            {
                if (Medal != null)
                {
                    Medal.SetActive(true);
                }
                //End it here
                IsPlayingScript = false;
            }
            if (distance < 1.5f && Bottlecaps != null)
            {
                Bottlecaps.SetActive(true);
            }

            if (distance < 4.7f && MedicalBills != null)
            {
                MedicalBills.SetActive(true);
            }
        }
    }

    public void DoScriptedAction()
    {
        IsPlayingScript = true;

    }
}
