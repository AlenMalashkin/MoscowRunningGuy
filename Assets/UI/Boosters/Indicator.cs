using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Image filler;

    [Header("Sprites")] 
    [SerializeField] private Sprite shieldSprite;
    [SerializeField] private Sprite megaJumpSprite;
    [SerializeField] private Sprite multipleScoreSprite;
    
    public void Init(BoostType type)
    {
        switch (type)
        {
            case BoostType.Shield:
                bg.sprite = shieldSprite;
                break;
            case BoostType.MegaJump:
                bg.sprite = megaJumpSprite;
                break;
            case BoostType.MultipleScore:
                bg.sprite = multipleScoreSprite;
                break;
        }
    }
    
    public void FillIndicator(float timeLeft, float time)
    {
        var normalizedValue = Mathf.Clamp(timeLeft / time, 0.0f, 1.0f);
        filler.fillAmount = normalizedValue;
    }
}
