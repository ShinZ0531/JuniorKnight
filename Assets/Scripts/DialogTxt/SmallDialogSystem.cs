using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SmallDialogSystem : MonoBehaviour
{
    [Header("UI Refereces")]
    public TMP_Text textLabel;


    [Header("Dialog Settings")]
    public float textSpeed = 0.05f;
    public float autoPlayWaitingTime = 2f;
    bool textFinished;

    [Header("Dialog Data")]
    public TextAsset textFile;
    private int index;
    private bool autoPlay = true;
    private bool isPlaying = false;
    // public delegate void ValueChanged(int newValue);
    // public static event ValueChanged OnValueChanged;

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
        isPlaying = true;
        textFinished = false;
        StartCoroutine(SetTextUI());
    }   

    // Update is called once per frame
    void Update()
    {
        
            // If the text is fully displayed, and the player press F, then the dialog will be closed
            if (index == textList.Count)
            {
                isPlaying = false;
                index = 0;
                gameObject.SetActive(false);
                return;    
            }

            // Auto play the text
            if (index != textList.Count && textFinished)
                StartCoroutine(SetTextUI());
                
        
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
        // Start to play the text
        textFinished = false;
        textLabel.text = "";

        // for (int i = 0; i < textList[index].Length; i++)
        // {
        //     textLabel.text += textList[index][i];
            
        //     yield return new WaitForSeconds(textSpeed);
        // }

        int letter = 0;
        while(letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;

            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];

        if (autoPlay)
            yield return new WaitForSeconds(autoPlayWaitingTime);
        // Finish playing the text
        textFinished = true;
        index++;
    }

    public bool checkIsPlaying()
    {
        return isPlaying;
    }
}
