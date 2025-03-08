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
	[SerializeField] private MaterialBehaviour tennisBall;
	[SerializeField] private MaterialBehaviour snuPicture;
	[SerializeField] private MaterialBehaviour books;
	[SerializeField] private MaterialBehaviour ds;
	
	[Header("Computer")]
	[SerializeField] private bool isComputerOn;
	[SerializeField] private Image powerButtonImage;
	[SerializeField] private Sprite powerButtonOnSprite, powerButtonOffSprite;
	[SerializeField] private GameObject powerOffPanel;
	[SerializeField] private GameObject desktopScreen;
	[SerializeField] private GameObject folderScreen;
	
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

		{ "tkd", new []
			{
				"태권도부는 나에게 안식처 같은 곳이야.",
				"좋은 친구들과 함께 웃고 떠들며 운동을 하다보면 많은 힘을 얻을 수 있어.",
				"이 사진은 얼마전 도쿄대 태권도부와 교류전을 진행했을 때 찍은 사진이야.",
				"정말 감사하게도 도쿄대 분들이 매년 우리 태권도부를 찾아와 주고 계셔.",
				"한국에 머무르는 동안 맛집도 가고, 술도 마시고, 정말 즐거웠어!",
				"이번 여름 방학에는 도쿄에 찾아가서 함께 놀 생각이야."
			}
		},

		{ "gdc", new []
			{
				"내 꿈은 재밌는 게임을 만드는 거야.",
				"그 꿈을 이루기 위해 들어간 동아리가 중앙 게임 개발 동아리 SNUGDC야.",
				"이번 학기에는 SNUGDC에서 홍보장으로 활동하게 되었어!",
				"얼마전 동소제에서는 실무진 1인 개발 대회에 참여하기도 했어.",
				"사실 이 프로그램도 1인 개발 대회 출품작을 개조해서 만든 거야."
			}
		},

		{
			"steam", new []
			{
				"나는 주로 STEAM에서 게임을 해.",
				"STEAM에서는 대형 개발사에서 개발한 AAA급 게임부터 소수의 인원들이 개발한 인디게임까지 다양한 게임들을 즐길 수 있어.",
				"가장 좋아하는 게임은 Project Zomboid라는 게임이야.",
				"그 외에도 시티즈, 문명, 더 헌터라는 게임도 좋아해!",
				"슬프게도 요즘에는 코딩을 하느라 게임을 많이 하지 못했어.",
				"게임을 만드느라 게임을 못하는 안타까운 상황이 되어버렸지."
			}
		},

		{
			"tennis", new []
			{
				"나는 고등학교를 다니던 때에 전혀 공부를 하지 않았어.",
				"어른들이 입을 모아 \"공부해라\"라고 말하는 데에 반발감이 들었거든.",
				"그래서 과감하게 대학 진학을 포기하고 내가 하고 싶은 일에 대해 생각해 봤어.",
				"자연스럽게 내 진로는 그때 당시 푹 빠져 있던 테니스로 정해졌어.",
				"국제 테니스 심판이 되기 위해서 국내 테니스 심판 자격증도 취득하고 영어 공부도 열심히 했어.",
				"하지만, 고등학교 졸업 후 시작한 테니스 심판 생활은 내 상상과는 거리가 있었어.",
				"열악한 환경, 고된 일, 사람들과의 신경전 속에서 이번에는 내 진로가 아닌 \'나\' 그 자체에 대해 진지하게 고민하게 되었어.",
				"내가 가장 좋아하는 일은 무엇일까?",
				"내게 가장 소중한 것은 무엇일까?",
				"내가 가장 행복할 때는 언제일까?",
				"그 고민 끝에는 게임이 있었어.",
				"그렇게 약 1년 반 동안의 테니스 심판 생활은 막을 내리게 되었고 나는 수능 공부를 시작했어.",
				"수능 공부를 할 때에는 1년 반 동안 허송 세월을 보냈다는 불안감이 존재했어.",
				"하지만 지금 돌이켜보면 전혀 그렇지 않은 것 같아.",
				"그 시간 동안 아주 많은 것들을 배울 수 있으니까 말이야."
			}
		},
		
		{ "snu", new []
			{
				"나는 자유전공학부에 23학번으로 입학해서 컴퓨터공학과 정보문화학을 전공하고 있어.",
				"처음 입학할 때는 정말 기뻤는데...",
				"지금은 30살 전에 졸업이나 할 수 있을지 걱정이야.",
				"좋은 학교 오래 다녀야지~"
			}
		},

		{
			"books", new []
			{
				"비록 나는 컴퓨터를 공부하는 공학도이지만, 문학을 사랑해.",
				"좋은 글을 읽으면 달콤한 사과 냄새, 비가 온 뒤의 아스팔트 냄새, 밀려오는 바다의 짠 냄새, 온갖 냄새를 다 맡을 수 있거든.",
				"가장 좋아하는 작가는 김유정이고, 가장 좋아하는 작품은 소낙비야.",
				"소낙비를 읽을 때면 한 여름의 쿱쿱하고 불쾌한 냄새가 나.",
				"수능 문학 속 김유정은 순수함을 그려내는 작가라는 느낌이 강했지만, 김유정 전집 속 김유정은 전혀 다르다는 것을 깨닫고 큰 충격을 받았어."
			}
		},

		{
			"ds", new []
			{
				"게임에 관한 나의 가장 오래된 기억은 닌텐도ds를 처음 받았을 때야.",
				"초등학교 2학년 크리스마스 선물로 부모님을 조르고 졸라서 얻은 게임기지.",
				"엄마는 절대 못 사주신다며 나를 꾸짖었지만, 결국에는 크리스마스 당일날 아빠가 닌텐도ds를 들고 오시더라.",
				"아빠는 TT칩에 여러 게임들을 넣어주셨어.",
				"그때는 이게 불법인지도 모르고 재밌게 플레이했었지.",
				"동물의 숲, 리듬 히어로, 리듬 천국, 마리오64, 레이튼 교수 시리즈...",
				"매일같이 수많은 게임들을 플레이했어.",
				"게임에 행복이라는 단어를 결부하기 시작한 것은 이때부터인 것 같아."
			}
		}
	};

	private void Awake()
	{
		isComputerOn = false;
		powerOffPanel.SetActive(true);
		desktopScreen.SetActive(true);
		folderScreen.SetActive(false);
		
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
	public void TennisClicked()
	{
		if (isIntroducing) return;
		StartCoroutine(StartIntroducing("tennis"));
		tennisBall.TurnOffMaterial();
	}
	public void SnuPictureClicked()
	{
		if (isIntroducing) return;
		StartCoroutine(StartIntroducing("snu"));
		snuPicture.TurnOffMaterial();
	}
	public void BooksClicked()
	{
		if (isIntroducing) return;
		StartCoroutine(StartIntroducing("books"));
		books.TurnOffMaterial();
	}
	public void DsClicked()
	{
		if (isIntroducing) return;
		StartCoroutine(StartIntroducing("ds"));
		ds.TurnOffMaterial();
	}




	public void PowerButtonClicked()
	{
		if (isIntroducing) return;
		GameManager.PlaySfx(1);
		if (isComputerOn) 
		{
			isComputerOn = false;
			powerButtonImage.sprite = powerButtonOffSprite;
			powerOffPanel.SetActive(true);
		}
		else
		{
			isComputerOn = true;
			powerButtonImage.sprite = powerButtonOnSprite;
			powerOffPanel.SetActive(false);
		}
	}
	public void FolderIconClicked()
	{
		if (isIntroducing) return;
		GameManager.PlaySfx(2);
		desktopScreen.SetActive(false);
		folderScreen.SetActive(true);
	}
	public void CloseFolderClicked()
	{
		if (isIntroducing) return;
		GameManager.PlaySfx(2);
		desktopScreen.SetActive(true);
		folderScreen.SetActive(false);
	}

	public void GdcClicked()
	{
		if (isIntroducing) return;
		GameManager.PlaySfx(2);
		StartCoroutine(StartIntroducing("gdc"));
	}
	public void SteamClicked()
	{
		if (isIntroducing) return;
		GameManager.PlaySfx(2);
		StartCoroutine(StartIntroducing("steam"));
	}
}
