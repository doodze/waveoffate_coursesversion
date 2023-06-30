using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _joystick;
    [SerializeField] private List<GameObject> _inGameUIElements;
    [SerializeField] private Slider _progressSlider;    
    [SerializeField] private float _progressStartValue = 500f;    
    [SerializeField] private float _progressStep = 500f;
    [SerializeField] private GameObject _mainScreen;
    //[SerializeField] private Button _startButton;
    [SerializeField] private int _weaponsCount;
    [SerializeField] private List<WeaponUIContent> _weaponsContent;
    [SerializeField] private List<WeaponUpgradeItem> _weaponUpgradeItems;
    [SerializeField] private List<SelectedLevelItem> _selectedLevelItems;
    [SerializeField] private GameObject _upgradesPanel;      
    [SerializeField] private TextMeshProUGUI _killedEnemiesCounter;
    [SerializeField] private GameObject _scoreWindow;
    [SerializeField] private TextMeshProUGUI _scoreValue;
    [SerializeField] private TextMeshProUGUI _enemiesKilledScore;    

    private int _enemyCounter = 0;    

    public event Action LevelStarted;
    public event Action LevelIncreased;
    public event Action<LevelType> LevelSelected;
    public event Action<WeaponType> WeaponUpgraded;
    public event Action GamePaused;
    public event Action GameUnpaused;
    public int WeaponsCount => _weaponsCount;    

    private void Awake()
    {      
        foreach (var item in _weaponUpgradeItems)
        {
            item.WeaponUpgraded += OnWeaponUpgraded;
        }
        foreach (var item in _selectedLevelItems)
        {
            item.LevelSelected += OnLevelSelected;
        }        
    }

    private void Start()
    {        
       // _startButton.onClick.AddListener(OnLevelStarted);
        _progressSlider.maxValue = _progressStartValue;
        _enemiesKilledScore.text = PlayerPrefs.GetInt("score", 0).ToString();
    }

    public void UpdateKilledEnemiesCounter()
    {
        _enemyCounter++;
        _killedEnemiesCounter.text = _enemyCounter.ToString();
    }

    public void UpdateBarValue(float value)
    {
        _progressSlider.value += value;

        if (_progressSlider.value == _progressSlider.maxValue)
        {            
            _progressSlider.maxValue += _progressStep;
            _progressSlider.value = 0;
            LevelIncreased?.Invoke();
        }        
    }   

    public void SetNotLeveledWeapons(List<WeaponLevelingData> weaponsList)
    {
        if (weaponsList.Count == 0)
        {
            if (_enemyCounter > PlayerPrefs.GetInt("score", 0))
            {
                PlayerPrefs.SetInt("score", _enemyCounter);
                PlayerPrefs.Save();
            }
            ShowScore();             
        }            

        for (int i = 0; i < weaponsList.Count; i++)
        {
            var content = _weaponsContent.Find(c => c.weaponType == weaponsList[i].weaponType);
            _weaponUpgradeItems[i].gameObject.SetActive(true);
            _weaponUpgradeItems[i].ShowUpgrade(content, weaponsList[i]);
        }       

        _joystick.SetActive(false);
        _upgradesPanel.SetActive(true);

        Pause();
    }    

    public void ChangeToGameUI()
    {
        _mainScreen.SetActive(false);

        foreach (var element in _inGameUIElements)
        {
            element.SetActive(true);
        }
    }    

    private void OnLevelStarted()
    {       
        LevelStarted?.Invoke();
    }

    private void OnLevelSelected(LevelType level)
    {
        OnLevelStarted();
        LevelSelected?.Invoke(level);
    }

    private void OnWeaponUpgraded(WeaponType weapon)
    {
        WeaponUpgraded?.Invoke(weapon);
        
        foreach (var item in _weaponUpgradeItems)
        {
            item.gameObject.SetActive(false);
        }

        _joystick.SetActive(true);
        _upgradesPanel.SetActive(false);        

        Play();
    }

    public void OpenWindow(GameObject window)
    {
        window.SetActive(true);
    }
    public void CloseWindow(GameObject window)
    {
        window.SetActive(false);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void RealoadScene()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Pause()
    {
        GamePaused?.Invoke();
        Time.timeScale = 0;
    }
    public void Play()
    {        
        Time.timeScale = 1;
        GameUnpaused?.Invoke();
    }

    public void TimeScaleOne()
    {
        Time.timeScale = 1;
    }

    public void ShowScore()
    {        
        Pause();
        _scoreWindow.SetActive(true);
        _scoreValue.text = _enemyCounter.ToString();
    }
}
