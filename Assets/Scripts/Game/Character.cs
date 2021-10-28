using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����������� ����� ��� ������ � ����
public abstract class Character : MonoBehaviour
{
    //������ ���� ������
    protected List<Planet> _planets = new List<Planet>();
    //������ �������������� ������
    protected List<Planet> _controledPlanets = new List<Planet>();
    //��� ���������
    protected Planet.TypePlanet _typePlanet;

    //������� ��� ��������� � ������ ������ �� ���������� 
    public delegate void EndGameHandler(Planet.TypePlanet type);
    public EndGameHandler endGame;
    /// <summary>
    /// ������������� ���������
    /// </summary>
    /// <param name="startPlanet">��������� �������</param>
    /// <param name="planets">������ ���� ������</param>
    /// <param name="typePlanet">��� ����������� ������</param>
    public virtual void Init(Planet startPlanet, List<Planet> planets, Planet.TypePlanet typePlanet)
    {
        _typePlanet = typePlanet;
        _planets = planets;
        _controledPlanets.Add(startPlanet);
        startPlanet.SetStartPlanet(_typePlanet);
    }
    /// <summary>
    /// �������� ��� �� ������� ���������� ������� ���������
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
    /// ������� ���������� ������� � ������ �������������� ������
    /// </summary>
    /// <param name="newPlanet"></param>
    public virtual void AddPlanet(Planet newPlanet)
    {
        _controledPlanets.Add(newPlanet);
        if (Check()&&endGame!=null) endGame(_typePlanet);
    }
    /// <summary>
    /// ������� �������� ������� �� ������ �������������� ������
    /// </summary>
    /// <param name="newPlanet"></param>
    public virtual void RemovePlanet(Planet newPlanet)
    {
        _controledPlanets.Remove(newPlanet);
    }
}
