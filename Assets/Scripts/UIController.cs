using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("HUD Box")]
    public RectTransform HUDBox;
    private Vector2 boxPositionOrigin;
    public Slider healthBar;
    public Slider staminaBar;

    [Header("Weapon Slot")]
    public CanvasGroup weaponSlot;
    public RectTransform weaponParent;
    public List<float> positions = new List<float>();

    [Header("Level Description")]
    public RectTransform levelTextBox;
    public TextMeshProUGUI levelText;

    [Header("Transition")]
    public Transition sceneTransition;

    [Header("Part Pop Up")]
    public Transform popUpParent;
    public GameObject partPopUpPrefab;
    private List<RectTransform> popUpTransformList = new List<RectTransform>();

    public GameObject levelSelectPanel;

    // Portal Navigation
    private bool canNavigate = false;
    public GameObject portalNavigation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        boxPositionOrigin = HUDBox.anchoredPosition;
    }

    private void Update()
    {
        if (canNavigate)
        {
            EnablePortalNavigation();
        }
    }

    public void UpdateHealthBar(float health)
    {
        float targetValue = health / 100f;
        healthBar.DOValue(targetValue, 0.5f).SetEase(Ease.OutQuad);

        // Shake the box
        HUDBox.anchoredPosition = boxPositionOrigin;
        HUDBox.DOShakeAnchorPos(0.2f, new Vector3(5f, 5f, 0)).SetEase(Ease.OutQuad).OnComplete(() => HUDBox.anchoredPosition = boxPositionOrigin);
    }

    public void UpdateStaminaBar(float stamina, float maxStamina)
    {
        staminaBar.value = stamina / maxStamina;
    }

    public void ChangeWeaponSlot(int index)
    {
        ShowWeaponSlot();
        weaponParent.DOLocalMoveY(positions[index], 0.15f).SetEase(Ease.OutQuad);
    }

    private void ShowWeaponSlot()
    {
        DOTween.Kill(weaponSlot);
        weaponSlot.DOFade(1f, 0.1f).OnComplete(() =>
        {
            weaponSlot.DOFade(0f, 1f).SetDelay(2f).OnComplete(() =>
            {
                weaponSlot.blocksRaycasts = false;
            });
        });
        weaponSlot.blocksRaycasts = true;
    }

    public void FadeIn()
    {
        sceneTransition.FadeIn();
        AudioManager.Instance.PlayFadeInSfx();
    }

    public void FadeOut()
    {
        sceneTransition.FadeOut();
        AudioManager.Instance.PlayFadeOutSfx();
    }

    public void ShowCurrentLevel(int level)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => { levelText.text = "B" + LevelManager.instance.currentLevel; }).
        Append(levelTextBox.DOAnchorPosY(-30, 1f).SetEase(Ease.OutQuad)).
        AppendInterval(2f).
        Append(levelTextBox.DOAnchorPosY(120, 1f).SetEase(Ease.OutQuad));

    }

    public void ShowCurrentLevel(string level)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => { levelText.text = level; }).
            Append(levelTextBox.DOAnchorPosY(-30, 1f).SetEase(Ease.OutQuad)).
            AppendInterval(2f).
            Append(levelTextBox.DOAnchorPosY(120, 1f).SetEase(Ease.OutQuad));
    }

    public void ShowPartPopUp(ModularPart part)
    {
        GameObject partPopUp = Instantiate(partPopUpPrefab, popUpParent);
        partPopUp.transform.localPosition = new Vector3(0, -100, 0);
        partPopUp.GetComponent<ItemPopUp>().UpdatePartName(part);
        MoveEveryPopUp();
        partPopUp.GetComponent<RectTransform>().DOAnchorPosY(partPopUp.transform.position.y + 100, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            DOVirtual.DelayedCall(2f, () =>
            {
                popUpTransformList.Remove(partPopUp.GetComponent<RectTransform>());
                Destroy(partPopUp);
            });
        });
        popUpTransformList.Add(partPopUp.GetComponent<RectTransform>());

    }

    public void MoveEveryPopUp()
    {
        for(int i = 0; i < popUpTransformList.Count; i++)
        {
            popUpTransformList[i].DOAnchorPosY((i + 1) * 100, 0.5f).SetEase(Ease.OutQuad);
        }
    }

    public void OpenLevelSelectPanel()
    {
        levelSelectPanel.SetActive(true);
    }

    public void EnablePortalNavigation()
    {
        canNavigate = true;
        portalNavigation.SetActive(true);
        Vector3 portalDirection = (LevelManager.instance.portal.transform.position - GameManager.instance.player.transform.position).normalized;

        portalNavigation.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(portalDirection.y, portalDirection.x) * Mathf.Rad2Deg - 90);
    }

    public void DisablePortalNavigation()
    {
        canNavigate = false;
        portalNavigation.SetActive(false);
    }
}
