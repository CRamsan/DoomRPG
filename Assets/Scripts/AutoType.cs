using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoType : MonoBehaviour
{
    public float typeWaitTime = 0.2f;
    public AudioClip typeSoundClip;

	private Text textComponent;
	private string textMessage;

    // Use this for initialization
    void OnEnable()
    {
		StartTyping ();
    }

    IEnumerator TypeText()
    {
        foreach (char letter in textMessage.ToCharArray())
        {
            textComponent.text += letter;
			if (typeSoundClip != null) {
				GetComponent<AudioSource> ().PlayOneShot (typeSoundClip);
			}
            yield return 0;
            yield return new WaitForSeconds(typeWaitTime);
        }
    }

    void OnDisable()
    {
		FinishTyping ();
    }

	public void StartTyping() {
		textComponent = GetComponent<Text>();
		textMessage = textComponent.text;
		textComponent.text = "";
		StartCoroutine(TypeText());
	}

	public void FinishTyping(){
		textComponent.text = textMessage;
	}
}