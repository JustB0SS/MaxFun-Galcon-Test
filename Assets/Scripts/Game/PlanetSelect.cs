using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSelect : MonoBehaviour
{
    
    private Player _player;

    [SerializeField]
    private List<Planet> _planets = new List<Planet>();
    [SerializeField]
    private List<Planet> _selectedPlanets = new List<Planet>();
    [SerializeField]
    private Vector2 _startSelection;
    [SerializeField]
    private Vector2 _endSelection;

    private Planet.RectPlanet _rectSelection;
    private float _size;
    private float _width;
    private float _height;
    private float _minHeighWidth;
    private RectTransform _selectBox;

    /// <summary>
    /// �������� ������������ �� ��� �������������� ������������� ������� � ������������� ���������� ����������
    /// </summary>
    /// <param name="rect">������������� �������</param>
    /// <returns></returns>
    private bool Check(Planet.RectPlanet rect)
    {
        return !(_rectSelection.y < rect.y1 || _rectSelection.y1 > rect.y || _rectSelection.x1 < rect.x || _rectSelection.x > rect.x1);
    }
    /// <summary>
    /// ������������� ������
    /// </summary>
    /// <param name="player">��������� ������</param>
    /// <param name="planets">������ ���� ������</param>
    /// <param name="selectBox">UI ����� ��� ����������� ���������� �������</param>
    public void Init(Player player,List<Planet> planets,RectTransform selectBox)
    {
        _selectBox = selectBox;
        _planets = planets;
        _player = player;
        _size = Camera.main.orthographicSize;
        _width = Screen.width;
        _height = Screen.height;
        _minHeighWidth = Mathf.Min(_width, _height);
        _startSelection = Vector2.zero;
        _endSelection = Vector2.zero;
        DrawBox();

    }

    /// <summary>
    /// ������� ������������� ���� ��� ��������� ������� �� �����
    /// � ���� ������ ����������� ���� ���� ������ ���� �� ������� ������
    /// �� ��� ����������, ���� �� ����������� ������� ��� �� ������� ����� �� �� ���� ���������� ������ ����� ������� ��������
    /// ����� ��������� ������ ��������� 
    /// </summary>
    private void Click()
    {
        Planet clickedPlanet = null;
        foreach (var planet in _planets)
        {
            if (Check(planet.Rect()))
            {
                clickedPlanet = planet;
                break;
            }
        }
        if (clickedPlanet == null)
        {
            _selectedPlanets.RemoveAll(x => { x.DiactivateSelectCirlce(); return true; });
        }
        else if (clickedPlanet.PlanetType == Planet.TypePlanet.Player)
        {
            _selectedPlanets.RemoveAll(x => { x.DiactivateSelectCirlce(); return true; });
            _selectedPlanets.Add(clickedPlanet);
        }
        else{
            if (_selectedPlanets.Count > 0)
            {
                foreach (var planet in _selectedPlanets) StartCoroutine(planet.SpawningFleet(clickedPlanet));
                _selectedPlanets.RemoveAll(x => { x.DiactivateSelectCirlce(); return true; });
            }
        }
    }
    /// <summary>
    /// ������� ���������� ���� ���� ��������� ��������� �������,
    /// � ���� ������ ��� ������������� �������������� � ��������������� ��������� ��������� � ������ ���������� ������
    /// </summary>
    private void Drag()
    {
        _selectedPlanets.RemoveAll(x => { x.DiactivateSelectCirlce(); return true; });
        foreach (var planet in _player.GetPlanets)
        {
            if (Check(planet.Rect()) && planet.PlanetType == Planet.TypePlanet.Player)
            {
                _selectedPlanets.Add(planet);
                planet.ActivateSelectCirlce();
            }
        }
    }
    /// <summary>
    /// ��������� ������� ���������
    /// </summary>
    private void DrawBox()
    {
        Vector2 boxStart = _startSelection;
        Vector2 boxEnd = _endSelection;
        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        _selectBox.position = boxCenter;
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        _selectBox.sizeDelta = boxSize;
    }


    /// <summary>
    /// ������������ ������� �� ������
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startSelection = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            _endSelection = Input.mousePosition;
            DrawBox();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _endSelection = Input.mousePosition;
            _rectSelection.x = Mathf.Min(_startSelection.x, _endSelection.x) / _minHeighWidth * _size * 2;
            _rectSelection.x1 = Mathf.Max(_startSelection.x, _endSelection.x) / _minHeighWidth * _size * 2;
            _rectSelection.y = Mathf.Max(_startSelection.y, _endSelection.y) / _minHeighWidth * _size * 2;
            _rectSelection.y1 = Mathf.Min(_startSelection.y, _endSelection.y) / _minHeighWidth * _size * 2;

            if (Vector2.Distance(_startSelection,_endSelection) <= 0.01f)
            {
                Click();
                Debug.Log("Click");
            }
            else
            {
                Drag();
                Debug.Log("Drag");
            }

            _startSelection = Vector2.zero;
            _endSelection = Vector2.zero;
            DrawBox();
        }
    }
}
