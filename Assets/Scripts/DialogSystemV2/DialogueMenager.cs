using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public class DialogueManager : MonoBehaviour {
    public TextAsset csvFile;           // CSV文件（拖入Inspector）
    public List<DialogueLine> dialogueList = new List<DialogueLine>();

    void Start() {
        ParseCSV();
    }

    void ParseCSV() {
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) { // 从第2行开始（跳过标题）
            string lineData = lines[i].Trim();
            if (string.IsNullOrEmpty(lineData)) continue;

            string[] data = SplitCsvLine(lineData);
            // 调试：输出当前行的字段数和内容
            Debug.Log($"第 {i + 1} 行字段数: {data.Length}\n内容: {string.Join("|", data)}");

            // 检查列数是否合法（标题行应有11列）
            if (data.Length < 10) { // ID(0) + 基础字段(3) + 3选项*2列(6) + NextID(9)
                Debug.LogError($"第{i+1}行数据不完整，应有至少10列，实际{data.Length}列。内容：{lineData}");
                continue;
            }

            try {
                // 解析 NextID（位于第 10 列，索引为 9）
                string nextIDRaw = data[9].Trim(); // 显式 Trim 去空格
                if (!int.TryParse(nextIDRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int nextID)) {
                    Debug.LogError($"第 {i + 1} 行 NextID 无效: {nextIDRaw}");
                    continue;
                }

                // // 解析基础字段
                // int id = ParseIntSafe(data[0], $"第{i+1}行ID");
                // int nextID = ParseIntSafe(data[9], $"第{i+1}行NextID");

                DialogueLine line = new DialogueLine {
                    // ID = id,
                    // CharacterName = data[1],
                    // Emotion = data[2],
                    // DialogueText = data[3],
                    // NextID = nextID,
                    // Options = new List<DialogueOption>()

                    ID = int.Parse(data[0].Trim()),
                    CharacterName = data[1].Trim(),
                    Emotion = data[2].Trim(),
                    DialogueText = data[3].Trim(),
                    NextID = nextID,
                    Options = new List<DialogueOption>()
                };

                // 解析3组选项（Option1~Option3，每组占2列）
                for (int optionIndex = 0; optionIndex < 3; optionIndex++) {
                    int col = 4 + optionIndex * 2; // Option1在4-5列，Option2在6-7列，Option3在8-9列

                    if (col + 1 >= data.Length) break; // 防止越界

                    string optionText = data[col].Trim();
                    string condition = data[col + 1].Trim();

                    if (string.IsNullOrEmpty(optionText)) continue;

                    // 分割选项文本和跳转ID（兼容中英文冒号）
                    string[] parts = optionText.Split(new[] { ':', '：' }, System.StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2) {
                        Debug.LogError($"第{i+1}行选项格式错误：{optionText}");
                        continue;
                    }

                    string text = parts[0].Trim();
                    int jumpID = ParseIntSafe(parts[1], $"第{i+1}行选项跳转ID");

                    line.Options.Add(new DialogueOption {
                        Text = text,
                        NextID = jumpID,
                        Condition = condition
                    });
                }

                dialogueList.Add(line);
            } catch (System.Exception e) {
                Debug.LogError($"解析第{i+1}行失败：{e.Message}\n原始数据：{lineData}");
            }
        }
    }

    // 安全解析整数（带错误提示）
    private int ParseIntSafe(string input, string errorContext) {
        if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result)) {
            return result;
        }
        throw new System.ArgumentException($"{errorContext} 不是有效数字：{input}");
    }

    // 处理带引号的CSV分割
    private string[] SplitCsvLine(string line) {
        List<string> result = new List<string>();
        StringBuilder current = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line) {
            if (c == '"') {
                inQuotes = !inQuotes;
            } else if (c == ',' && !inQuotes) {
                result.Add(current.ToString().Trim());
                current.Clear();
            } else {
                current.Append(c);
            }
        }

        result.Add(current.ToString().Trim());
        return result.ToArray();
    }

    // 根据ID获取对话行
    public DialogueLine GetDialogue(int id) {
        return dialogueList.Find(x => x.ID == id);
    }
}