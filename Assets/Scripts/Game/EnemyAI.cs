using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Character
{
    private Difficult _difficult;
    /// <summary>
    /// Нахождения ближайшей планеты от планеты переданной в аргументе
    /// </summary>
    /// <param name="fromPlanet">Планета от которой ищется ближайшая планета</param>
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
    /// Инициализация Бота
    /// </summary>
    /// <param name="startPlanet">Стартовая планета</param>
    /// <param name="planets">Список всех планет</param>
    /// <param name="typePlanet">Тип подвластных планет</param>
    /// <param name="difficult">Переменная определяющая сложость бота</param>
    public void Init(Planet startPlanet, List<Planet> planets, Planet.TypePlanet typePlanet,Difficult difficult)
    {
        base.Init(startPlanet, planets,typePlanet);
        _difficult = difficult;
        StartCoroutine(FindNextTarget());

    }
    /// <summary>
    /// через каждый определнный промежуток времени пытается найти ближайшую не захваченную планету
    /// и пытается захватить её
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
