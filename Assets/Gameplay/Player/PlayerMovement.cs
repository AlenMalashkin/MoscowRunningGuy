using System.Collections;
using UnityEngine;
using VavilichevGD.Utils.Timing;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerBoosts playerBoosts;
    [SerializeField] private float timeToSpeedUp;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float slideDuration = 1;
    [SerializeField] private float step;
    [SerializeField] private Indicator boostIndicatorPrefab;
    [SerializeField] private Player player;

    private Indicator _boostIndicator;
    private BoostIndicators _boostIndicators;
    private SyncedTimer _speedUpTimer;
    private SyncedTimer _boostTimer;
    private Vector3 _moveDirection;
    private bool _movingToPosition;
    private int _currentLane = 1;

    [Inject]
    private void Construct(BoostIndicators boostIndicators)
    {
        _boostIndicators = boostIndicators;
    }
    
    private void Awake()
    {
        _speedUpTimer = new SyncedTimer(TimerType.UpdateTick);
        
        _speedUpTimer.Start(timeToSpeedUp);
        
        _boostTimer = new SyncedTimer(TimerType.UpdateTick);
    }

    private void OnEnable()
    {
        _speedUpTimer.TimerFinished += SpeedUp;
        playerBoosts.OnBoostCollectedEvent += CollectBoost;
        _boostTimer.TimerFinished += FinishBoost;
        _boostTimer.TimerValueChanged += BoostTick;
        player.OnPlayerDiedEvent += OnPlayerDied;
    }

    private void OnDisable()
    {
        _speedUpTimer.TimerFinished -= SpeedUp;
        playerBoosts.OnBoostCollectedEvent -= CollectBoost;
        _boostTimer.TimerFinished -= FinishBoost;
        _boostTimer.TimerValueChanged -= BoostTick;
        player.OnPlayerDiedEvent -= OnPlayerDied;
    }

    private void Update()
    { 
        Move();
    }

    private void FixedUpdate()
    {
        _moveDirection.z = moveSpeed;
        _moveDirection.y += gravity * Time.fixedDeltaTime;
        controller.Move(_moveDirection * moveSpeed * Time.deltaTime);
    }

    public void StartSlide()
    {
        StartCoroutine(Slide());
    }

    public void MoveRight()
    {
        if (_currentLane < 2)
            _currentLane++;
    }

    public void MoveLeft()
    {
        if (_currentLane > 0)
            _currentLane--;
    }

    public void Jump()
    {
        if (controller.isGrounded)
            _moveDirection.y = jumpStrength;
    }

    private void Move()
    {
        Vector3 targetPositoin = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (_currentLane == 0)
            targetPositoin += Vector3.left * step;
        else if (_currentLane == 2)
            targetPositoin += Vector3.right * step;

        if (transform.position == targetPositoin)
            return;
        Vector3 diff = targetPositoin - transform.position;
        Vector3 moveVec = diff.normalized * 25 * Time.deltaTime;
        if (moveVec.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveVec);
        else
            controller.Move(diff);
    }

    private void SpeedUp()
    {
        if (moveSpeed < 7.5f)
            moveSpeed += 0.2f;
        
        _speedUpTimer.Start(timeToSpeedUp);
    }

    private IEnumerator Slide()
    {
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;
        
        yield return new WaitForSeconds(slideDuration);
        
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
    }

    private void CollectBoost(BoostType type)
    {
        if (type == BoostType.MegaJump)
        {
            StartBoost();
        }
    }

    private void StartBoost()
    {
        _boostTimer.Stop();
        
        _boostIndicator = Instantiate(boostIndicatorPrefab, _boostIndicators.transform);
        _boostIndicator.Init(BoostType.MegaJump);
        
        _boostTimer.Start(15);
        jumpStrength *= 2;
    }

    private void BoostTick(float timeLeft, TimeChangingSource source)
    {
        if (_boostIndicator != null)
            _boostIndicator.FillIndicator(timeLeft, 15);
    }

    private void FinishBoost()
    {
        Destroy(_boostIndicator.gameObject);
        
        jumpStrength /= 2;
    }

    private void OnPlayerDied()
    {
        _boostTimer.Stop();
    }
}
