using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WindowsInput;

public class SpiderBehaviour : MonoBehaviour
{
    float timeBetweenKeyPresses = 1.0f;
    float currentTimer;
    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        currentTimer = timeBetweenKeyPresses;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

    void Update()
    {
        currentTimer -= Time.deltaTime;
        if(currentTimer < 0)
        {
            makeDescision();
            currentTimer = timeBetweenKeyPresses;
            Debug.Log("pressing a button");
        }
    }

	public void makeDescision()
    {
        Button[] buttons =FindObjectsOfType<Button>();

        int selection = Random.Range(0, buttons.Length);

        if(buttons[selection].isActiveAndEnabled)
        {
            buttons[selection].onClick.Invoke();
        }
    }
}
