using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct Boundary
{
    public float min_x;
    public float max_x;
    public float min_z;
    public float max_z;

    public float rot_min;
    public float rot_max;

    public float scroll__min_y;
    public float scroll__max_y;
}

public class CameraControl : MonoBehaviour
{
    public Boundary bnd;
    Vector3 dire;
    float x, y, z;
    public float move_speed;
    float rotate_ang_x;
    float rotate_ang_y;
    bool is_pause_active;
    public bool Is_Pause_Active { get { return is_pause_active; } set { is_pause_active = value; } }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        x = 0;
        y = 0;
        z = 0;
        is_pause_active = true;
        rotate_ang_x = transform.localEulerAngles.x;
        rotate_ang_y = transform.localEulerAngles.y;
    }

    private void Update()
    {
        if (!GameControl.Instance.choose_panel.gameObject.activeSelf)
        {
            var m_x = Input.GetAxis("Mouse X");
            var m_y = Input.GetAxis("Mouse Y");
            var scroll_wheel = Input.GetAxis("Mouse ScrollWheel");
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            float tmp_y = transform.position.y;
            //滚轮放大缩小(注意是Vector3.forward而不是transform.forward,Translate默认Space.Self)
            if (is_pause_active)
            {
                transform.Translate(Vector3.forward * scroll_wheel * move_speed * Time.deltaTime * 100);
                y = Mathf.Clamp(transform.position.y, bnd.scroll__min_y, bnd.scroll__max_y);
            }
            //左键拖动屏幕
            if (Input.GetMouseButton(0))
            {
                transform.Translate(new Vector3(-m_x, 0, -m_y) * move_speed * Time.deltaTime);
                y = tmp_y;
            }
            //右键左右上下旋转
            if (Input.GetMouseButton(1))
            {
                rotate_ang_x -= m_y * move_speed * Time.deltaTime;
                rotate_ang_y -= m_x * move_speed * Time.deltaTime;

                rotate_ang_x = Mathf.Clamp(rotate_ang_x, bnd.rot_min, bnd.rot_max);
                transform.localEulerAngles = new Vector3(rotate_ang_x,
                    rotate_ang_y, transform.localEulerAngles.z);
            }
            //键盘控制移动
            if (Input.anyKey)
            {
                transform.Translate(new Vector3(hor, 0, ver) * move_speed * Time.deltaTime);
                y = tmp_y;
            }
            //键盘控制旋转
            if (Input.GetKey(KeyCode.Q))
            {
                transform.RotateAround(transform.position, Vector3.up, -move_speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.RotateAround(transform.position, Vector3.up, move_speed * Time.deltaTime);
            }

            x = Mathf.Clamp(transform.position.x, bnd.min_x, bnd.max_x);
            z = Mathf.Clamp(transform.position.z, bnd.min_z, bnd.max_z);
            transform.position = new Vector3(x, y, z);
        }
    }
}
