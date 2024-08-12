using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    //�׽�Ʈ ���� �ִϸ��̼� ��� ���� �������� ó��    
    [SerializeField] Renderer render;
    [SerializeField] Collider capsule;
    // ���� �ִϸ��̼� ���� ���� ���� ���� �ð����� ����
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

    //���� ���� ������� �νĵǾ� Damage���� 0���� ó���� ������ ���� �ȵ��� ����
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
