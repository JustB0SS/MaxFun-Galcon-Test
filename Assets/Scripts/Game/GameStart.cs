using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    // Таблица соотношение Цветов и типов Планет
    public static Hashtable hashtableColors = new Hashtable();

    // Префаб необходимый для создания объектов типа планета
    public Planet planetPrefab;
    // Родитель внутри которого будет происходить генерация планет
    public Transform poolPlanets;
    //UI элементы для отображения окончания уровня
    public Text winningText;
    public GameObject winningPanel;

    // Цвета для окраски планет и кораблей
    public Color playerColor;
    public Color neutralColor;
    public Color enemyColor;

    // UI элемент необходимый для отрисовки выделения 
    public RectTransform _selectBox;
    // Стандартная сложность на случай если при загрузки сцены произошел сбой и не удалось передать сложность выбранную в меню
    public Difficult defaultDifficlut;
    


    // список сгенерированых планет
    private List<Planet> _planets = new List<Planet>();
    // максимальное кол-во итерация при генерации на случай если не удается сгенерировать определенное кол-во планет в области
    private const int _maxiter = 1000000;
    // базавый радиус планеты
    private float _baseRadius;
    // высота и ширина экрана
    private int _width;
    private int _height;

    //реальная высота и ширина области камеры
    private float _realWidth;
    private float _realHeight;

    // экземляры игрока и бота
    private EnemyAI enemy;
    private Player player;

    // переменная в которую передается и хранится сложность уровня
    private Difficult _difficult;

    // делегат для удаления и добавления планет в распоряжения игрока/бота при захвате планеты
    public delegate void ActionPlanetDelegate(Planet planet);
    public ActionPlanetDelegate addPlanetToPlayerRemoveFromEnemy;
    public ActionPlanetDelegate addPlanetToEnemyRemoveFromPlayer;


    /// <summary>
    /// Функция устанавливающая конец игры
    /// с выводом победителя на экран
    /// </summary>
    /// <param name="type">тип победителя</param>
    public void EndGame(Planet.TypePlanet type)
    {
        enemy.StopAllCoroutines();
        foreach (var planet in _planets)
        {
            planet.StopAllCoroutines();
        }
        if (type == Planet.TypePlanet.Player)
        {
            winningPanel.SetActive(true);
            winningText.text = "Player win";
        }else if(type == Planet.TypePlanet.Enemy)
        {
            winningPanel.SetActive(true);
            winningText.text = "Bot win";
        }

    }

    /// <summary>
    /// Проверка подходит ли параметры для генерации планеты,
    /// то есть находится ли планета на расстояние от остальных планет как минимум на сумму их радиусов
    /// </summary>
    /// <param name="coord">координаты новой планеты</param>
    /// <param name="radius">радиус планеты </param>
    /// <returns></returns>
    private bool CheckDistanceBetweenPlanet(Vector2 coord,float radius)
    {
        foreach (var planet in _planets)
        {
            if (Vector2.Distance(planet.Coord, coord) < radius*2 + planet.Radius*2)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Функция для нахождения самой удаленной планеты от планеты переданной в качестве параметра
    /// </summary>
    /// <param name="fromPlanet">планета относительно которой ищется самая удаленная от неё планета</param>
    /// <returns></returns>
    private Planet MaxDistance(Planet fromPlanet)
    {
        Planet ans = null;
        float dist = 0;
        foreach(var planet in _planets)
        {
            float newDist = Vector2.Distance(planet.Coord, fromPlanet.Coord);
            if (newDist > dist)
            {
                ans = planet;
                dist = newDist;
            }
        }
        return ans;
    }
    /// <summary>
    /// Функция возврата в главное меню
    /// </summary>    
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        if (DataHandler.difficult == null)
            _difficult = defaultDifficlut;
        else
            _difficult = DataHandler.difficult;
        player = GetComponent<Player>();
        enemy = GetComponent<EnemyAI>();
        hashtableColors[Planet.TypePlanet.Player] = playerColor;
        hashtableColors[Planet.TypePlanet.Enemy] = enemyColor;
        hashtableColors[Planet.TypePlanet.Neutral] = neutralColor;
        Init();
    }

    /// <summary>
    /// Функция инициализации всей игры, а точнее 
    /// генерация всех планет, инициализация игрока и бота
    /// </summary>
    private void Init()
    {
        float size = Camera.main.orthographicSize;
        _width = Screen.width;
        _height = Screen.height;
        float minWH = Mathf.Min(_width, _height);
        _realWidth = _width / minWH * size;
        _realHeight = _height / minWH * size;
        _baseRadius = planetPrefab.gameObject.transform.localScale.x/2;

        // генерация планет
        int countPlanet = Random.Range(_difficult.minRandomSpawnPlanet, _difficult.maxRandomSpawnPlanet);
        for(int i = 0,j=0; i < countPlanet;j++)
        {
            if (j == _maxiter) break;

            float randomRadiusKoef = Random.Range(1f, _difficult.maxRandomRadius);
            float radius = randomRadiusKoef * _baseRadius;
            float spawnXRange = _realWidth - radius;
            float spawnYRange = _realHeight - radius;
            float x = Random.Range(-spawnXRange, spawnXRange);
            float y = Random.Range(-spawnYRange, spawnYRange);
            if(CheckDistanceBetweenPlanet(new Vector2(x, y), radius))
            {
                i++;
                var planet = Instantiate(planetPrefab, poolPlanets);
                Planet.RectPlanet rect;
                rect.x = x - radius + _realWidth;
                rect.x1 = x + radius + _realWidth;
                rect.y = y + radius + _realHeight;
                rect.y1 = y - radius + _realHeight;
                planet.Init(rect,radius, x, y,this);
                _planets.Add(planet);
                
            }
        }

        // Инициализация Игрока
        player.Init(_planets[_planets.Count - 1], _planets, Planet.TypePlanet.Player, _selectBox);
        addPlanetToPlayerRemoveFromEnemy += player.AddPlanet;
        addPlanetToPlayerRemoveFromEnemy += enemy.RemovePlanet;

        // Инициализация Врага
        enemy.Init(MaxDistance(_planets[_planets.Count - 1]), _planets, Planet.TypePlanet.Enemy, _difficult);
        addPlanetToEnemyRemoveFromPlayer += enemy.AddPlanet;
        addPlanetToEnemyRemoveFromPlayer += player.RemovePlanet;
        player.endGame = EndGame;
        enemy.endGame = EndGame;
        
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
