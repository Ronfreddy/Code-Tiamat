using TMPro;
using UnityEngine;

public class GameOverResult : MonoBehaviour
{
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI enemyKilledText;

    private void OnEnable()
    {
        floorText.text = "Floor: B" + LevelManager.instance.currentLevel.ToString();
        difficultyText.text = "Difficulty: <color=red>" + LevelManager.instance.currentDifficulty.ToString() + "</color>";
        enemyKilledText.text = "Enemies Killed: " + LevelManager.instance.enemyKilledCount.ToString();
    }
}
