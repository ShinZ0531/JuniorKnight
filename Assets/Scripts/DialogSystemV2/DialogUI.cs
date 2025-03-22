using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueUI : MonoBehaviour {
    [Header("UI组件绑定")]
    public TMP_Text nameText;     // 角色名文本（TMP）
    public TMP_Text dialogueText; // 对话内容文本（TMP）
    public Image characterImage;         // 角色立绘
    public GameObject optionButtonPrefab;// 选项按钮预制体（需挂载TMP_Text）
    public Transform optionsPanel;       // 选项父物体（建议使用VerticalLayoutGroup）

    [Header("对话管理")]
    public DialogueManager dialogueManager;
    private int currentDialogueID;

    void Start() {
        StartCoroutine(RunDialogue(1)); // 从ID=1开始
    }

    // 主对话协程
    IEnumerator RunDialogue(int startID) {
        currentDialogueID = startID;

        while (currentDialogueID != 0) {
            DialogueLine line = dialogueManager.GetDialogue(currentDialogueID);
            if (line == null) {
                Debug.LogError($"找不到对话ID：{currentDialogueID}");
                yield break;
            }

            // 更新UI
            UpdateDialogueUI(line);

            // 处理选项
            if (line.Options.Count > 0) {
                yield return StartCoroutine(ShowOptions(line.Options));
            } else {
                yield return new WaitForSeconds(2); // 无选项等待2秒
                currentDialogueID = line.NextID;
            }
        }

        Debug.Log("对话结束");
    }

    // 更新角色信息和文本
    void UpdateDialogueUI(DialogueLine line) {
        nameText.text = line.CharacterName;
        dialogueText.text = line.DialogueText;
        characterImage.sprite = LoadCharacterSprite(line.CharacterName, line.Emotion);
    }

    // 显示选项并等待选择
    IEnumerator ShowOptions(List<DialogueOption> options) {
        ClearOptions();

        // 动态生成按钮
        foreach (var option in options) {
            GameObject buttonObj = Instantiate(optionButtonPrefab, optionsPanel);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = option.Text;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => OnOptionSelected(option.NextID));
        }

        // 等待玩家选择
        int previousID = currentDialogueID;
        yield return new WaitUntil(() => currentDialogueID != previousID);
    }

    // 选项点击处理
    void OnOptionSelected(int nextID) {
        currentDialogueID = nextID;
        ClearOptions();
    }

    // 清空选项按钮
    void ClearOptions() {
        foreach (Transform child in optionsPanel) {
            Destroy(child.gameObject);
        }
    }

    // 加载角色表情图片
    Sprite LoadCharacterSprite(string characterName, string emotion) {
        string path = $"Characters/{characterName}/{emotion}";
        Sprite sprite = Resources.Load<Sprite>(path);
        if (sprite == null) Debug.LogError($"找不到图片：{path}");
        return sprite;
    }
}