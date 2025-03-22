using UnityEngine;

[System.Serializable]
public class DialogueOption {
    public string Text;                 // 选项文本
    public int NextID;                  // 选择后的跳转ID
    public string Condition;            // 条件检测（预留字段）
}