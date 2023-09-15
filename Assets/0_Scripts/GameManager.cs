using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   [SerializeField] private string mainMenuName = "MainMenu";
   [SerializeField] private GameObject pausePanel;

   private bool _isGamePaused;

   private void Awake() => Instance = this;
   public void ReturnToMenu()
   {
      _isGamePaused = false;
      Time.timeScale = 1;
      SceneManager.LoadScene(mainMenuName);
   }
   public void Quit() => Application.Quit();
   public void ChangePauseState()
   {
      _isGamePaused = !_isGamePaused;
      Time.timeScale = _isGamePaused ? 0 : 1;
      pausePanel.SetActive(_isGamePaused);
   }
}
