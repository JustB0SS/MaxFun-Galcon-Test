using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    private const float _speed = 1f;
    private const float _speedRotation = 3f;
    private Ship _ship;


    /// <summary>
    /// ѕоворот в направлени€ вектора
    /// </summary>
    /// <param name="dir">направление поворота</param>
    public void LookAt(Vector2 dir)
    {
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.back), _speedRotation * Time.deltaTime);
    }

    /// <summary>
    /// ѕередвижени€ по заданому направлению
    /// </summary>
    /// <param name="dir">Ќаправление движени€</param>
    public void Move(Vector2 dir)
    {
        Vector2 rotateDir = dir;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + dir.normalized ,dir.normalized,0.02f);
        if (hit.collider!=null)
        {
            if (hit.transform.tag == "Planet" &&hit.transform != _ship.GetTransformTargetPlanet)
            {
                Vector2 dirLeft = Quaternion.AngleAxis(-30, transform.forward) * dir;
                Vector2 dirRight = Quaternion.AngleAxis(30, transform.forward) * dir;
                RaycastHit2D rightHit = Physics2D.Raycast((Vector2)transform.position + dirRight.normalized, dirRight.normalized, 0.01f);
                RaycastHit2D leftHit = Physics2D.Raycast((Vector2)transform.position + dirLeft.normalized, dirLeft.normalized, 0.01f);
                if (leftHit.collider == null)
                    rotateDir=dirLeft;
                else if (rightHit.collider == null)
                    rotateDir=dirRight;
                else
                    rotateDir = dirRight;
            }
        }
        transform.position += transform.up * _speed * Time.deltaTime;
        LookAt(rotateDir);
    }

    private void Awake()
    {
        _ship = GetComponent<Ship>();
    }
}
