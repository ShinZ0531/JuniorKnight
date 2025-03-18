using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    public GameObject Button;
    public GameObject talkUI;
    // private bool canTalkAgain = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Button.SetActive(false);
    }

    private void Update()
    {
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            talkUI.SetActive(true);
            Button.SetActive(false);

        }
    }
}
