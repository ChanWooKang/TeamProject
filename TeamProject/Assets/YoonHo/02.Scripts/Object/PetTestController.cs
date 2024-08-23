using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetTestController : MonoBehaviour
{
    



    public IEnumerator MoveToObject(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.5)
        {
            Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
            yield return null;
        }
        if (Vector3.Distance(transform.position, targetPos) <= 0.5)
            StopCoroutine(MoveToObject(targetPos));
    }
}
