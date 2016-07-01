using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpiderBehaviour : MonoBehaviour
{
    float timeBetweenKeyPresses = 1.0f;
    float currentTimer;
    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        currentTimer = timeBetweenKeyPresses;
	}

    [RuntimeInitializeOnLoadMethod]
    private static void Load()
    {
        new GameObject("UISpider").AddComponent<SpiderBehaviour>();
    }

    void Update()
    {
        currentTimer -= Time.unscaledDeltaTime;
        if(currentTimer < 0)
        {
            MakeDescision();
            currentTimer = timeBetweenKeyPresses;
            Logging.Log("pressing a button");
        }
    }

	public void MakeDescision()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        int selection = Random.Range(0, buttons.Length);

        if(buttons[selection].IsInteractable())
        {
            buttons[selection].onClick.Invoke();
            Logging.Log("clicked on " + buttons[selection]);
        }
    }
}
