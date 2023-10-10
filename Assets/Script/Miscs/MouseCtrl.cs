using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseCtrl : MonoBehaviour
{
    public event Action MouseOnDragEvent;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            MouseOnDragEvent?.Invoke();
    }
}
