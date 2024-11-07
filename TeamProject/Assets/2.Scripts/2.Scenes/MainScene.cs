using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MainScene : BaseScene
{
    protected override void Init()
    {
        CurrScene = eScene.MainScene;
        SoundManager._inst.PlayBGM("MainScene");

    }


    public void ClickGameStart()
    {
        SceneLoad(eScene.GameScene);
    }
}
