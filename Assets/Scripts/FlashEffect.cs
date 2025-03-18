using UnityEngine;

public class SimpleFlashControl : MonoBehaviour
{
    // 直接挂载在DialogEndFlash的Canvas上
    public GameObject dialogEndFlash; // 拖入自己的Canvas
    
    void Start()
    {
        dialogEndFlash.SetActive(false); // 初始隐藏
    }

    // 对话结束时调用这个方法
    public void ShowFlash()
    {
        dialogEndFlash.SetActive(true);
    }

    // 玩家继续后调用这个方法
    public void HideFlash()
    {
        dialogEndFlash.SetActive(false);
    }
}