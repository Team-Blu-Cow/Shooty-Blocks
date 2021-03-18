using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private string m_applicationPath;
    private UserData m_userData;

    // Start is called before the first frame update
    private void Start()
    {
        m_applicationPath = Application.persistentDataPath;
        m_userData = new UserData(m_applicationPath);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}