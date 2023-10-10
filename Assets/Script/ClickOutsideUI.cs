using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOutsideUI : MonoBehaviour, IPointerClickHandler
{
    List<RaycastResult> hitList;
    [SerializeField] GameObject[] visibleObjectsList;

    // Start is called before the first frame update
    void Start()
    {
        hitList = new List<RaycastResult>();
    }

    private GameObject GetCollisionObjects(PointerEventData pointerEventData)
    {
        EventSystem.current.RaycastAll(pointerEventData, hitList);

        if (hitList.Count > 0)
            return hitList[0].gameObject;

        return null;
    }

    private Transform GetParentTransform(PointerEventData pointerEventData)
    {
        GameObject currentGO = GetCollisionObjects(pointerEventData);

        if (currentGO != null)
        {
            Transform currentTransform = currentGO.transform;

            while (currentTransform.parent != null)
            {
                if (isInVisibleObjList(currentTransform))
                    return currentTransform;

                currentTransform = currentTransform.parent;
            }
        }

        return null;
    }

    private bool isInVisibleObjList(Transform TargetTransform)
    {
        foreach (GameObject go in visibleObjectsList)
        {
            if (go.transform == TargetTransform)
                return true;
        }
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GetParentTransform(eventData))
        {
            gameObject.SetActive(false);
        }
    }
}