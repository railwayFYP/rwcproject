using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateVolValueScript : MonoBehaviour
{
	public Text text;

	public void UpdateText(float f)
	{
		text.text = f.ToString ("F1");
	}
}
