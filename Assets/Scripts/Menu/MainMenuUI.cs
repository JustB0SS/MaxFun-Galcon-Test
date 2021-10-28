using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    //UI ������ �� ������� ������������� ������ ������ ���� � ����� ����������
    public GameObject panelDifficult;

    private void Awake()
    {
        panelDifficult.SetActive(false);
    }
    /// <summary>
    /// ������� ����������� ��� ������� �� ������ StartGame
    /// ���������� ��� ����������� ������ � �������� ����������� 
    /// </summary>
    public void DifficultMenuOnClick()
    {
        panelDifficult.SetActive(true);
    }
    /// <summary>
    /// ������� ����������� ��� ������� �� ������ �������� (�������) 
    /// ���������� ��� ������� ������ � �������� ����������� 
    /// </summary>
    public void ReturnOnClick()
    {
        panelDifficult.SetActive(false);
    }
    /// <summary>
    /// ������� ����������� ��� ������� �� ���� �� ����� ������� ���������  
    /// ���������� ��� ������ ������ � ������������ ������� ���������
    /// </summary>
    /// <param name="difficult">������� ��������� �������������� �� ���� ���������� ������ � ������� ���������� ���������</param>
    public void StartGameOnClick(Difficult difficult)
    {
        DataHandler.difficult = difficult;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// �������� �� ������� ���������� ������ ������ ��� Android
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
