using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera CameraUI;
    [SerializeField] MouseCtrl mouseCtrl;
    private const float MoveSpeed = 50f;
    private const float MoveSmoothLerp = 3f;
    private Camera MainCamera;
    private Vector3 CameraPosition;
    private Vector3 Target;
    Vector3 PreviousTilemapTRPos;
    Vector3 PreviousTilemapBLPos;


    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        CameraPosition = Target = Vector3.zero;
        Target.z = -10f;
        mouseCtrl.MouseOnDragEvent += MouseMove;
    }

    void MouseMove()
    {
        Vector2 MouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Target = CameraPosition - (((transform.up * MouseMovement.y) + (transform.right * MouseMovement.x)) * MoveSpeed);
        Target.z = -10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != transform.position)
            CameraPosition = Vector3.Lerp(CameraPosition, Target, MoveSmoothLerp * Time.deltaTime);

        UpdateBounds();
        transform.position = CameraPosition;
    }

    void UpdateBounds()
    {
        float h = MainCamera.orthographicSize;
        float w = h * MainCamera.aspect;

        Vector3 UpVec = Vector3.up;
        Vector3 rightVec = Vector3.right;

        Vector3 tl = CameraPosition - (rightVec * w) + (UpVec * h);
        Vector3 tr = CameraPosition + (rightVec * w) + (UpVec * h);
        Vector3 bl = CameraPosition - (rightVec * w) - (UpVec * h);
        Vector3 br = CameraPosition + (rightVec * w) - (UpVec * h);

        Vector3Int trTilemap = MapManager.GetInstance().GetMainTileMap().WorldToCell(tr);
        Vector3Int blTilemap = MapManager.GetInstance().GetMainTileMap().WorldToCell(bl);

        if (!MapManager.GetInstance().GetMainTileMap().HasTile(trTilemap))
        {
            if (tr.y > PreviousTilemapTRPos.y)
            {
                CameraPosition += -UpVec * Mathf.Abs(tr.y - (PreviousTilemapTRPos.y));
            }
            if (tr.x > PreviousTilemapTRPos.x)
            {
                CameraPosition += -rightVec * Mathf.Abs(tr.x - (PreviousTilemapTRPos.x));
            }
        }
        else
        {
            PreviousTilemapTRPos = tr;
        }

        if (!MapManager.GetInstance().GetMainTileMap().HasTile(blTilemap))
        {
            if (bl.x < PreviousTilemapBLPos.x)
            {
                CameraPosition += rightVec * Mathf.Abs(bl.x - (PreviousTilemapBLPos.x));
            }
            if (bl.y < PreviousTilemapBLPos.y)
            {
                CameraPosition += UpVec * Mathf.Abs(bl.y - (PreviousTilemapBLPos.y));
            }
        }
        else
        {
            PreviousTilemapBLPos = bl;
        }
    }
}
