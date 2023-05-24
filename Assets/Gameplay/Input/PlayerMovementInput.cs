using UnityEngine;
using Zenject;

public class PlayerMovementInput : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;

    [Inject]
    private void Construct(Player player)
    {
        _playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        tap = swipeDown = swipeUp = swipeLeft = swipeRight = false;
        
        HandleDesctopInput();
        HandleMobileInput();
        DetectSwipe();
        MovePlayer();
    }

    private void HandleDesctopInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            Reset();
        }
    }

    private void HandleMobileInput()
    {
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();
            }
        }
    }

    private void DetectSwipe()
    {
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length < 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        if (swipeDelta.magnitude > 100)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {

                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {

                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }

            Reset();
        }
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }

    private void MovePlayer()
    {
        if (swipeLeft)
            _playerMovement.MoveLeft();
        
        if (swipeRight)
            _playerMovement.MoveRight();

        if (swipeUp)
            _playerMovement.Jump();
        
        if (swipeDown)
            _playerMovement.StartSlide();
    }
}