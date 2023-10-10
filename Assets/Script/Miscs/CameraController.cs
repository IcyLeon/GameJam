using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera CameraUI;
    [SerializeField] SpriteRenderer ground;
    private const float MoveSpeed = 50f;
    private const float MoveSmoothLerp = 3f;
    private Camera MainCamera;
    private Vector3 CameraPosition;
    private Vector3 Target;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        CameraPosition = Vector3.zero;
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

        MouseMove();
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

        float left = ground.sprite.bounds.max.x * 0.5f + w;
        float up = ground.sprite.bounds.max.y * 0.5f + h;

        Debug.Log(ground.sprite.bounds);

        if (tr.x > ground.transform.position.x + left)
        {
            CameraPosition += -rightVec * Mathf.Abs(tr.x - (ground.transform.position.x + left));
        }
        if (tl.x < ground.transform.position.x - left)
        {
            CameraPosition += rightVec * Mathf.Abs((ground.transform.position.x - left) - tl.x);
        }
        if (tr.y > ground.transform.position.y + up)
        {
            CameraPosition += -UpVec * Mathf.Abs(tr.y - (ground.transform.position.y + up));
        }
        if (br.y < ground.transform.position.y - up)
        {
            CameraPosition += UpVec * Mathf.Abs((ground.transform.position.y - up) - br.y);
        }
    }
}
