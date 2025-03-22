using System.Collections;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEngine;
using System.Data;
using System.Text;

// Only basic read
public class ExcelTest1 : MonoBehaviour
{

    [Header("输入文件名（带后缀）")]
    public string excelFileName = "Dialogue.xlsx"; // Inspector 中输入文件名

    // Start is called before the first frame update
    void Start()
    {
        
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
        
    }

    public void Read()
    {
        // The path of the target dialog file
        string middlePath = "/Resources/Dialogs/";
        string filePath = Application.dataPath+ middlePath + excelFileName;

        Debug.Log(filePath);
        // string filePath = Application.dataPath + "/../道具表.xlsx";
        FileInfo fileInfo = new FileInfo(filePath);
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet sheet = null;
            //excel的表数据
            for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
            {
                sheet = package.Workbook.Worksheets[i];
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

}
