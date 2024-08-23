using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponCtrl : MonoBehaviour
{
    [Header("Components")]
    protected Animator _animator;
    public PlayerManager playerManager;
    public PlayerAssetsInputs _input;    
    public WeaponItemInfo weaponData;
    
    public int WeaponIndex;
    public DefineDatas.WeaponType WeaponType;
    
    protected bool _hasAnimator;


    public virtual void Init()
    {
        _hasAnimator = TryGetComponent(out _animator);

        weaponData = InventoryManager._inst.Dict_Weapon[WeaponIndex];
    }

    public abstract void Fire();
    
}
