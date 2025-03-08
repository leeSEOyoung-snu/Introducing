using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ComputerManager : MonoBehaviour
{
  [Header("Values")] 
  [SerializeField] private float waitBeforeCheck;
  [SerializeField] private float buttonPressedDuration;
  
  [Header("Sprites")]
  [SerializeField] private Sprite[] numberSprites;
  [SerializeField] private Sprite[] letterSprites;
  [SerializeField] private Sprite[] symbolSprites;
  [SerializeField] private Sprite powerOnSprite, powerOffSprite;
  
  [Header("References")]
  [SerializeField] private RoomManager roomManager;
  [SerializeField] private Image powerImage;
  [SerializeField] private Image displayImage;
  [SerializeField] private GameObject powerOffPanel;
  
  
  [Header("States")]
  [SerializeField] private bool isComputerOn;
  [SerializeField] private ScreenStates screenState;
  
  [Header("Socket")]
  [SerializeField] private Image socketImage;
  [SerializeField] private Sprite socketOnSprite, socketOffSprite;
  
  [Header("LockedScreen")]
  [SerializeField] private GameObject lockedScreen;
  [SerializeField] private int passwordInputCnt;
  [SerializeField] private bool isPasswordCorrect, isEnd1Password;
  [SerializeField] private Image[] passwordImages;
  [SerializeField] private GameObject wrongScreen;
  
  [Header("DesktopScreen")]
  [SerializeField] private GameObject desktopScreen;
  
  [Header("FolderScreen")]
  [SerializeField] private GameObject folderScreen;
  
  [Header("ButtonScreen")]
  [SerializeField] private GameObject buttonScreen;
  [SerializeField] private int buttonCodeInputCnt;
  [SerializeField] private bool isButtonCodeCorrect;
  [SerializeField] private Image[] colorButtonImages;
  [SerializeField] private Sprite[] normalColorButtonSprites, pressedColorButtonSprites;
  [SerializeField] private bool[] isClickingColorButton = new bool[2];
  [SerializeField] private Image[] buttonCodeImages;
  
  [Header("TakeALook")]
  [SerializeField] private GameObject takeALookScreen;
  [SerializeField] private int takeALookCodeInputCnt;
  [SerializeField] private bool isTakeALookCodeCorrect;
  [SerializeField] private Image[] takeALookCodeImages;
  [SerializeField] private GameObject redEye;
  [SerializeField] private float redEyeFadeDuration;

  
  private enum ScreenStates
  {
    Locked,
    Desktop,
    Folder,
    Button,
    TakeALook,
  }
  
  public void InitComputer()
  {
    // Socket
    socketImage.sprite = socketOnSprite;
    
    // Computer Power
    powerImage.sprite = powerOffSprite;
    powerOffPanel.SetActive(true);
    isComputerOn = false;
    
    // LockedScreen
    foreach (var image in passwordImages) image.sprite = symbolSprites[0];
    isPasswordCorrect = isEnd1Password = true;
    passwordInputCnt = 0;
    lockedScreen.SetActive(true);
    wrongScreen.SetActive(false);
    
    // DesktopScreen
    desktopScreen.SetActive(false);
    
    // FolderScreen
    folderScreen.SetActive(false);
    
    // ButtonScreen
    foreach (var image in buttonCodeImages) image.sprite = symbolSprites[0];
    isButtonCodeCorrect = true;
    buttonCodeInputCnt = 0;
    buttonScreen.SetActive(false);
    for (int i = 0; i < isClickingColorButton.Length; i++) isClickingColorButton[i] = false;
    
    // TakeALookScreen
    foreach (var image in takeALookCodeImages) image.sprite = symbolSprites[0];
    isTakeALookCodeCorrect = true;
    takeALookCodeInputCnt = 0;
    takeALookScreen.SetActive(false);
    redEye.SetActive(false);
    
    screenState = ScreenStates.Locked;
    
    Debug.Log("InitScreen");
  }

  public void TogglePower()
  {
    if (isComputerOn)
    {
      // power on => power off
      powerImage.sprite = powerOffSprite;
      powerOffPanel.SetActive(true);
      isComputerOn = false;
    }
    else
    {
      GameManager.PlaySfx(11);
      // power off => power on
      powerImage.sprite = powerOnSprite;
      UpdateScreen();
      powerOffPanel.SetActive(false);
      isComputerOn = true;
    }
  }

  private void UpdateScreen()
  {
    Debug.Log("UpdateScreen");
  }

  public void SocketClicked()
  {
    GameManager.PlaySfx(6);
    socketImage.sprite = socketOffSprite;
    isComputerOn = true;
    TogglePower();
    StartCoroutine(roomManager.EndingFound(3));
  }

  private void Update()
  {
    // process key input
    for (KeyCode key = KeyCode.A; key <= KeyCode.Z; key++)
      if (Input.GetKeyDown(key) && isComputerOn) ProcessKeyInput(key);
    for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
      if (Input.GetKeyDown(key) && isComputerOn) ProcessKeyInput(key);
    for (KeyCode key = KeyCode.Keypad0; key <= KeyCode.Keypad9; key++)
      if (Input.GetKeyDown(key) && isComputerOn) ProcessKeyInput(key);
  }

  private void ProcessKeyInput(KeyCode key)
  {
    switch (screenState)
    {
      case ScreenStates.Locked:
        if (passwordInputCnt >= 4) break;
        switch (key)
        { // end1: 1331, password: 1290
          case KeyCode.Alpha0: case KeyCode.Keypad0 :passwordImages[passwordInputCnt].sprite = numberSprites[0]; 
            if (passwordInputCnt != 3) isPasswordCorrect = false;
            isEnd1Password = false;
            passwordInputCnt++; break;
          
          case KeyCode.Alpha1: case KeyCode.Keypad1 :passwordImages[passwordInputCnt].sprite = numberSprites[1];
            if (passwordInputCnt != 0) isPasswordCorrect = false;
            if (passwordInputCnt != 0 && passwordInputCnt != 3) isEnd1Password = false;
            passwordInputCnt++; break;
          
          case KeyCode.Alpha2: case KeyCode.Keypad2 :passwordImages[passwordInputCnt].sprite = numberSprites[2]; 
            if (passwordInputCnt != 1) isPasswordCorrect = false;
            isEnd1Password = false;
            passwordInputCnt++; break;
          
          case KeyCode.Alpha3: case KeyCode.Keypad3 :passwordImages[passwordInputCnt].sprite = numberSprites[3];
            isPasswordCorrect = false;
            if (passwordInputCnt != 1 && passwordInputCnt != 2) isEnd1Password = false;
            passwordInputCnt++; break;
          
          case KeyCode.Alpha4: case KeyCode.Keypad4 :passwordImages[passwordInputCnt].sprite = numberSprites[4]; 
            isPasswordCorrect = false;
            isEnd1Password = false;
            passwordInputCnt++; break;
          
          case KeyCode.Alpha5: case KeyCode.Keypad5 :passwordImages[passwordInputCnt].sprite = numberSprites[5]; 
            isPasswordCorrect = false;
            isEnd1Password = false;
            passwordInputCnt++; break;
          
          case KeyCode.Alpha6: case KeyCode.Keypad6 :passwordImages[passwordInputCnt].sprite = numberSprites[6]; 
            isPasswordCorrect = false;
            isEnd1Password = false;
            passwordInputCnt++; break;
          
          case KeyCode.Alpha7: case KeyCode.Keypad7 :passwordImages[passwordInputCnt].sprite = numberSprites[7]; 
            isPasswordCorrect = false;
            isEnd1Password = false;
            passwordInputCnt++; break;
          
          case KeyCode.Alpha8: case KeyCode.Keypad8 :passwordImages[passwordInputCnt].sprite = numberSprites[8]; 
            isPasswordCorrect = false;
            isEnd1Password = false;
            passwordInputCnt++; break;
          
          case KeyCode.Alpha9: case KeyCode.Keypad9 :passwordImages[passwordInputCnt].sprite = numberSprites[9]; 
            if (passwordInputCnt != 2) isPasswordCorrect = false;
            isEnd1Password = false;
            passwordInputCnt++; break;
          
          default: break;
        }
        if (passwordInputCnt == 4) StartCoroutine(CheckPassword());
        break;
      
      case ScreenStates.Desktop: case ScreenStates.Folder: case ScreenStates.Button: break;
      
      case ScreenStates.TakeALook:
        if (takeALookCodeInputCnt >= 6) break;
        switch (key)
        { // code: WAKE UP
          case KeyCode.A: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[0];
            if(takeALookCodeInputCnt != 1) isTakeALookCodeCorrect = false;
            takeALookCodeInputCnt++; break;
          
          case KeyCode.B: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[1];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.C: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[2];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.D: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[3];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.E: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[4];
            if(takeALookCodeInputCnt != 3) isTakeALookCodeCorrect = false;
            takeALookCodeInputCnt++; break;
          
          case KeyCode.F: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[5];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.G: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[6];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.H: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[7];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.I: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[8];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.J: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[9];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.K: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[10];
            if(takeALookCodeInputCnt != 2) isTakeALookCodeCorrect = false;
            takeALookCodeInputCnt++; break;
          
          case KeyCode.L: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[11];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.M: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[12];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.N: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[13];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.O: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[14];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.P: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[15];
            if(takeALookCodeInputCnt != 5) isTakeALookCodeCorrect = false;
            takeALookCodeInputCnt++; break;
          
          case KeyCode.Q: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[16];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.R: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[17];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.S: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[18];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.T: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[19];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.U: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[20];
            if(takeALookCodeInputCnt != 4) isTakeALookCodeCorrect = false;
            takeALookCodeInputCnt++; break;
          
          case KeyCode.V: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[21];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.W: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[22];
            if(takeALookCodeInputCnt != 0) isTakeALookCodeCorrect = false;
            takeALookCodeInputCnt++; break;
          
          case KeyCode.X: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[23];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.Y: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[24];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          case KeyCode.Z: takeALookCodeImages[takeALookCodeInputCnt].sprite = letterSprites[25];
            isTakeALookCodeCorrect = false; takeALookCodeInputCnt++; break;
          
          default: break;
        }
        if (takeALookCodeInputCnt == 6) StartCoroutine(CheckTakeALookCode());
        break;
        
      default: Debug.LogError("Unknown screen state"); break;
    }
  }
  
  private IEnumerator CheckPassword()
  {
    yield return new WaitForSeconds(waitBeforeCheck);
    if (isPasswordCorrect)
    { 
      GameManager.PlaySfx(12);
      screenState = ScreenStates.Desktop;
      lockedScreen.SetActive(false);
      desktopScreen.SetActive(true);
      roomManager.PasswordCleared();
    }
    else if (isEnd1Password)
    {
      GameManager.PlaySfx(13);
      wrongScreen.SetActive(true);
      lockedScreen.SetActive(false);
      StartCoroutine(roomManager.EndingFound(1));
    }
    else
    {
      GameManager.PlaySfx(13);
      foreach (var image in passwordImages) image.sprite = symbolSprites[0];
      isPasswordCorrect = true;
      passwordInputCnt = 0;
    }
  }

  private IEnumerator CheckTakeALookCode()
  {
    yield return new WaitForSeconds(waitBeforeCheck);
    if (isTakeALookCodeCorrect)
    {
      GameManager.PlaySfx(12);
      roomManager.TakeALookCodeCleared();
      StartCoroutine(ShowRedEye());
    }
    else
    {
      GameManager.PlaySfx(13);
      foreach (var image in takeALookCodeImages) image.sprite = symbolSprites[0];
      isTakeALookCodeCorrect = true;
      takeALookCodeInputCnt = 0;
    }
  }

  public void FolderIconClicked()
  {
    GameManager.PlaySfx(5);
    screenState = ScreenStates.Folder;
    desktopScreen.SetActive(false);
    folderScreen.SetActive(true);
  }

  public void CloseFolderIconClicked()
  {
    GameManager.PlaySfx(5);
    screenState = ScreenStates.Desktop;
    desktopScreen.SetActive(true);
    folderScreen.SetActive(false);
  }

  public void ButtonIconClicked()
  {
    GameManager.PlaySfx(5);
    screenState = ScreenStates.Button;
    foreach (var image in buttonCodeImages) image.sprite = symbolSprites[0];
    isButtonCodeCorrect = true;
    buttonCodeInputCnt = 0;
    folderScreen.SetActive(false);
    buttonScreen.SetActive(true);
  }

  public void CloseButtonIconClicked()
  {
    GameManager.PlaySfx(5);
    screenState = ScreenStates.Folder;
    folderScreen.SetActive(true);
    buttonScreen.SetActive(false);
  }

  public void PinkButtonClicked()
  {
    GameManager.PlaySfx(5);
    StartCoroutine(ColorButtonClicked(0));
  }

  public void BlueButtonClicked()
  {
    GameManager.PlaySfx(5);
    StartCoroutine(ColorButtonClicked(1));
  }
  
  private IEnumerator ColorButtonClicked(int colorButtonIdx)
  {
    if (buttonCodeInputCnt >= 5 || isClickingColorButton[colorButtonIdx]) yield break;
    isClickingColorButton[colorButtonIdx] = true;
    colorButtonImages[colorButtonIdx].sprite = pressedColorButtonSprites[colorButtonIdx];
    switch (colorButtonIdx)
    {
      case 0:
        buttonCodeImages[buttonCodeInputCnt].sprite = letterSprites[15];
        if (buttonCodeInputCnt != 1 && buttonCodeInputCnt != 4) isButtonCodeCorrect = false;
        break;
      case 1:
        buttonCodeImages[buttonCodeInputCnt].sprite = letterSprites[1];
        if (buttonCodeInputCnt != 0 && buttonCodeInputCnt != 2 && buttonCodeInputCnt != 3) isButtonCodeCorrect = false;
        break;
    }
    buttonCodeInputCnt++;
    if (buttonCodeInputCnt == 5) StartCoroutine(CheckButtonCode());
    yield return new WaitForSeconds(buttonPressedDuration);
    colorButtonImages[colorButtonIdx].sprite = normalColorButtonSprites[colorButtonIdx];
    isClickingColorButton[colorButtonIdx] = false;
  }
  

  private IEnumerator CheckButtonCode()
  {
    yield return new WaitForSeconds(waitBeforeCheck);
    if (isButtonCodeCorrect)
    {
      GameManager.PlaySfx(12);
      screenState = ScreenStates.Desktop;
      buttonScreen.SetActive(false);
      folderScreen.SetActive(true);
      roomManager.ButtonCodeCleared();
    }
    else
    {
      GameManager.PlaySfx(13);
      foreach (var image in buttonCodeImages) image.sprite = symbolSprites[0];
      isButtonCodeCorrect = true;
      buttonCodeInputCnt = 0;
    }
  }

  private IEnumerator ShowRedEye()
  {
    TogglePower();
    var redEyeImage = redEye.GetComponent<Image>();
    redEyeImage.color = new Color(1f, 1f, 1f, 0f);
    redEye.SetActive(true);
    var elapsedTime = 0f;
    
    while (elapsedTime < redEyeFadeDuration)
    {
      redEyeImage.color = new Color(1f, 1f, 1f, elapsedTime);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
    redEyeImage.color = new Color(1f, 1f, 1f, 1f);
  }

  public void TakeALookIconClicked()
  {
    GameManager.PlaySfx(5);
    screenState = ScreenStates.TakeALook;
    foreach (var image in takeALookCodeImages) image.sprite = symbolSprites[0];
    isTakeALookCodeCorrect = true;
    takeALookCodeInputCnt = 0;
    folderScreen.SetActive(false);
    takeALookScreen.SetActive(true);
  }
  public void CloseTakeALookIconClicked()
  {
    GameManager.PlaySfx(5);
    screenState = ScreenStates.Folder;
    folderScreen.SetActive(true);
    takeALookScreen.SetActive(false);
  }
}
