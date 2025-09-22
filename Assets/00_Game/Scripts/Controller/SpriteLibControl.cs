using System.Collections.Generic;
using UnityEngine;

public class SpriteLibControl : SingletonMono<SpriteLibControl>
{
    [SerializeField]
    private List<Sprite> icons;
    private Dictionary<string, Sprite> dic = new Dictionary<string, Sprite>();

    protected override void Awake()
    {
        base.Awake();
        foreach(Sprite s in icons)
        {
            dic.Add(s.name, s);
        }
    }

    public Sprite GetImageByName(string name_srpite)
    {
        return dic[name_srpite];
    }
}