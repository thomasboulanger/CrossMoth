using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   [SerializeField] private GameObject pausePanel;

   private bool _isGamePaused;

   private void Awake() => Instance = this;
   public void ChangePauseState()
   {
      _isGamePaused = !_isGamePaused;
      Time.timeScale = _isGamePaused ? 0 : 1;
      pausePanel.SetActive(_isGamePaused);
   }
}
