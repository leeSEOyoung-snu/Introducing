using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private RoomManager roomManager;
  
  [Header("Window")]
  [SerializeField] private Image frameImage;
  [SerializeField] private Sprite closedFrameSprite, openedFrameSprite;
  [SerializeField] private GameObject closedFrameButton, openedFrameButton;
  [SerializeField] private bool isWindowOpened;

  [Header("Pot")]
  [SerializeField] private Image potImage;
  [SerializeField] private Sprite potLeafSprite, potFlowerSprite, potDeadSprite;
  [SerializeField] private GameObject wateringCan;
  [SerializeField] private float wateringDuration;
  private Vector2 _wateringRectPos = new Vector2(-60, 140);
  private Vector3 _wateringRectRotation = new Vector3(0, 0, 30);

  [Header("Draws")]
  [SerializeField] private Image[] drawImages;
  [SerializeField] private Sprite[] drawClosedSprites;
  [SerializeField] private Sprite[] drawOpenedSprites;
  [SerializeField] private GameObject[] openDrawButtons;
  [SerializeField] private GameObject[] openedDrawParents;
  [SerializeField] private bool[] isDrawOpened = new bool[3];
  [SerializeField] private bool[] isDrawLocked = new bool[3];
  
  [Header("Scissors")]
  [SerializeField] private float cutPlantDuration;
  [SerializeField] private GameObject drawScissors, potScissors;
  
  [Header("Bookshelf")]
  [SerializeField] private GameObject bookshelfLeftDoor;
  [SerializeField] private GameObject bookshelfRightDoor;
  [SerializeField] private bool isBookshelfLeftDoorOpened, isBookshelfRightDoorLocked;
  [SerializeField] private GameObject leftKey;
  [SerializeField] private GameObject rightKey;
  [SerializeField] private float openBookshelfRightDoorDuration;
  
  [Header("Morse Memo")]
  [SerializeField] private GameObject morseMemoSmall;
  [SerializeField] private GameObject morseMemoFull;
  [SerializeField] private bool isMorseMemoOpened;
  
  
  public void InitButtons()
  {
    // Window
    frameImage.sprite = closedFrameSprite;
    closedFrameButton.SetActive(true);
    openedFrameButton.SetActive(false);
    isWindowOpened = false;
    
    // Pot
    potImage.sprite = potLeafSprite;
    
    // Draws
    for (int i = 0; i < drawImages.Length; i++)
    {
      drawImages[i].sprite = drawClosedSprites[i];
      openDrawButtons[i].SetActive(true);
      openedDrawParents[i].SetActive(false);
      isDrawOpened[i] = false;
      if (i == 0) isDrawLocked[i] = false;
      else isDrawLocked[i] = true;
    }
    
    // Scissors
    potScissors.SetActive(false);
    
    // Bookshelf
    bookshelfLeftDoor.SetActive(true);
    bookshelfRightDoor.SetActive(true);
    isBookshelfLeftDoorOpened = false;
    isBookshelfRightDoorLocked = true;
    leftKey.SetActive(true);
    rightKey.SetActive(false);
    
    // MorseMemo
    morseMemoSmall.SetActive(true);
    morseMemoFull.SetActive(false);
    isMorseMemoOpened = false;
  }

  public void WindowClicked()
  {
    GameManager.PlaySfx(7);
    if (isWindowOpened)
    {
      // opened => closed
      frameImage.sprite = closedFrameSprite;
      closedFrameButton.SetActive(true);
      openedFrameButton.SetActive(false);
      isWindowOpened = false;
    }
    else
    {
      // closed => opened
      frameImage.sprite = openedFrameSprite;
      closedFrameButton.SetActive(false);
      openedFrameButton.SetActive(true);
      isWindowOpened = true;
    }
  }

  public void WateringCanClicked()
  {
    StartCoroutine(DoWatering());
    StartCoroutine(roomManager.EasterEggFound(2));
  }

  private IEnumerator DoWatering()
  {
    var wateringCanRect = wateringCan.GetComponent<RectTransform>();
    var wateringCanStartPos = wateringCanRect.anchoredPosition;
    var wateringCanStartRotation = wateringCanRect.rotation;
    wateringCanRect.anchoredPosition = _wateringRectPos;
    wateringCanRect.rotation = Quaternion.Euler(_wateringRectRotation);
    GameManager.PlaySfx(8);
    yield return new WaitForSeconds(wateringDuration);
    potImage.sprite = potFlowerSprite;
    wateringCanRect.anchoredPosition = wateringCanStartPos;
    wateringCanRect.rotation = wateringCanStartRotation;
  }
  
  public void UnlockDraw(int drawIdx)
  {
    isDrawLocked[drawIdx] = false;
    DrawClicked(drawIdx);
  }

  public void Draw0ButtonClicked() { DrawClicked(0); }
  public void Draw1ButtonClicked() { DrawClicked(1); }
  public void Draw2ButtonClicked() { DrawClicked(2); }

  private void DrawClicked(int drawIdx)
  {
    if (isDrawOpened[drawIdx])
    { // opened -> closed
      GameManager.PlaySfx(1);
      drawImages[drawIdx].sprite = drawClosedSprites[drawIdx];
      openDrawButtons[drawIdx].SetActive(true);
      openedDrawParents[drawIdx].SetActive(false);
      isDrawOpened[drawIdx] = false;
    }
    else
    { // closed -> opened
      if (isDrawLocked[drawIdx])
      {
        GameManager.PlaySfx(2);
        return;
      }
      GameManager.PlaySfx(1);
      drawImages[drawIdx].sprite = drawOpenedSprites[drawIdx];
      openDrawButtons[drawIdx].SetActive(false);
      openedDrawParents[drawIdx].SetActive(true);
      isDrawOpened[drawIdx] = true;
    }
  }

  public void ScissorsClicked()
  {
    GameManager.PlaySfx(10);
    StartCoroutine(CutPlant());
    StartCoroutine(roomManager.EndingFound(2));
  }
  private IEnumerator CutPlant()
  {
    potScissors.SetActive(true);
    drawScissors.SetActive(false);
    GameManager.PlaySfx(9);
    yield return new WaitForSeconds(cutPlantDuration);
    potImage.sprite = potDeadSprite;
    potScissors.SetActive(false);
    drawScissors.SetActive(true);
  }

  public void BookshelfLeftDoorClicked()
  {
    GameManager.PlaySfx(1);
    isBookshelfLeftDoorOpened = true;
    bookshelfLeftDoor.SetActive(false);
  }
  
  public void BookshelfRightDoorClicked()
  {
    GameManager.PlaySfx(2);
  }

  public void KeyClicked()
  {
    StartCoroutine(OpenBookshelfRightDoor());
  }

  private IEnumerator OpenBookshelfRightDoor()
  {
    GameManager.PlaySfx(3);
    isBookshelfRightDoorLocked = false;
    rightKey.SetActive(true);
    leftKey.SetActive(false);
    yield return new WaitForSeconds(openBookshelfRightDoorDuration);
    GameManager.PlaySfx(1);
    bookshelfRightDoor.SetActive(false);
  }
    
  public void MorseMemoSmallClicked()
  {
    GameManager.PlaySfx(4);
    morseMemoSmall.SetActive(false);
    morseMemoFull.SetActive(true);
    isMorseMemoOpened = true;
  }
}
