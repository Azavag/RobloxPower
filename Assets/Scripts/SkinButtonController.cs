using UnityEngine;

public class SkinButtonController : MonoBehaviour
{
    /// <summary>
    /// Синхронизирует UI-карточки с массивом покупок из Bank.
    /// Слот 0 всегда бесплатный. Карточки также сами читают Bank в OnEnable.
    /// </summary>
    protected static void ApplyBuyStates(SkinCard[] cards, bool[] buyStates)
    {
        if (cards == null || cards.Length == 0)
            return;

        if (buyStates != null && buyStates.Length > 0)
            buyStates[0] = true;

        for (int i = 0; i < cards.Length; i++)
        {
            SkinCard card = cards[i];
            if (card == null)
                continue;

            // Карточка сама сверится с Bank (slot / price / buyStates).
            card.ApplyUnlockFromBank();

            bool unlockedByIndex = buyStates != null
                && i < buyStates.Length
                && buyStates[i];

            if (i == 0 || unlockedByIndex)
                card.Unclock();
        }
    }
}
