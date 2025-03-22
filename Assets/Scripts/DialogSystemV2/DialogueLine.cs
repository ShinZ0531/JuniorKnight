using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine {
    public int ID;                      // 对话ID
    public string CharacterName;        // 角色名
    public string Emotion;              // 表情
    public string DialogueText;         // 对话内容
    public List<DialogueOption> Options;// 选项列表（最多3个）
    public int NextID;                  // 无选项时默认跳转ID
}
