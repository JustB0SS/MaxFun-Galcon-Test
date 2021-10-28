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
    /// Отслеживание столкновение, в случае когда объект с которым столкнулся корабль оказался планетой на которую они летели
    /// то он пытается захватить планету, а также уничтажается
    /// </summary>
    /// <param name="collision">Объект пересечения</param>
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
    /// Инициализация коробля
    /// </summary>
    /// <param name="target">Планету на которую летит корабль</param>
    /// <param name="pos">позиция коробля</param>
    /// <param name="typePlanet">тип родной планеты</param>
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
    /// Передвижение
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
