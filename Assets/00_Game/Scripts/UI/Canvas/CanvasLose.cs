using UnityEngine;
using UnityEngine.UI;
using CONSTANT;
public class CanvasLose : UICanvas
{
   [SerializeField] private Button closeButton;
   protected override void Awake()
   {
      base.Awake();
      closeButton.onClick.AddListener(OnClickCloseButton);
   }

   private void OnClickCloseButton()
   {
      UIManager.Instance.CloseAll();
      UIManager.Instance.OpenUI<CanvasLoadingInGame>();
      LoadSceneManager.Instance.LoadSceneByName(SceneName.BufferScene, () =>
      {
         UIManager.Instance.CloseUI<CanvasLoadingInGame>(0f);
         UIManager.Instance.OpenUI<CanvasHome>();
      });
   }
}
