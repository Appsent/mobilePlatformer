using UnityEngine;
using System.Collections;

public class SneakBar : MonoBehaviour
{
    public Player Player;
    public Transform ForegroundSprite;
    public SpriteRenderer ForegroundRenderer;
    public Color MinSneakColor = new Color(255 / 255f, 63 / 255f, 63 / 255f);
    public Color MaxSneakColor = new Color(64 / 255f, 137 / 255f, 255 / 255f);

    public void Update()
    {
        var sneakPercent = Player.sneaking/ Player.sneakDuration;

        ForegroundSprite.localScale = new Vector3(sneakPercent, 1, 1);
        ForegroundRenderer.color = Color.Lerp(MaxSneakColor, MinSneakColor, sneakPercent);
    }

}
