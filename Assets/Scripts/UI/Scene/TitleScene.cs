using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        // TitleScene �ʱ�ȭ �ڵ�
        Debug.Log("TitleScene Initialized");
    }

    public override void Clear()
    {
        // TitleScene ����/���� �۾�
        Debug.Log("TitleScene Cleared");
    }
}
