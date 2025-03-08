using System;
using UnityEngine;
using UnityEngine.UI;

public class MaterialBehaviour : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Image image;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	public void ObjectClicked()
	{
		image.material = null;
	}
}
