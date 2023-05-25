using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void PlayRunAnimation()
    {
        PlayAnimation(true, false, false);
    }

    public void PlayJumpAnimation()
    {
        PlayAnimation(false, true, false);
    }

    public void PlaySlideAnimation()
    {
        PlayAnimation(false, false, true);
    }

    private void PlayAnimation(bool running, bool jumping, bool sliding)
    {
        _animator.SetBool("Running", running);
        _animator.SetBool("Jumping", jumping);
        _animator.SetBool("Sliding", sliding);
    }
}
