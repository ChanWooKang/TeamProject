using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    //테스트 버전 애니메이션 대신 색상 변경으로 처리    
    [SerializeField] Renderer render;
    [SerializeField] Collider capsule;
    // 공격 애니메이션 공격 지점 도달 예상 시간으로 설정
    [SerializeField] float AttackTime = 0.5f;

    public float weaponDamage;
    Coroutine weaponEvent;

    void Awake()
    {
        weaponEvent = null;
        SetEnable(false);
    }

    void SetEnable(bool isAttack)
    {
        capsule.enabled = isAttack;
    }

    void ChangeColor(bool isAttack)
    {
        Color color = isAttack ? Color.black : Color.cyan;
        render.material.color = color;
    }

    public void OnWeaponUse(float damage)
    {
        weaponDamage = damage;
        if(weaponEvent != null)
            StopCoroutine(weaponEvent);
        weaponEvent = StartCoroutine(WeaponCoroutine());
    }

    //몬스터 에게 닿았을때 인식되어 Damage값을 0으로 처리해 나머지 딜이 안들어가게 설정
    public void OnAttack()
    {
        if (weaponDamage != 0)
            weaponDamage = 0;
    }
    
    IEnumerator WeaponCoroutine()
    {
        yield return new WaitForSeconds(AttackTime);
        SetEnable(true);
        ChangeColor(true);
        yield return new WaitForSeconds(0.25f);
        SetEnable(false);
        ChangeColor(false);
    }

}
