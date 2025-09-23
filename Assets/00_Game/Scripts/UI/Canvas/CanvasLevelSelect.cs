using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class CanvasLevelSelect : UICanvas
{
    [SerializeField] private List<Button> levelSelectButton;
    protected override void Awake()
    {
        base.Awake();
        for (int i= 0; i < levelSelectButton.Count; i++)
        {
            int index = i;
            levelSelectButton[i].onClick.AddListener(()=>OnClickLevelSelectButton(index));
        }
    }

    private void OnClickLevelSelectButton(int index)
    {
        LevelData.SelectedLevelId = index;
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasLoadingInGame>();
        LoadSceneManager.Instance.LoadSceneByName($"ThinhScene", () =>
        {
            UIManager.Instance.CloseUI<CanvasLoadingInGame>(0);
            UIManager.Instance.OpenUI<CanvasGamePlay>();

        });
    }
}

public static class LevelData
{
    public static int SelectedLevelId;
}
