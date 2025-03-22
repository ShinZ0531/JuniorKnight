using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTalkButton : MonoBehaviour
{
    public GameObject Button;
    public GameObject EventUI;
    public SmallDialogSystem target;
    // public GameObject targetObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!target.checkIsPlaying())
        // if (target.activeSelf)
            Button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!target.checkIsPlaying())
            Button.SetActive(false);
    }

    private void Update()
    {
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            EventUI.SetActive(true);
            Button.SetActive(false);
        }
    }

}
