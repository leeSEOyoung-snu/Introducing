using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroducingManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject speechBubble;
	[SerializeField] private TextMeshProUGUI speechBubbleText;
	[SerializeField] private GameObject fadePanel;
	[SerializeField] private GameObject powerOffPanel;
	
	[Header("Values")]
	[SerializeField] private float speechBubbleDuration;
	[SerializeField] private float typingSpeed;
	[SerializeField] private float fadeDuration;
	
	[Header("States")]
	[SerializeField] private bool isIntroducing;
	[SerializeField] private bool isSkipClicked;
	
	private Dictionary<string, string[]> _introducingData = new Dictionary<string, string[]>()
	{
		{ "greeting", new []
			{
				"안녕? 내 이름은 이서영이야. 만나서 반가워.",
				"나에 대해 궁금한 점이 있다면, 이 방 안의 물건들을 클릭해봐!"
			}
		},
		
		{ "family picture", new []
			{
				"나는 형제자매 없이 엄마, 아빠 셋이서만 살았어.",
				"외동으로 태어났지만, 외가 친척 모두가 가까운 곳에 살았기에 전혀 외롭지 않았어.",
				"이따금씩 우리 엄마는 나에게 세 명의 엄마가 있다고 말씀하셔.",
				"나를 낳아주신 우리 엄마, 나를 키워주신 할머니, 나를 가르쳐주신 이모.",
				"그만큼 나는 많은 사랑을 받고 자랐어.",
				"이 사진은 작년 겨울 모두 함께 후쿠오카로 여행을 가서 찍은 사진이야.",
				"맛있는 것도 잔뜩 먹고, 따뜻한 물에 몸도 담그고, 나에게는 아주 행복한 기억으로 남아있어."
			}
		}
	};

	private void Awake()
	{
		powerOffPanel.SetActive(false);
		fadePanel.SetActive(true);
		speechBubble.SetActive(false);
	}

	private void Start()
	{
		StartCoroutine(InitCoroutine());
	}

	private IEnumerator InitCoroutine()
	{
		fadePanel.SetActive(true);
		var elapsedTime = 0f;
		var fadeImage = fadePanel.GetComponent<Image>();
    
		while (elapsedTime < fadeDuration)
		{
			fadeImage.color = new Color(0, 0, 0, 1f - elapsedTime);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		fadePanel.SetActive(false);
		
		StartCoroutine(StartIntroducing("greeting"));
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			if (isIntroducing) isSkipClicked = true;
	}

	private IEnumerator StartIntroducing(string key)
	{
		if (isIntroducing) yield break;
		
		isIntroducing = true;
		string[] data = _introducingData[key];
		speechBubbleText.text = "";
		speechBubble.SetActive(true);

		foreach (string sentence in data)
		{
			GameManager.PlaySfx(0);
			yield return StartTyping(sentence);
			float elapsedTime = 0f;
			while (elapsedTime < speechBubbleDuration)
			{
				if (isSkipClicked)
				{
					isSkipClicked = false;
					break;
				}
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			speechBubbleText.text = "";
			yield return null;
		}
		
		speechBubble.SetActive(false);
		isIntroducing = false;
	}

	private IEnumerator StartTyping(string sentence)
	{
		foreach (char c in sentence)
		{
			if (isSkipClicked)
			{
				speechBubbleText.text = sentence;
				isSkipClicked = false;
				yield break;
			}

			if (c == ' ')
			{
				speechBubbleText.text += " ";
				yield return null;
			}
			else
			{
				speechBubbleText.text += c;
				yield return new WaitForSeconds(typingSpeed);
			}
		}
	}

	public void FamilyPictureClicked()
	{
		
	}
}
