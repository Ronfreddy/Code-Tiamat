using UnityEngine;

public class StartLevelPortal : MonoBehaviour
{
    private bool isPlayerInside = false;
    public GameObject interactHint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            interactHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            interactHint.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E) && Time.timeScale == 1)
        {
            if (GameManager.instance.hasStartedTutorial)
            {
                UIController.Instance.OpenLevelSelectPanel();
            }
            else
            {
                GameManager.instance.OnTutorialSelected();
            }
            //GameManager.instance.DestroyLobby(1.5f);
            //LevelManager.instance.StartNewLevel();
        }
    }
}
