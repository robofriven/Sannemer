using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Typer : MonoBehaviour {
	
	public float letterPause = 0.01f;
	[HideInInspector] public AudioClip typeSound1;
	[HideInInspector] public AudioClip typeSound2;

	
	string message;
	public Text textField;
	
	// Use this for initialization
	void Start () 
	{
		//textField = GetComponent<Text>();
		message = textField.text;
		textField.text = "";
        //StartCoroutine(AppendText(corpController.hand[0].description));
	}
	
	IEnumerator TypeText () 
	{
        yield return new WaitForSeconds(letterPause * 5);
        foreach (char letter in message.ToCharArray()) 
		{
			textField.text += letter;
			if (typeSound1 && typeSound2)
				//SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);
			yield return 0;
			yield return new WaitForSeconds (letterPause);
		}
	}

    public void TypeText(string msg)
    {
        //textField = GetComponent<Text>();
        message = msg;
        textField.text = "";
        StartCoroutine(TypeText());
    }

    public IEnumerator AppendText(string message)
	{
		foreach (char letter in message.ToCharArray()) 
		{
			textField.text += letter;
			if (typeSound1 && typeSound2)
				//SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);
				yield return 0;
			yield return new WaitForSeconds (letterPause);
		}
	}
}