using System;
using UnityEngine;
using UnityEngine.UI;

public class MaterialBehaviour : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Material outlineMaterial;

	private void Awake()
	{
		outlineMaterial = GetComponent<Image>().material;
	}

	public void ObjectClicked()
	{
		outlineMaterial = null;
	}
}
