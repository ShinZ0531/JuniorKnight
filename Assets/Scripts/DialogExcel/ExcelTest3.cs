using System.Collections;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEngine;
using System.Data;
using System.Text;
using System;
using TMPro;
using UnityEngine.UI;

public class ExcelTest3: MonoBehaviour
{

    [Header("Whole file name with extension: FileName.xlsx")]
    public string excelFileName = "Dialogue.xlsx";

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
    private int[,] dialogAccessList;
    private FileInfo fileInfo;
    
    bool textFinished;
    bool cancelTyping;
    private bool autoPlay = false;

    void Awake()
    {
        // Set the path of the target dialog file
        // Initialize the dialog access list
        ReadInit();
        // Debug.Log("==Start==: ReadInit() is called.");
    }

    void OnEnable()
    {
        // Debug.Log("==OnEnable==: Start.");

        // Get the parent data

        // Set all the related UI elements to true
        // SkipButton.SetActive(true);
        // AutoButton.SetActive(true);

        // player controller off
        // FindObjectOfType<PlayerController2D>().SetCanMove(false);

        // Initialize the dialog data
        textFinished = false;
        
        // StartCoroutine(SetDialogUI(RandomChooseDialog()));
        int targetDialog = RandomChooseDialog();
        Debug.Log(targetDialog);
        // Debug.Log("==OnEnable==: RandomChooseDialog() is called.");

        // Read(targetDialog);
        // Debug.Log("==OnEnable==: Read() is called.");
    }
    void OnDisable()
    {
        FindObjectOfType<PlayerController2D>().SetCanMove(true);
        SkipButton.SetActive(false);
        AutoButton.SetActive(false);
        autoPlay = false;
    }

    public void ReadInit()
    {
        // The path of the target dialog file
        string middlePath = "/Resources/Dialogs/";
        string filePath = Application.dataPath+ middlePath + excelFileName;
        // Debug.Log("[ReadInit]: " + filePath);

        fileInfo = new FileInfo(filePath);
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            // Initialize the dialog access list
            // row 0: is this dialog accessible? 0: no, 1: yes
            dialogAccessList = new int[package.Workbook.Worksheets.Count, 1];
            Debug.Log("[ReadInit]: Number of sheets: " + package.Workbook.Worksheets.Count);
            Debug.Log("[ReadInit]: Number of rows: " + dialogAccessList.GetLength(0));
            Debug.Log("[ReadInit]: Number of columns: " + dialogAccessList.GetLength(1));
            SetRowToValue(dialogAccessList, 0, 1);
            // Debug.Log("[ReadInit]: SetRowToValue() is called.");
        }
    }



    // Set the whole row to a specific value
    void SetRowToValue(int[,] array, int rowIndex, int value)
    {
        Debug.Log("[SetRowToValue]: rowIndex is " + rowIndex);
        Debug.Log("[SetRowToValue]: array.GetLength(0) is " + array.GetLength(0));
        if (rowIndex < 0 || rowIndex >= array.GetLength(0))
        {
            Debug.LogError("[SetRowValue]: rowIndex is out of range!");
            return;
        }

        for (int col = 0; col < array.GetLength(1); col++)
        {
            array[rowIndex, col] = value; // 动态赋值
        }
    }

    public int RandomChooseDialog()
    {
        // Randomly choose a dialog from the dialog access list
        int dialogIndex = UnityEngine.Random.Range(0, dialogAccessList.GetLength(0));
        do
        {    Debug.Log("[RandomChooseDialog]: Dialog index: " + dialogIndex);
            if (dialogAccessList[dialogIndex, 0] == 1)
            {
                Debug.Log("[RandomChooseDialog]: Dialog is accessible.");
            }
            else
            {
                Debug.Log("[RandomChooseDialog]: Dialog is not accessible. Try again.");
            }}
        while (dialogAccessList[dialogIndex, 0] != 1);
        return dialogIndex;
    }

    public void Read(int dialogIndex)
    {
        ExcelWorksheet sheet = null;
        
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            sheet = package.Workbook.Worksheets[dialogIndex + 1];
            // Debug.Log("[Read]: " + sheet.Cells[1, 1].Value.ToString());
            //行
            int rowCount = sheet.Dimension.End.Row;
            //列
            int columnCount = sheet.Dimension.End.Column;

            //行数据：从第2行开始读取，因为第0行，我们定义了字段
            for (var row = 2; row <= rowCount; row++)
            {
                //列数据：从第1行开始读取
                for (var col = 1; col <= columnCount; col++)
                {
                    //如果数据不为空，则打印
                    if (sheet.Cells[row, col].Value != null)
                    {
                        Debug.Log(sheet.Cells[row, col].Value.ToString());
                    }

                }
            }
        }
    }
}
