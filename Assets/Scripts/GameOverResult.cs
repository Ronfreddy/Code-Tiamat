using TMPro;
using UnityEngine;

public class GameOverResult : MonoBehaviour
{
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI enemyKilledText;
    public TextMeshProUGUI timeText;

    private void OnEnable()
    {
        floorText.text = "Floor: B" + LevelManager.instance.currentLevel.ToString();
        difficultyText.text = "Difficulty: <color=red>" + LevelManager.instance.currentDifficulty.ToString() + "</color>";
        enemyKilledText.text = "Enemies Killed: " + LevelManager.instance.enemyKilledCount.ToString();
        int survivedMinutes = (int)(GameManager.instance.timeAlive / 60);
        int survivedSeconds = (int)(GameManager.instance.timeAlive % 60);
        timeText.text = "Time Survived: " + survivedMinutes.ToString("00") + ":" + survivedSeconds.ToString("00");
    }
}
