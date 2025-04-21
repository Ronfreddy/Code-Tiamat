using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamagePopText : MonoBehaviour
{
    public TextMeshPro damageText;

    public void SetDamageText(float damage)
    {
        damageText.text = Mathf.RoundToInt(damage).ToString();

        float randomX = Random.Range(-0.5f, 0.5f);
        transform.DOMoveX(transform.position.x + randomX, 0.6f).SetEase(Ease.OutQuad);
        transform.DOMoveY(transform.position.y + 1, 0.6f).SetEase(Ease.OutQuad);
        DOTween.To(() => damageText.color, x => damageText.color = x, new Color(1, 0, 0, 0), 0.6f)
            .OnComplete(() => Destroy(gameObject));
    }
}
