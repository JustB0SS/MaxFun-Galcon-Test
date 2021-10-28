using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    //UI панель на которой распологаются кнопки начала игры с раной сложностью
    public GameObject panelDifficult;

    private void Awake()
    {
        panelDifficult.SetActive(false);
    }
    /// <summary>
    /// Функция срабатывает при нажатие на кнопку StartGame
    /// необходима для отображения панели с уровнями сложнястями 
    /// </summary>
    public void DifficultMenuOnClick()
    {
        panelDifficult.SetActive(true);
    }
    /// <summary>
    /// Функция сробатывает при нажатие на кнопку Возврата (Крестик) 
    /// необходима для скрытия панели с уровнями сложнястями 
    /// </summary>
    public void ReturnOnClick()
    {
        panelDifficult.SetActive(false);
    }
    /// <summary>
    /// Функция сробатывает при нажатие на одну из кнопк уровней сложности  
    /// необходима для старта уровня с определенным уровнем сложности
    /// </summary>
    /// <param name="difficult">Уровень сложности представляющий из себя скриптовый объект с зарание указынными значниями</param>
    public void StartGameOnClick(Difficult difficult)
    {
        DataHandler.difficult = difficult;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Проверка на нажатие аппаратной кнопки выхода под Android
    /// </summary>
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
