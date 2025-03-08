using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
  public static RoomManager Instance { get; private set; }
  
  [Header("References")]
  [SerializeField] private ComputerManager computerManager;
  [SerializeField] private ButtonManager buttonManager;
  [SerializeField] private TakeALookBehaviour takeALookBehaviour;
  [SerializeField] private GameObject fadePanel, greetingPanel, achievement;
  [SerializeField] private Image achievementImage;
  [SerializeField] private TextMeshProUGUI achievementName, achievementDescription;
  [SerializeField] private GameObject speechBubble;
  [SerializeField] private TextMeshProUGUI speechBubbleText;

  [Header("States")]
  [SerializeField] private bool isPasswordCleared = false;
  [SerializeField] private bool isButtonCodeCleared = false;
  [SerializeField] private bool isTakeALookCodeCleared = false;
  
  [Header("Values")]
  [SerializeField] private float showAchievementDuration;
  [SerializeField] private float waitAfterEndFound, fadeDuration, InitDuration, SpeechBubbleDuration;
  [SerializeField] private int greetingNum;

  [Header("Easter Eggs")]
  [SerializeField] private GameObject easterEgg1;
  [SerializeField] private GameObject easterEgg2;
  [SerializeField] private Sprite easterEgg1AchieveSprite, easterEgg2AchieveSprite;

  [Header("Ends")]
  [SerializeField] private Image end1Image;
  [SerializeField] private Image end2Image;
  [SerializeField] private Image end3Image;
  [SerializeField] private Image end4Image;
  [SerializeField] private Sprite end1Sprite, end2Sprite, end3Sprite, end4Sprite;
  [SerializeField] private Sprite end1PositionSprite, end2PositionSprite, end3PositionSprite, end4PositionSprite;
  [SerializeField] private Sprite end1AchieveSprite, end2AchieveSprite, end3AchieveSprite, end4AchieveSprite;
  
  private enum RoomStates
  {
    Loading,
    Greeting,
    Exploring,
    Ending,
  }
  
  [Header("State")]
  [SerializeField] private RoomStates roomState = RoomStates.Loading;

  private void Awake()
  {
    fadePanel.SetActive(true);
    greetingNum = 0;
  }

  private void Start()
  {
    DoInit();
  }

  private void DoInit()
  {
    computerManager.InitComputer();
    buttonManager.InitButtons();
    takeALookBehaviour.InitTakeALook();
    StartCoroutine(InitRoom());
  }
  
  private IEnumerator InitRoom()
  {
    // Easter Egg
    if (GameManager.Instance.egg1) easterEgg1.SetActive(true);
    else easterEgg1.SetActive(false);
    if (GameManager.Instance.egg2) easterEgg2.SetActive(true);
    else easterEgg2.SetActive(false);
    
    // Ending
    if (GameManager.Instance.end1) end1Image.sprite = end1Sprite;
    else end1Image.sprite = end1PositionSprite;
    if (GameManager.Instance.end2) end2Image.sprite = end2Sprite;
    else end2Image.sprite = end2PositionSprite;
    if (GameManager.Instance.end3) end3Image.sprite = end3Sprite;
    else end3Image.sprite = end3PositionSprite;
    if (GameManager.Instance.end4) end4Image.sprite = end4Sprite;
    else end4Image.sprite = end4PositionSprite;
    
    achievement.SetActive(false);
    speechBubble.SetActive(false);
    speechBubbleText.text = "";
    
    
    yield return FadeIn();
    roomState = RoomStates.Greeting;
    StartCoroutine(DoGreeting());
  }

  private IEnumerator FadeIn()
  {
    fadePanel.SetActive(true);
    var elapsedTime = 0f;
    var fadeImage = fadePanel.GetComponent<Image>();
    
    while (elapsedTime < fadeDuration)
    {
      fadeImage.color = new Color(0.3215686f, 0.2901961f, 0.3098039f, 1f - elapsedTime);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
    fadePanel.SetActive(false);
  }

  private IEnumerator FadeOut()
  {
    fadePanel.SetActive(true);
    var elapsedTime = 0f;
    var fadeImage = fadePanel.GetComponent<Image>();
    
    while (elapsedTime < fadeDuration)
    {
      fadeImage.color = new Color(0.3215686f, 0.2901961f, 0.3098039f, elapsedTime);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
    fadeImage.color = new Color(0.3215686f, 0.2901961f, 0.3098039f, 1f);
    GameManager.SetBgmVolume(0f);
    GameManager.StopBgm();
  }

  private IEnumerator DoGreeting()
  {
    greetingPanel.SetActive(true);

    switch (greetingNum)
    {
      case 0:
        speechBubbleText.text = "안녕? 오랜만이야.";
        speechBubble.SetActive(true);
        GameManager.PlaySfx(0);
        yield return new WaitForSeconds(SpeechBubbleDuration);
        speechBubbleText.text = "천천히, 구석구석 잘 살펴봐";
        GameManager.PlaySfx(0);
        yield return new WaitForSeconds(SpeechBubbleDuration);
        break;
      
      case 1:
        speechBubbleText.text = "오랜만이라 비밀번호가 잘 기억이 안 나는 거야?";
        speechBubble.SetActive(true);
        GameManager.PlaySfx(0);
        yield return new WaitForSeconds(SpeechBubbleDuration);
        speechBubbleText.text = "예전에 네가 이 방 어딘가에 적어뒀으니 잘 생각해봐";
        GameManager.PlaySfx(0);
        yield return new WaitForSeconds(SpeechBubbleDuration);
        break;
      
      case 2:
        speechBubbleText.text = "저 꽃은 내 소중한 친구야.";
        speechBubble.SetActive(true);
        GameManager.PlaySfx(0);
        yield return new WaitForSeconds(SpeechBubbleDuration);
        speechBubbleText.text = "그러니 친절하게 대해줘.";
        GameManager.PlaySfx(0);
        yield return new WaitForSeconds(SpeechBubbleDuration);
        break;
      
      case 3:
        speechBubbleText.text = "감전되면 위험하니까 조심해.";
        speechBubble.SetActive(true);
        GameManager.PlaySfx(0);
        yield return new WaitForSeconds(SpeechBubbleDuration);
        break;
      
      default: break;
    }
    
    speechBubble.SetActive(false);

    GameManager.PlayBgm(0);
    GameManager.SetBgmVolume(0.1f);
    roomState = RoomStates.Exploring;
    greetingPanel.SetActive(false);
  }

  public void ButtonCodeCleared()
  {
    isButtonCodeCleared = true;
    buttonManager.UnlockDraw(1);
  }

  public void PasswordCleared() { isPasswordCleared = true; }

  public void TakeALookCodeCleared()
  {
    isTakeALookCodeCleared = true;
    StartCoroutine(EndingFound(4));
  }

  public IEnumerator EasterEggFound(int eggId)
  {
    switch (eggId)
    {
      case 1:
        if (GameManager.Instance.egg1) yield break;
        achievementImage.sprite = easterEgg1AchieveSprite;
        achievementName.text = "Easter Egg 1";
        achievementDescription.text = "에옹";
        GameManager.Instance.egg1 = true; 
        var easterEgg1Image = easterEgg1.GetComponent<Image>();
        easterEgg1Image.color = new Color(1f, 1f, 1f, 0f);
        easterEgg1.SetActive(true);
        var elapsedTime1 = 0f;
    
        while (elapsedTime1 < fadeDuration)
        {
          easterEgg1Image.color = new Color(1f, 1f, 1f, elapsedTime1);
          elapsedTime1 += Time.deltaTime;
          yield return null;
        }
        easterEgg1Image.color = new Color(1f, 1f, 1f, 1f);
        break;
      case 2:
        if (GameManager.Instance.egg2) yield break;
        achievementImage.sprite = easterEgg2AchieveSprite;
        achievementName.text = "Easter Egg 2";
        achievementDescription.text = "고마워요";
        GameManager.Instance.egg2 = true; 
        
        var easterEgg2Image = easterEgg2.GetComponent<Image>();
        easterEgg2Image.color = new Color(1f, 1f, 1f, 0f);
        easterEgg2.SetActive(true);
        var elapsedTime2 = 0f;
    
        while (elapsedTime2 < fadeDuration)
        {
          easterEgg2Image.color = new Color(1f, 1f, 1f, elapsedTime2);
          elapsedTime2 += Time.deltaTime;
          yield return null;
        }
        easterEgg2Image.color = new Color(1f, 1f, 1f, 1f);
        break;
      default: Debug.LogError("앙"); yield break;
    }
    
    StartCoroutine(ShowAchievement(14));
  }
  

  public IEnumerator EndingFound(int endId)
  {
    roomState = RoomStates.Ending;
    
    switch (endId)
    {
      case 1:
        if (!GameManager.Instance.end1)
        {
          greetingNum = 1;
          GameManager.Instance.end1 = true;
          achievementImage.sprite = end1AchieveSprite;
          achievementName.text = "End 1";
          achievementDescription.text = "누가 써둔 거야!";
          StartCoroutine(ShowAchievement(15));

          end1Image.color = new Color(1f, 1f, 1f, 0f);
          end1Image.sprite = end1Sprite;
          var elapsedTime1 = 0f;
          while (elapsedTime1 < fadeDuration)
          {
            end1Image.color = new Color(1f, 1f, 1f, elapsedTime1);
            elapsedTime1 += Time.deltaTime;
            yield return null;
          }

          end1Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else greetingNum = 4;
        break;
      
      case 2:
        if (!GameManager.Instance.end2)
        {
          greetingNum = 2;
          GameManager.Instance.end2 = true;
          achievementImage.sprite = end2AchieveSprite;
          achievementName.text = "End 2";
          achievementDescription.text = "아야";
          StartCoroutine(ShowAchievement(15));
          
          end2Image.color = new Color(1f, 1f, 1f, 0f);
          end2Image.sprite = end2Sprite;
          var elapsedTime2 = 0f;
          while (elapsedTime2 < fadeDuration)
          {
            end2Image.color = new Color(1f, 1f, 1f, elapsedTime2);
            elapsedTime2 += Time.deltaTime;
            yield return null;
          }
          end2Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else greetingNum = 4;
        break;
      
      case 3:
        if (!GameManager.Instance.end3)
        {
          greetingNum = 3;
          GameManager.Instance.end3 = true;
          achievementImage.sprite = end3AchieveSprite;
          achievementName.text = "End 3";
          achievementDescription.text = "찌릿찌릿";
          StartCoroutine(ShowAchievement(15));
          
          end3Image.color = new Color(1f, 1f, 1f, 0f);
          end3Image.sprite = end3Sprite;
          var elapsedTime3 = 0f;
          while (elapsedTime3 < fadeDuration)
          {
            end3Image.color = new Color(1f, 1f, 1f, elapsedTime3);
            elapsedTime3 += Time.deltaTime;
            yield return null;
          }
          end3Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else greetingNum = 4;
        takeALookBehaviour.TakeALookPowerOff();
        break;
      
      case 4:
        if (!GameManager.Instance.end4)
        {
          GameManager.Instance.end4 = true;
          achievementImage.sprite = end4AchieveSprite;
          achievementName.text = "End 4";
          achievementDescription.text = "";
          StartCoroutine(ShowAchievement(99));
          
          end4Image.color = new Color(1f, 1f, 1f, 0f);
          end4Image.sprite = end4Sprite;
          var elapsedTime4 = 0f;
          while (elapsedTime4 < fadeDuration)
          {
            end4Image.color = new Color(1f, 1f, 1f, elapsedTime4);
            elapsedTime4 += Time.deltaTime;
            yield return null;
          }
          end4Image.color = new Color(1f, 1f, 1f, 1f);
        }
        greetingNum = 4;
        takeALookBehaviour.TakeALookPowerOff();
        break;
      
      default: yield break;
    }
    
    yield return new WaitForSeconds(waitAfterEndFound);
    yield return FadeOut();
    yield return new WaitForSeconds(InitDuration);
    DoInit();
  }

  private IEnumerator ShowAchievement(int sfxIdx)
  {
    achievement.SetActive(true);
    if (sfxIdx != 99) GameManager.PlaySfx(sfxIdx);
    yield return new WaitForSeconds(showAchievementDuration);
    achievement.SetActive(false);
  }
}
