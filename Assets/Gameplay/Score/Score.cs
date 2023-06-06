using TMPro;
using UnityEngine;
using VavilichevGD.Utils.Timing;
using Zenject;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Indicator boostIndicatorPrefab;

    private Indicator _boostIndicator;
    private Player _player;
    private PlayerBoosts _playerBoosts;
    private SyncedTimer _timer;
    private BoostIndicators _boostIndicators;

    private int _score;

    private int _multiplier = 1;

    [Inject]
    private void Construct(Player player, BoostIndicators boostIndicators)
    {
        _player = player;
        _playerBoosts = player.GetComponent<PlayerBoosts>();
        _boostIndicators = boostIndicators;
    }

    private void Awake()
    {
        _timer = new SyncedTimer(TimerType.UpdateTick);
    }

    private void OnEnable()
    {
        _playerBoosts.OnBoostCollectedEvent += BoostCollected;
        _timer.TimerFinished += FinishBoost;
        _timer.TimerValueChanged += BoostTick;
        _player.OnPlayerDiedEvent += PlayerDied;
    }

    private void OnDisable()
    {
        _playerBoosts.OnBoostCollectedEvent -= BoostCollected;
        _timer.TimerFinished -= FinishBoost;
        _timer.TimerValueChanged -= BoostTick;
        _player.OnPlayerDiedEvent -= PlayerDied;
    }

    private void Update()
    {
        _score += 1 * _multiplier;
        scoreText.text = _score + "";
    }

    private void BoostCollected(BoostType type)
    {
        if (type == BoostType.MultipleScore)
        {
            StartBoost();
        }
    }

    private void StartBoost()
    {
        _timer.Stop();
        
        _boostIndicator = Instantiate(boostIndicatorPrefab, _boostIndicators.transform);
        _boostIndicator.Init(BoostType.MultipleScore);
        
        _timer.Start(15);
        _multiplier = 2;
    }

    private void BoostTick(float timeLeft, TimeChangingSource source)
    {
        if (_boostIndicator != null)
            _boostIndicator.FillIndicator(timeLeft, 15);
    }

    private void FinishBoost()
    {
        Destroy(_boostIndicator.gameObject);
        
        _multiplier = 1;
    }

    private void PlayerDied()
    {
        if (int.Parse(Db.ExecuteQueryWithAnswer("SELECT Record FROM Player WHERE Id = 1")) < _score)
            Db.ExecuteQueryWithoutAnswer($"UPDATE Player SET Record = {_score} WHERE Id = 1");
        
        _timer.Stop();
        _multiplier = 0;
    }
}
