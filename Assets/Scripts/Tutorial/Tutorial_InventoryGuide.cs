using UnityEngine;

public class Tutorial_InventoryGuide : MonoBehaviour
{
    // Tutorial Pages
    public GameObject panel;
    public GameObject[] tutorialPages;
    public bool canToggle = false;
    private bool tutorialToggled = false;
    private int currentPageIndex = 0;

    private void Update()
    {
        if (canToggle && Input.GetKeyDown(KeyCode.Tab) && !tutorialToggled)
        {
            panel.SetActive(true);
            PlayerInputController.Instance.canInput = false;
            ShowTutorial();
        }

        if (tutorialToggled && Input.GetKeyDown(KeyCode.Mouse0))
        {
            NextPage();
        }
    }

    public void ShowTutorial()
    {
        tutorialToggled = true;
        currentPageIndex = 0;
        UpdateTutorialPages();
    }

    public void NextPage()
    {
        if (currentPageIndex < tutorialPages.Length - 1)
        {
            currentPageIndex++;
            UpdateTutorialPages();
        }
        else
        {
            HideTutorial();
            PlayerInputController.Instance.canInput = true;
        }
    }

    public void UpdateTutorialPages()
    {
        for (int i = 0; i < tutorialPages.Length; i++)
        {
            tutorialPages[i].SetActive(i == currentPageIndex);
        }
    }

    public void HideTutorial()
    {
        foreach (GameObject page in tutorialPages)
        {
            page.SetActive(false);
        }
        panel.SetActive(false);
    }
}
