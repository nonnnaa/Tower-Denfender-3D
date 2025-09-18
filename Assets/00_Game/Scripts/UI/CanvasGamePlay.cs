using System.Collections.Generic;
using UnityEngine;
public class CanvasGamePlay : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI text;

    public override void Setup()
    [SerializeField] private List<string> unitKeys;
    [SerializeField] private List<UnitSelectControl> unitSelectControls;
    
    protected override void Awake()
    {
        base.Awake();
        for(int i = 0; i < unitKeys.Count; i++)
        {
            unitSelectControls[i].Init(unitKeys[i]);
        }
    }

    public void PauseButton()
    {
        text.text = "Continue";
        Time.timeScale = 0;
    }
    
    
    
}
