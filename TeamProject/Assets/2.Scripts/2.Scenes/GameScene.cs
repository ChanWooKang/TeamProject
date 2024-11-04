using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        CurrScene = eScene.GameScene;
        SoundManager._inst.PlayBGM("InGameScene");

    }

}
