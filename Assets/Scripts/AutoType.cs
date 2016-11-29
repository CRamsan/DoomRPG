using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoType : MonoBehaviour
{

    public float letterPause = 0.2f;
    public AudioClip sound;

    private Text text;
    private string message;

    // Use this for initialization
    void OnEnable()
    {
        text = GetComponent<Text>();
        message = text.text;
        text.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            text.text += letter;
            if (sound)
                GetComponent<AudioSource>().PlayOneShot(sound);
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
    }

    void OnDisable()
    {
        text.text = message;
    }
}