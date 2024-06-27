using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager: MonoBehaviour
{
    [SerializeField] private TimerImage _timerImageHarvest, _timerImagePeasant, 
                                        _timerImageWarrior, _timerImageEating, 
                                        _timerImageRaid; //таймеры
    
    [SerializeField] private Button _buttonBuyPeasant,_buttonBuyWarrior; //кнопки найма
    
    [SerializeField] private GameObject panelLose,panelWin;//панели проигрыша и выигрыша
    
    [SerializeField] private TextMeshProUGUI peasant,warrior,
                                             harvest,wave,enemy, 
                                             attackAfter;//текстовые обозначения для закладывания в них int переменных

    [SerializeField] private AudioSource _soundGame, _soundEat, _soundHarvest,
                                         _soundWarrior, _soundPeasant, _soundAttack, //звуки
                                         _soundWin, _soundLose, _soundMenu,
                                         _soundEasterEgg,_soundEasterEggClick;
    
    private int peasantQuantity = 1, warriorQuantity,
                harvestQuantity = 10, waveQuantity, //int переменные которые объявятся на экране ( через текстовые обозначения)
                enemyQuantity, raidIncrease = 2,
                attackAfterWave=3;
    
    private int harvestPerPeasant = 3, //сколько добывет крестьянин
                harvestToWarrior = 5; //сколько ест воин
    
    private int winWarrior = 50; //условия выигрыша
    private int winharvest = 500;
    private int winWave = 11;

    private int peasentCost = 3; //сколько стоят крестьянин и воин
    private int warriorCost = 6;

    public void Start()
    {
        _soundMenu.Play();
        ResetAllTimer();
        StopAllTimer();
    }
    private void Update()
    {
        RaiderIncrease();
        CheckConditionWarrior();
        CheckConditionPeasant();
        CheckHarvest();
        CheckEating();
        DataPressButton();
        LimitUI();
        UpdateUI();
    }

    /// <summary>
    /// Панели проигрыша\выигрыша
    /// </summary>
    public void PanelLose() //метод вызова панели проигрыша
    {
        if (warriorQuantity<=-1)
        {
            panelLose.SetActive(true); //вызвать панель проигрыша
            _soundLose.Play();
            StopAllTimer();
        }
    }
    public void PanelWin() //метод вызова панели победы
    {
        if (warriorQuantity >= winWarrior || harvestQuantity >= winharvest || waveQuantity == winWave)
        {
            panelWin.SetActive(true); // вызвать панель победы
            _soundWin.Play(); 
            StopAllTimer();
        }
    }
    

    /// <summary>
    /// кнопки
    /// </summary>
    public void ButtonBuyPeasant() //Покупка крестьянина
    {
        _buttonBuyPeasant.interactable = false;
        harvestQuantity -= peasentCost;
        _timerImagePeasant.StartTimer();
    }
    public void ButtonBuyWarrior() //Покупка воина
    {
        _buttonBuyWarrior.interactable = false;
        harvestQuantity -= warriorCost;
        _timerImageWarrior.StartTimer();
    }
    public void OnPlayButtonClick() //метод кнопки New Game
    {
        _soundMenu.Stop();
        _soundGame.Play();
        ResetAllTimer();
        StartAlmostAllTimer();
    }
    
    /// <summary>
    /// Сброс всех значений
    /// </summary>
    public void Restart()
    {
        OnPlayButtonClick();
        panelLose.SetActive(false);
        panelWin.SetActive(false);
        peasantQuantity = 1;
        harvestQuantity = 0;
        harvestQuantity = 10;
        waveQuantity = 0;
        enemyQuantity =0;
        UpdateUI();
    }

    /// <summary>
    /// проверка состояния кнопок
    /// </summary>
    private void CheckConditionWarrior()
    {
        if(_timerImageWarrior.currentTime<=0)
        {
            _timerImageWarrior.ResetTimer();
            _timerImageWarrior.StopTimer();
            _buttonBuyWarrior.interactable = true;
            _soundWarrior.Play();
            warriorQuantity++;
            PanelWin();
        }
    }
    private void CheckConditionPeasant()
    {
        if (_timerImagePeasant.currentTime <= 0)
        {
            _timerImagePeasant.ResetTimer();
            _timerImagePeasant.StopTimer();
            _buttonBuyPeasant.interactable = true;
            _soundPeasant.Play();
            peasantQuantity ++;
        }
    }

    private void DataPressButton() //Условия для нажатия кнопок воина и крестьянина
    {
        if (harvestQuantity < peasentCost)
        {
            _buttonBuyPeasant.interactable = false;
        }

        if (harvestQuantity < warriorCost)
        {
            _buttonBuyWarrior.interactable = false;
        }
    }    
    
    /// <summary>
    /// отображение данных на экране
    /// </summary>
    private void UpdateUI() // Метод обявления значений на экране
    {
        peasant.text = peasantQuantity.ToString();
        warrior.text = warriorQuantity.ToString();
        harvest.text = harvestQuantity.ToString();
        enemy.text = enemyQuantity.ToString();
        wave.text = waveQuantity.ToString();
        attackAfter.text = attackAfterWave.ToString();
    }
    private void LimitUI() //Установка Лимита данных 
    {
        int valueMin = 0;
        int valueMax = 1000;
        peasantQuantity = Mathf.Clamp(peasantQuantity, valueMin, valueMax);
        warriorQuantity = Mathf.Clamp(warriorQuantity, valueMin, valueMax);
        harvestQuantity = Mathf.Clamp(harvestQuantity, valueMin, valueMax);
        enemyQuantity = Mathf.Clamp(enemyQuantity, valueMin, valueMax);
        attackAfterWave = Mathf.Clamp(attackAfterWave, valueMin, valueMax);
    }

    /// <summary>
    /// Таймеры, запускающиеся при старте
    /// </summary>
    private void RaiderIncrease() //Условия для таймера набега врагов
    {
        if (_timerImageRaid.currentTime <=0)// при времени больш или равном нулю. почти ноль
        { 
            _timerImageRaid.ResetTimer();
            waveQuantity ++;
            attackAfterWave -= 1;
            PanelWin();
            if (waveQuantity >= 3)
            {
                warriorQuantity -= enemyQuantity; //
                PanelLose();
                enemyQuantity += raidIncrease; // в переменную противников плюсуется новое значение для следующей волн
                if (waveQuantity >= 4)
                {
                    _soundAttack.Play();
                }
            }
        }
    }
    private void CheckHarvest() //Получение и трата пшеницы по таймеру 
    {
        if (_timerImageHarvest.currentTime <= 0)
        {
            _timerImageHarvest.ResetTimer();
            harvestQuantity += peasantQuantity * harvestPerPeasant;
            _soundHarvest.Play();
            Debug.Log($"пшена стало {harvestQuantity}");
            PanelWin();
        }
    }
    private void CheckEating()
    {
        if (_timerImageEating.currentTime <= 0)
        {
            _timerImageEating.ResetTimer();
            harvestQuantity -= warriorQuantity * harvestToWarrior;
            _soundEat.Play();
            Debug.Log($"пшена сожрали {warriorQuantity * harvestToWarrior},теперь пшена {harvestQuantity}");
        }
    }
    
    /// <summary>
    /// методы по работе с таймерами
    /// </summary>
    private void StopAllTimer()
    {
        _timerImageRaid.StopTimer();
        _timerImageEating.StopTimer();
        _timerImageHarvest.StopTimer();
        _timerImagePeasant.StopTimer();
        _timerImageWarrior.StopTimer();
    }
    private void ResetAllTimer()
    {
        _timerImagePeasant.ResetTimer();
        _timerImageWarrior.ResetTimer();
        _timerImageEating.ResetTimer();
        _timerImageRaid.ResetTimer();
        _timerImageHarvest.ResetTimer();
    }
    private void StartAlmostAllTimer()
    {
        _timerImageRaid.StartTimer();
        _timerImageEating.StartTimer();
        _timerImageHarvest.StartTimer();
    }

}
