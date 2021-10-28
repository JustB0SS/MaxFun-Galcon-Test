using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    // ������������ ����� ������
    public enum TypePlanet
    {
        Neutral, Player, Enemy
    }

    // ��������� ������������ ������������� ������� ���������� ��� ��������� ������
    public struct RectPlanet
    {
        public float x, y, x1, y1;
        public override string ToString() 
        { 
            return x.ToString() + ";" + y.ToString() + ";" + x1.ToString() + ";" +  y1.ToString() ;
        }
    }

    private RectPlanet _rect;
    private GameStart _game;
    private GameObject _selectCicle;

    public Ship shipPrefab;
    public TypePlanet PlanetType { get; private set; }
    public float Radius { get; private set; }
    public Vector2 Coord { get; private set; }
    private int _curientShipOnPlanet;

    private Text shipsOnPlanetText;
    private SpriteRenderer spriteRenderer;
    private int _shipProduction;

    public RectPlanet Rect() => _rect;

    public void ActivateSelectCirlce()
    {
        _selectCicle.SetActive(true);
    }

    public void DiactivateSelectCirlce()
    {
        _selectCicle.SetActive(false);
    }

    private void Awake()
    {
        _selectCicle = transform.GetChild(0).gameObject;
        _selectCicle.SetActive(false);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        shipsOnPlanetText = GetComponentInChildren<Text>();
    }

    /// <summary>
    /// ������������ ������ ������� ��� ��������� ������� ���� ������ ���� ���� � ����������� �� ������������� ����
    /// </summary>
    /// <param name="typePlanet">��� ��������� �������</param>
    public void SetStartPlanet(TypePlanet typePlanet)
    {
        PlanetType = typePlanet;
        _curientShipOnPlanet = 50;
        spriteRenderer.color = (Color)GameStart.hashtableColors[PlanetType];
        _shipProduction = 5;
        shipsOnPlanetText.text = _curientShipOnPlanet.ToString();
    }

    /// <summary>
    /// ������������� �������
    /// </summary>
    /// <param name="rect">������������� ����������� �������</param>
    /// <param name="radius">������ �������</param>
    /// <param name="centerX">������������ ������ ������� � ������������ �� ���������� X</param>
    /// <param name="centerY">������������ ������ ������� � ������������ �� ���������� Y</param>
    /// <param name="game">��������� ������ GameStart</param>
    public void Init(RectPlanet rect, float radius, float centerX, float centerY, GameStart game)
    {
        _game = game;
        Radius = radius;
        Coord = new Vector2(centerX, centerY);
        transform.position = Coord;
        transform.localScale *= (Radius*2);
        _rect = rect;
        PlanetType = TypePlanet.Neutral;
        spriteRenderer.color = (Color)GameStart.hashtableColors[PlanetType];
        _curientShipOnPlanet = UnityEngine.Random.Range(10, 25);
        shipsOnPlanetText.text = _curientShipOnPlanet.ToString();
        _shipProduction = 0;
        StartCoroutine(ShipProductionOnPlanet());
    }

    /// <summary>
    /// ������� ������� ������� � ������ ������ ������� ��������� ��� ���������� ����,
    /// ��� ��� ������� �������� � ������ ����� �� ������� ���� 0 ����������(��������)
    /// </summary>
    /// <param name="typeInvaderPlanet">��� ����������</param>
    public void Capture(TypePlanet typeInvaderPlanet)
    {
        if (PlanetType != typeInvaderPlanet)
        {
            if (_curientShipOnPlanet == 0)
            {
                PlanetType = typeInvaderPlanet;
                spriteRenderer.color = (Color)GameStart.hashtableColors[PlanetType];
                if(PlanetType == TypePlanet.Enemy)
                    _game.addPlanetToEnemyRemoveFromPlayer(this);
                else
                    _game.addPlanetToPlayerRemoveFromEnemy(this);
                _shipProduction = 5;
            }
            else
            {
                _curientShipOnPlanet--;
            }
        }
        else
        {
            _curientShipOnPlanet++;
        }
        shipsOnPlanetText.text = _curientShipOnPlanet.ToString();
    }

    /// <summary>
    /// ��������� �������� � ������� �������� �� ��������� �������� �� ������� 
    /// ��� ��������� �� ��������� ������� 
    /// </summary>
    /// <param name="capturePlanet">������� �� ������� ����������� �����</param>
    /// <returns></returns>
    public IEnumerator SpawningFleet(Planet capturePlanet)
    {
        int shipsToAttack = _curientShipOnPlanet / 2;
        _curientShipOnPlanet /= 2;
        Vector2 dir = (capturePlanet.transform.position - transform.position).normalized * Radius * 1.5f;
        Vector2 perpndicularDir = Vector2.Perpendicular((capturePlanet.transform.position - transform.position).normalized);
        int j = 1;
        int maxJ = 5;
        while (shipsToAttack > 0)
        {
            int tmp = Mathf.Min(j, shipsToAttack);
            for (int i = 0; i < tmp; i++)
            {
                Vector2 offset = perpndicularDir * (-0.05f * (tmp - 1) + 0.1f * i); 
                Ship ship = Instantiate(shipPrefab, transform.parent);
                ship.Init(capturePlanet, (Vector2)transform.position + dir + offset, PlanetType);
            }
            j = Mathf.Min(j+1,maxJ);
            shipsToAttack -= tmp;
            yield return new WaitForSeconds(0.4f);
        }
    }
    /// <summary>
    /// ��������� �������� ������ �������
    /// </summary>
    /// <returns></returns>
    IEnumerator ShipProductionOnPlanet()
    {
        while (true)
        {
            shipsOnPlanetText.text = _curientShipOnPlanet.ToString();
            yield return new WaitForSeconds(1f);
            _curientShipOnPlanet += _shipProduction;
        }
    }

}
