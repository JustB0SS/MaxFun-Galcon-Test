using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Character
{
    private Difficult _difficult;
    /// <summary>
    /// ���������� ��������� ������� �� ������� ���������� � ���������
    /// </summary>
    /// <param name="fromPlanet">������� �� ������� ������ ��������� �������</param>
    /// <returns></returns>
    private Planet MinDistance(Planet fromPlanet)
    {
        Planet ans = null;
        float dist = Mathf.Infinity;
        foreach (var planet in _planets)
        {
            if (planet.PlanetType == Planet.TypePlanet.Enemy) continue;
            float newDist = Vector2.Distance(planet.Coord, fromPlanet.Coord);
            if (newDist < dist)
            {
                ans = planet;
                dist = newDist;
            }
        }
        return ans;
    }
    /// <summary>
    /// ������������� ����
    /// </summary>
    /// <param name="startPlanet">��������� �������</param>
    /// <param name="planets">������ ���� ������</param>
    /// <param name="typePlanet">��� ����������� ������</param>
    /// <param name="difficult">���������� ������������ �������� ����</param>
    public void Init(Planet startPlanet, List<Planet> planets, Planet.TypePlanet typePlanet,Difficult difficult)
    {
        base.Init(startPlanet, planets,typePlanet);
        _difficult = difficult;
        StartCoroutine(FindNextTarget());

    }
    /// <summary>
    /// ����� ������ ����������� ���������� ������� �������� ����� ��������� �� ����������� �������
    /// � �������� ��������� �
    /// </summary>
    /// <returns></returns>
    public IEnumerator FindNextTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(_difficult.timeBetweenAction);
            foreach (var planet in _controledPlanets)
            {
                var targetPlanet = MinDistance(planet);
                if(targetPlanet != null)
                    StartCoroutine(planet.SpawningFleet(targetPlanet));
            }
        }
    }

}
