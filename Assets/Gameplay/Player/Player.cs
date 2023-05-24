using System;
using UnityEngine;
using VavilichevGD.Utils.Timing;
using Zenject;

public class Player : MonoBehaviour
{
    public event Action OnPlayerDiedEvent;
    
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private PlayerBoosts playerBoosts;
    [SerializeField] private Indicator boostIndicatorPrefab;
    
    private Indicator _boostIndicator;
    private LosePanel _losePanel;
    private Bank _bank;
    private SyncedTimer _boostTimer;
    private BoostIndicators _boostIndicators;
    private bool _immortal;

    [Inject]
    private void Construct(LosePanel losePanel, Bank bank, BoostIndicators boostIndicators)
    {
        _losePanel = losePanel;
        _bank = bank;
        _boostIndicators = boostIndicators;
    }

    private void Awake()
    {
        _boostTimer = new SyncedTimer(TimerType.UpdateTick);
    }

    private void OnEnable()
    {
        playerCollision.OnPlayerColliderHitEvent += PlayerColliderHit;
        playerBoosts.OnBoostCollectedEvent += CollectBoost;
        _boostTimer.TimerFinished += FinishBoost;
        _boostTimer.TimerValueChanged += BoostTick;
    }

    private void OnDisable()
    {
        playerCollision.OnPlayerColliderHitEvent -= PlayerColliderHit;
        playerBoosts.OnBoostCollectedEvent -= CollectBoost;
        _boostTimer.TimerFinished -= FinishBoost;
        _boostTimer.TimerValueChanged -= BoostTick;
    }

    private void PlayerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out Barier barrier))
        {
            if (!_immortal)
                Die();
            else
                Destroy(barrier.gameObject);
        }

        if (hit.collider.TryGetComponent(out Coin coin))
        {
            CollectCoin();
            Destroy(coin.gameObject);
        }
    }

    private void Die()
    {
        _losePanel.EnablePanel();
        _boostTimer.Stop();
        OnPlayerDiedEvent?.Invoke();
        playerMovement.enabled = false;
    }

    private void CollectCoin()
    {
        _bank.GetMoney(1);
    }

    private void CollectBoost(BoostType type)
    {
        if (type == BoostType.Shield)
        {
            StartBoost();
        }
    }

    private void StartBoost()
    {
        _boostTimer.Stop();
        
        _boostIndicator = Instantiate(boostIndicatorPrefab, _boostIndicators.transform);
        _boostIndicator.Init(BoostType.Shield);
        
        _boostTimer.Start(15);
        _immortal = true;
    }
    
    private void BoostTick(float timeLeft, TimeChangingSource source)
    {
        if (_boostIndicator != null)
            _boostIndicator.FillIndicator(timeLeft, 15);
    }

    private void FinishBoost()
    {
        Destroy(_boostIndicator.gameObject);
        
        _immortal = false;
    }
}
