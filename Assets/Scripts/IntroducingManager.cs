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
	
	[Header("Material Behaviours")]
	[SerializeField] private MaterialBehaviour familyPicture;
	[SerializeField] private MaterialBehaviour tkdPicture;
	
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
		},
		
		{ "snu", new []
			{
				"나는 자유전공학부에 23학번으로 입학했어.",
				""
			}
		},

		{ "tkd", new []
			{
				"태권도부는 나에게 안식처 같은 곳이야.",
				"좋은 친구들과 함께 웃고 떠들며 운동을 하다보면 많은 힘을 얻을 수 있어.",
				"이 사진은 얼마전 도쿄대 태권도부와 교류전을 진행했을 때 찍은 사진이야.",
				"정말 감사하게도 도쿄대 분들이 매년 우리 태권도부를 찾아와 주고 계셔.",
				"한국에 머무르는 동안 맛집도 가고, 술도 마시고, 정말 즐거웠어!",
				"이번 여름 방학에는 도쿄에 찾아가서 함께 놀 생각이야."
			}
		}
	};

	private void Awake()
	{
		powerOffPanel.SetActive(true);
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
		
		yield return StartIntroducing("greeting");
		GameManager.PlayBgm(0);
		GameManager.SetBgmVolume(0.1f);
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
			speechBubbleText.text = "";
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
		if (isIntroducing) return;
		StartCoroutine(StartIntroducing("family picture"));
		familyPicture.TurnOffMaterial();
	}

	public void TkdPictureClicked()
	{
		if (isIntroducing) return;
		StartCoroutine(StartIntroducing("tkd"));
		tkdPicture.TurnOffMaterial();
	}
}
