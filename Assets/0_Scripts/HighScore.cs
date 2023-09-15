using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScore : MonoBehaviour
{
    public int levelNumber;

    [SerializeField] private bool displayScore;
    [SerializeField] private bool displayTime;
    [SerializeField] private bool displayHighScore;
    [SerializeField] private bool levelPlay;
    [SerializeField] private bool levelEnd;
    [SerializeField] private bool levelStart;
    [SerializeField] private bool levelFailure;
    
    public GameObject endGameUI;
    
    public Transform scoreObj;
    public Transform timeObj;
    public Transform highScoreObj;
    private float _score;
    private float _time;
    private TMP_Text _scoreTxt;
    private TMP_Text _timeTxt;
    private TMP_Text _highScoreTxt;
   

    void Start()
    {
        levelStart = true;
        displayTime = true;
        displayScore = false;
        displayHighScore = false;
        
        _scoreTxt = scoreObj.GetComponent<TMP_Text>();
        _timeTxt = timeObj.GetComponent<TMP_Text>();
        _highScoreTxt = highScoreObj.GetComponent<TMP_Text>();
        _highScoreTxt.text = "Highscore = " + PlayerPrefs.GetFloat("Highscore" + levelNumber.ToString(), 0).ToString("F2");

        scoreObj.gameObject.SetActive(false);
        timeObj.gameObject.SetActive(false);
        highScoreObj.gameObject.SetActive(false);

        if (PlayerPrefs.GetFloat("Highscore" + levelNumber.ToString(), 0) <= 0){ PlayerPrefs.SetFloat("Highscore" + levelNumber.ToString(), 1000000000f);}
    }

    public void GameEnded(bool hasWon) {
        levelEnd = true;
        levelFailure = !hasWon;
    }

    void Update()
    {
        if (levelStart == true) { 
            _score = 0;
            _time = 0;
            levelPlay = true;
            levelStart = false;
            levelEnd = false;
            _scoreTxt.text = "Score = ";
        }

        if (levelEnd == true)
        {
            endGameUI.SetActive(true);
            displayTime = false;
            displayScore = true;
            displayHighScore = true;
            
            levelPlay = false;
            if (levelFailure == true) { _score = 1000000000f; }
            else { _score = _time; }
            _scoreTxt.text = "Score = " + _score.ToString("F2");
            if (_score < PlayerPrefs.GetFloat("Highscore" + levelNumber.ToString(), 0))
            {
                PlayerPrefs.SetFloat("Highscore" + levelNumber.ToString(), _score);
                _highScoreTxt.text = "Highscore = " + _score.ToString();
            }
        }
        else if (levelPlay == true) {
            _time += Time.deltaTime;
            _timeTxt.text = "Time = " + _time.ToString("F2");
            _highScoreTxt.text = "Highscore = " + PlayerPrefs.GetFloat("Highscore" + levelNumber.ToString(), 0).ToString("F2");
        }
      
        scoreObj.gameObject.SetActive(displayScore);
        timeObj.gameObject.SetActive(displayTime);
        highScoreObj.gameObject.SetActive(displayHighScore);
    }

    public void ToNextScene() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
