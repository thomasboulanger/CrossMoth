using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    public Transform scoreObj;
    public Transform timeObj;
    public Transform highScoreObj;
    private float _score;
    private float _time;
    private TextMeshPro _scoreTxt;
    private TextMeshPro _timeTxt;
    private TextMeshPro _highScoreTxt;

    //private bool _levelStart;
    //private bool _levelEnd;
    //private bool _levelFailure;

    // Start is called before the first frame update
    void Start()
    {
        _scoreTxt = scoreObj.GetComponent<TextMeshPro>();
        _timeTxt = timeObj.GetComponent<TextMeshPro>();
        _highScoreTxt = highScoreObj.GetComponent<TextMeshPro>();
        _highScoreTxt.text = "Highscore = " + PlayerPrefs.GetFloat("Highscore" + levelNumber.ToString(), 0).ToString();

        scoreObj.gameObject.SetActive(false);
        timeObj.gameObject.SetActive(false);
        highScoreObj.gameObject.SetActive(false);

        if (PlayerPrefs.GetFloat("Highscore" + levelNumber.ToString(), 0) <= 0){ PlayerPrefs.SetFloat("Highscore" + levelNumber.ToString(), 1000000000f);}
    }

    // Update is called once per frame
    void Update()
    {
        //A faire :
        //Loqique pour savoir quand displayScore == true/false //
        //Loqique pour savoir quand displayTime == true/false //
        //Loqique pour savoir quand displayHighScore == true/false //
        //Loqique pour savoir quand levelPlay == true/false //
        //Loqique pour savoir quand levelEnd == true/false //
        //Loqique pour savoir quand levelStart == true/false //
        //Loqique pour savoir quand levelFailure == true/false //
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
            levelPlay = false;
            if (levelFailure == true) { _score = 1000000000f; }
            else { _score = _time; }
            _scoreTxt.text = "Score = " + _score;
            if (_score < PlayerPrefs.GetFloat("Highscore" + levelNumber.ToString(), 0))
            {
                PlayerPrefs.SetFloat("Highscore" + levelNumber.ToString(), _score);
                _highScoreTxt.text = "Highscore = " + _score.ToString();
            }
        }
        else if (levelPlay == true) {
            _time += Time.deltaTime;
            _timeTxt.text = "Time = " + _time;
            _highScoreTxt.text = "Highscore = " + PlayerPrefs.GetFloat("Highscore" + levelNumber.ToString(), 0).ToString();
        }
      

        scoreObj.gameObject.SetActive(displayScore);
        timeObj.gameObject.SetActive(displayTime);
        highScoreObj.gameObject.SetActive(displayHighScore);
    }
}
