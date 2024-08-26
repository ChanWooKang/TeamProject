using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowCtrl : WeaponCtrl
{
    //Test
    //È­»ì ÇÁ¸®ÆÕ »ðÀÔ
    public Transform Player;
    public Transform ArrowParent;
    public GameObject arrowPrefab;

    ArrowCtrl nowArrow;
    
    private void Update()
    {
        Aim();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Fire()
    {
        if(nowArrow != null)
        {
            nowArrow.FireArrow(Player);
        }
    }

    void Aim()
    {
        if (_hasAnimator)
        {
            _animator.SetBool(Animator.StringToHash("Aim"), _input.aim);
        }
    }

    public void AimStart()
    {
        if (_hasAnimator)
        {
            _animator.SetTrigger(Animator.StringToHash("AimStart"));
        }
    }

    public void DrawArrow()
    {
        //¼£¶ù¤©¶ó
        GameObject go = Instantiate(arrowPrefab, ArrowParent);
        nowArrow = go.GetComponent<ArrowCtrl>();
        nowArrow.Init();
    }
}
