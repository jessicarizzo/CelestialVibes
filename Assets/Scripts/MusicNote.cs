using System;
using Constants;
using Enums;
using UnityEngine;

public class MusicNote : MonoBehaviour
{
    public bool canBePressed = false;
    public KeyCode keyToPress;
    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress) && canBePressed)
        {
            // Hide the note when pressed
            gameObject.SetActive(false);
            
            // Determine hit accuracy
            var noteHoverLocation = Math.Abs(transform.position.y);
            
            if (noteHoverLocation > HitAccuracy.Normal)
            {
                GameManager.s_gameInstance.NoteHit(HitType.Normal);
                Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
            }
            else if (noteHoverLocation > HitAccuracy.Good)
            {
                GameManager.s_gameInstance.NoteHit(HitType.Good);
                Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
            }
            else
            {
                GameManager.s_gameInstance.NoteHit(HitType.Perfect);
                Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
            }
            // GameManager.s_gameInstance.NoteHit(HitType.Good);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Used activeSelf check to fix every note firing a miss when it exited its collider
        if (other.CompareTag("Activator") && gameObject.activeSelf)
        {
            canBePressed = false;
            GameManager.s_gameInstance.NoteMissed();
            Instantiate(missEffect, transform.position + new Vector3(0, 0.5f, 0), missEffect.transform.rotation);
        }
    }
}
