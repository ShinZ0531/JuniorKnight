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

// public interface DialogData
// {
//     int[,] GetDialogAccessList();
//     FileInfo GetFileInfo();
// }
public class ExcelTest2Parent : MonoBehaviour
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

    void Start()
    {
        // Set the path of the target dialog file
        // Initialize the dialog access list
        ReadInit();
        Debug.Log("==Start==: ReadInit() is called.");
    }


    public void ReadInit()
    {
        // The path of the target dialog file
        string middlePath = "/Resources/Dialogs/";
        string filePath = Application.dataPath+ middlePath + excelFileName;
        Debug.Log("[ReadInit]: " + filePath);

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
            Debug.Log("[ReadInit]: SetRowToValue() is called.");
        }
    }

    void SetRowToValue(int[,] array, int rowIndex, int value)
{
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

    public int[,] GetDialogAccessList() => dialogAccessList;
    public FileInfo GetFileInfo() => fileInfo;
}
