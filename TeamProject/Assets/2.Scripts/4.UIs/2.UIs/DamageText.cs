using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    TMP_Text m_Text;
    public string Damage = string.Empty;
    public float FloatSpeed = 8.0f;

    void Init()
    {
        m_Text = GetComponent<TMP_Text>();
        Damage = string.Empty;
    }

    public void Configure(float damage,float lifeTime)
    {
        if (m_Text == null)
            Init();

        Damage = ((int)damage).ToString();

        StartCoroutine(Floatting());

        if (lifeTime > 0)
        {
            Invoke("Despawn", lifeTime);
        }
    }

    IEnumerator Floatting()
    {
        while (true)
        {
            m_Text.text = Damage;
            //transform.rotation = Camera.main.transform.rotation;
            transform.Translate(new Vector3(0, FloatSpeed * Time.deltaTime, 0));
            yield return null;
        }
    }

    public void Despawn()
    {
        gameObject.DestroyAPS();
    }
}
