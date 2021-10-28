using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private PlanetSelect planetSelect;

    public List<Planet> GetPlanets => _controledPlanets;

    /// <summary>
    /// ������������� ������
    /// </summary>
    /// <param name="startPlanet">��������� �������</param>
    /// <param name="planets">������ ���� ������</param>
    /// <param name="typePlanet">��� ����������� ������</param>
    /// <param name="selectBox">UI ����� ��� ����������� ���������� �������</param>
    public void Init(Planet startPlanet, List<Planet> planets, Planet.TypePlanet typePlanet,RectTransform selectBox)
    {
        base.Init(startPlanet, planets, typePlanet);
        planetSelect = GetComponent<PlanetSelect>();
        planetSelect.Init(this,_planets, selectBox);
    }

}
