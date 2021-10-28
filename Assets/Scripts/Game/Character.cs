using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Абстрактный класс для Игрока и Бота
public abstract class Character : MonoBehaviour
{
    //Список всех планет
    protected List<Planet> _planets = new List<Planet>();
    //Список подконтрольных планет
    protected List<Planet> _controledPlanets = new List<Planet>();
    //Тип персонажа
    protected Planet.TypePlanet _typePlanet;

    //Делегат для сообщения о победе одного из оппонентов 
    public delegate void EndGameHandler(Planet.TypePlanet type);
    public EndGameHandler endGame;
    /// <summary>
    /// Инициализация персонажа
    /// </summary>
    /// <param name="startPlanet">Стартовая планета</param>
    /// <param name="planets">Список всех планет</param>
    /// <param name="typePlanet">Тип подвластных планет</param>
    public virtual void Init(Planet startPlanet, List<Planet> planets, Planet.TypePlanet typePlanet)
    {
        _typePlanet = typePlanet;
        _planets = planets;
        _controledPlanets.Add(startPlanet);
        startPlanet.SetStartPlanet(_typePlanet);
    }
    /// <summary>
    /// проверка все ли планеты подвластны данному персонажу
    /// </summary>
    /// <returns></returns>
    public virtual bool Check()
    {
        
        foreach (var planet in _planets)
        {
            if (!_controledPlanets.Contains(planet)) return false;
        }
        return true;
    }

    /// <summary>
    /// Функция добавления планеты в список подконтрольных планет
    /// </summary>
    /// <param name="newPlanet"></param>
    public virtual void AddPlanet(Planet newPlanet)
    {
        _controledPlanets.Add(newPlanet);
        if (Check()&&endGame!=null) endGame(_typePlanet);
    }
    /// <summary>
    /// Функция удаления планеты из список подконтрольных планет
    /// </summary>
    /// <param name="newPlanet"></param>
    public virtual void RemovePlanet(Planet newPlanet)
    {
        _controledPlanets.Remove(newPlanet);
    }
}
