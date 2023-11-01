// Script for having a typewriter effect for UI
// Prepared by Nick Hwang (https://www.youtube.com/nickhwang)
// Want to get creative? Try a Unicode leading character(https://unicode-table.com/en/blocks/block-elements/)
// Copy Paste from page into Inpector

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class typewriterUI : MonoBehaviour
{
	Text _text;
	TMP_Text _tmpProText;
    public AudioSource tecla;

	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
	[SerializeField] string leadingChar = "";
	[SerializeField] bool leadingCharBeforeDelay = false;
    public bool rotinaUm;
    public bool rotinaDois;

	// Use this for initialization
	public void Comecar(string writer)
	{
		_text = GetComponent<Text>()!;
		_tmpProText = GetComponent<TMP_Text>()!;

		if(_text != null)
        {

			StartCoroutine(TypeWriterText(writer));
		}

		if (_tmpProText != null)
		{
			writer = _tmpProText.text;

			StartCoroutine(TypeWriterTMP(writer));
		}
	}

	IEnumerator TypeWriterText(string writer)
	{
        rotinaUm = true;
		_text.text += leadingCharBeforeDelay ? leadingChar : "";
        for(int le = 0; le < leadingChar.Length; le++){
            _text.text += " ";
        }
		yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
            tecla.pitch = Random.Range(0.5f, 3f);
            tecla.Play(0);
			if (_text.text.Length > 0)
			{
				_text.text = _text.text.Substring(0, _text.text.Length - leadingChar.Length);
			}
			_text.text += c;
			_text.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if(leadingChar != "")
        {
			_text.text = _text.text.Substring(0, _text.text.Length - leadingChar.Length);
		}
        rotinaUm = false;
	}

	IEnumerator TypeWriterTMP(string writer)
    {
        rotinaDois = true;
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
            tecla.pitch = Random.Range(0.5f, 1.5f);
            tecla.Play(0);
			if (_tmpProText.text.Length > 0)
			{
				_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
			}
			_tmpProText.text += c;
			_tmpProText.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (leadingChar != "")
		{
			_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
		}
        rotinaDois = false;
	}
}