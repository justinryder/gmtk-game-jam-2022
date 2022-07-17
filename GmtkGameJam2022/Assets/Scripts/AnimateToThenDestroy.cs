using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimateTo))]
public class AnimateToThenDestroy : MonoBehaviour
{
    public void AnimateToPosition(Vector3 targetPosition)
    {
        GetComponent<AnimateTo>().AnimateToPosition(targetPosition, () => Destroy(gameObject));
    }
}
