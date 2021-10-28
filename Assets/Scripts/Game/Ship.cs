using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    private Planet _targetPlanet;
    private ShipMovement _movement;
    private Planet.TypePlanet _typeHomePlanet;
    private SpriteRenderer spriteRenderer;

    
    public Transform GetTransformTargetPlanet => _targetPlanet.transform;

    /// <summary>
    /// ������������ ������������, � ������ ����� ������ � ������� ���������� ������� �������� �������� �� ������� ��� ������
    /// �� �� �������� ��������� �������, � ����� ������������
    /// </summary>
    /// <param name="collision">������ �����������</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == _targetPlanet.transform)
        {
            _targetPlanet.Capture(_typeHomePlanet);
            StopCoroutine(Movement());
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// ������������� �������
    /// </summary>
    /// <param name="target">������� �� ������� ����� �������</param>
    /// <param name="pos">������� �������</param>
    /// <param name="typePlanet">��� ������ �������</param>
    public void Init(Planet target,Vector2 pos,Planet.TypePlanet typePlanet)
    {
        _targetPlanet = target;
        _typeHomePlanet = typePlanet;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.color = (Color)GameStart.hashtableColors[typePlanet];
        transform.position = pos;
        _movement = GetComponent<ShipMovement>();
        Vector2 dir = _targetPlanet.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        StartCoroutine(Movement());
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <returns></returns>
    IEnumerator Movement()
    {
        while (true) { 
            yield return new WaitForFixedUpdate();
            Vector2 dir = _targetPlanet.transform.position - transform.position;
            _movement.Move(dir);
        }
    }

}
