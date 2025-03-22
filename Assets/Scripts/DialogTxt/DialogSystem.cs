using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI Refereces")]
    public TMP_Text textLabel;
    public Image faceImage;
    public TMP_Text textName;
    public GameObject SkipButton;
    public GameObject AutoButton;

    [Header("Dialog Settings")]
    public float textSpeed = 0.05f;
    public SimpleFlashControl flashControl;
    public float autoPlayWaitingTime = 2f;
    bool textFinished;
    bool cancelTyping;

    [Header("Character Image")]
    public Sprite face01;
    public Sprite face02;
    public Sprite face03;
    public Sprite face04;

    [Header("Dialog Data")]
    public TextAsset textFile;
    private int index;
    private bool autoPlay = false;

    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        GetTextFromFile(textFile);
    }

    // Show the first sentence of text
    private void OnEnable()
    {
        index = 0;

        // Set all the related UI elements to true
        SkipButton.SetActive(true);
        AutoButton.SetActive(true);
        
        // player controller off
        FindObjectOfType<PlayerController2D>().SetCanMove(false);

        textFinished = false;
        StartCoroutine(SetTextUI());
    }   

    void OnDisable()
    {
        FindObjectOfType<PlayerController2D>().SetCanMove(true);
        autoPlay = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (autoPlay)
        {
            // If the text is fully displayed, and the player press F, then the dialog will be closed
            if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0)) && index == textList.Count)
            {
                index = 0;
                flashControl.HideFlash();
                gameObject.SetActive(false);
                return;    
            }

            // Auto play the text
            if (index != textList.Count && textFinished)
                StartCoroutine(SetTextUI());
                
                    
            if (index == textList.Count)
            {
                flashControl.ShowFlash();
            }
        }
        else
        {
            // If the text is fully displayed, and the player press F, then the dialog will be closed
            if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0)) && index == textList.Count)
            {
                flashControl.HideFlash();
                index = 0;
                gameObject.SetActive(false);
                return;    
            }
            if (index == textList.Count)
            {
                flashControl.ShowFlash();
            }

            // Else, play the text normally
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (textFinished && !cancelTyping)
                    StartCoroutine(SetTextUI());
                else if (!textFinished)
                    cancelTyping = !cancelTyping;
            }
        }
        
    }

    // Read the text from the file
    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineData = file.text.Split('\n');

        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {

        switch(textList[index])
        {
            case "ME":
                faceImage.sprite = face01;
                textName.text = "ME";
                index++;
                break;
            case "DETECTIVE":
                faceImage.sprite = face02;
                textName.text = "DETECTIVE";
                index++;
                break;
            case "C":
                faceImage.sprite = face03;
                textName.text = "C";
                index++;
                break;
            case "D":
                faceImage.sprite = face04;
                textName.text = "D";
                index++;
                break;
        }


        // Start to play the text
        textFinished = false;
        textLabel.text = "";

        // for (int i = 0; i < textList[index].Length; i++)
        // {
        //     textLabel.text += textList[index][i];
            
        //     yield return new WaitForSeconds(textSpeed);
        // }

        int letter = 0;
        while(!cancelTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;

            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        
        cancelTyping = false;
        // If auto play is on, then wait for a while before playing the next sentence
        if (autoPlay && !cancelTyping)
            yield return new WaitForSeconds(autoPlayWaitingTime);
        
        // Finish playing the text
        textFinished = true;
        index++;
    }

    public void EndDialogue()
    {
        // Set all the UI elements to false
        gameObject.SetActive(false);
        // dialogEndFlesh.SetActive(false);
        SkipButton.SetActive(false);
        AutoButton.SetActive(false);
        index = 0;
        return;
    }

    public void AutoPlay()
    {
        autoPlay = !autoPlay;
    }
}
