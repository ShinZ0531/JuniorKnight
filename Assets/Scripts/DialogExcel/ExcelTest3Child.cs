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


public class ExcelTest3Child : MonoBehaviour
{

    private ExcelTest2Parent parentData;
    private bool textFinished;
    private bool cancelTyping;
    private bool autoPlay = false;
    private int[,] dialogAccessList;
    private FileInfo fileInfo;


    private void Awake()
    {
        parentData = GetComponentInParent<ExcelTest2Parent>();
        if (parentData == null)
        {
            Debug.LogError("未找到父节点的 ExcelTest2Parent 组件！请确保父节点正确挂载。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        Debug.Log("==OnEnable==: Start.");

        // Get the parent data
        parentData = GetComponentInParent<ExcelTest2Parent>();
        if (parentData == null)
        {
            Debug.LogError("父节点数据未初始化！");
            return;
        }

        // Set all the related UI elements to true
        // parentData.SkipButton.SetActive(true);
        // parentData.AutoButton.SetActive(true);

        // player controller off
        // FindObjectOfType<PlayerController2D>().SetCanMove(false);

        // Initialize the dialog data
        textFinished = false;
        dialogAccessList = parentData.GetDialogAccessList();
        fileInfo = parentData.GetFileInfo();
        
        // StartCoroutine(SetDialogUI(RandomChooseDialog()));
        int targetDialog = RandomChooseDialog();
        Debug.Log("==OnEnable==: RandomChooseDialog() is called.");

        Read(targetDialog);
        Debug.Log("==OnEnable==: Read() is called.");
    }
        void OnDisable()
    {
        FindObjectOfType<PlayerController2D>().SetCanMove(true);
        autoPlay = false;
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

    // IEnumerator SetDialogUI(int dialogIndex)
    // {
    //     // Start to play the text
    //     // textFinished = false;
    //     // textLabel.text = "";

    //     // for (int i = 0; i < textList[index].Length; i++)
    //     // {
    //     //     textLabel.text += textList[index][i];
            
    //     //     yield return new WaitForSeconds(textSpeed);
    //     // }

    //     // int letter = 0;
    //     // while(!cancelTyping && letter < textList[index].Length - 1)
    //     // {
    //     //     textLabel.text += textList[index][letter];
    //     //     letter++;

    //     //     yield return new WaitForSeconds(textSpeed);
    //     // }
        
    //     // dialog index + 1 because the index starts from 0 in array
    //     ExcelWorksheet sheet = package.Workbook.Worksheets[dialogIndex + 1];
    //     int rowCount = sheet.Dimension.End.Row;
    //     int columnCount = sheet.Dimension.End.Column;

    //     //行数据：从第2行开始读取，因为第1行，我们定义了字段
    //     for (var row = 2; row <= rowCount; row++)
    //     {
    //         //列数据：从第1行开始读取
    //         for (var col = 1; col <= columnCount; col++)
    //         {
    //             //如果数据不为空，则打印
    //             if (sheet.Cells[row, col].Value != null)
    //             {
    //                 Debug.Log(sheet.Cells[row, col].Value.ToString());
    //             }
    //         }
    //     }
    // }
}
