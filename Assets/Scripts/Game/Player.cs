using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private PlanetSelect planetSelect;

    public List<Planet> GetPlanets => _controledPlanets;

    /// <summary>
    /// Инициализация Игрока
    /// </summary>
    /// <param name="startPlanet">Стартовая планета</param>
    /// <param name="planets">Список всех планет</param>
    /// <param name="typePlanet">Тип подвластных планет</param>
    /// <param name="selectBox">UI обект для отображения выделяемой области</param>
    public void Init(Planet startPlanet, List<Planet> planets, Planet.TypePlanet typePlanet,RectTransform selectBox)
    {
        base.Init(startPlanet, planets, typePlanet);
        planetSelect = GetComponent<PlanetSelect>();
        planetSelect.Init(this,_planets, selectBox);
    }

}
