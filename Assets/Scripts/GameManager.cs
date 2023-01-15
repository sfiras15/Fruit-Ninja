using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    // TextMeshProUGUI component that displays the current score
    public TextMeshProUGUI scoreText;
    // A reference to the Blade game object
    private Blade blade;
    // A reference to the Spawner game object
    private Spawner spawner;
    // Image component used for the fading effect during the game over sequence
    public Image fadeImage;

    private void Awake()
    {
        blade = FindObjectOfType<Blade>();  
        spawner = FindObjectOfType<Spawner>();
    }
    private void Start()
    {
        NewGame();
    }
    private void NewGame()
    {
        // Set the time scale to normal
        Time.timeScale = 1f;
        // Enable the blade and spawner scripts
        blade.enabled = true;
        spawner.enabled = true;
        score = 0;
        scoreText.text = score.ToString();
        // Clear the scene of any remaining fruits or bombs
        ClearScene();
    }
    // Clears the scene of any remaining fruits or bombs
    private void ClearScene()
    {
        // Find all fruits in the scene
        Fruit[] fruits = FindObjectsOfType<Fruit>();
        foreach(Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }
        // Find all bombs in the scene
        Bomb[] bombs = FindObjectsOfType<Bomb>();
        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }
    // Increments the score by the specified amount
    public void IncreaseScore(int fruitScore)
    {
        score += fruitScore;
        scoreText.text = score.ToString();
    }
    // Called when the player hits a bomb
    public void GameOver()
    {
        // Disable the blade and spawner scripts
        blade.enabled = false;
        spawner.enabled = false;
        // Start the game over sequence
        StartCoroutine(nameof(ExplodeSequence));
    }
    private IEnumerator ExplodeSequence()
    {
        // Duration of the fade effect
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            // Calculate the interpolation value
            float t = Mathf.Clamp(elapsed / duration,0f,1f);
            // Interpolate the fade image's color
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);
            // Slow down time as the fade effect progresses
            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        elapsed = 0f;

        // Wait for 1 second before starting a new game
        yield return new WaitForSecondsRealtime(1f);
        NewGame();
        // Fade the image back to clear
        while (elapsed < duration)
        {
            float t = Mathf.Clamp(elapsed / duration, 0f, 1f);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }
     
}
