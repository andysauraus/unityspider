using UnityEngine;
using System.Collections;

public class ButtonGettingClicked : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClickedAButton(GameObject button)
    {
        GetComponent<UnityEngine.UI.Text>().text = button.name;
    }
}
