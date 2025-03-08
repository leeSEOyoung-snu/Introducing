using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TakeALookBehaviour : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private RoomManager roomManager;
  
  [Header("Hearts")]
  [SerializeField] private GameObject heart;
  [SerializeField] private Image heartImage;
  [SerializeField] private Sprite[] heartSprites;
  [SerializeField] private int heartRate;
  [SerializeField] private bool isHeartActive;
  [SerializeField] private float showHeartDuration;
  
  [Header("TakeALook")]
  [SerializeField] private Image bodyImage;
  [SerializeField] private Image tailImage;
  [SerializeField] private Sprite readBodySprite;
  [SerializeField] private Sprite tailDownSprite;
  [SerializeField] private Animator bodyAnimator, tailAnimator;

  public void InitTakeALook()
  {
    heart.SetActive(false);
    
    // Animator
    bodyAnimator.enabled = true;
    tailAnimator.enabled = true;
  }

  public void TakeALookHeart()
  {
    if (isHeartActive) return;
    StartCoroutine(ShowHeart());
  }

  private IEnumerator ShowHeart()
  {
    GameManager.PlaySfx(0);
    isHeartActive = true;
    heartImage.sprite = heartSprites[heartRate];
    heart.SetActive(true);
    heartRate++;
    if (heartRate == 4)
    {
      heartRate = 3;
      StartCoroutine(roomManager.EasterEggFound(1));
    }
    yield return new WaitForSeconds(showHeartDuration);
    heart.SetActive(false);
    isHeartActive = false;
  }

  public void TakeALookPowerOff()
  {
    bodyAnimator.enabled = false;
    tailAnimator.enabled = false;
    bodyImage.sprite = readBodySprite;
    tailImage.sprite = tailDownSprite;
  }
}
