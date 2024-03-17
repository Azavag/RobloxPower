using UnityEngine;
using UnityEngine.UI;

namespace QuantumTek.QuantumUI
{
    /// <summary>
    /// QUI_SwitchToggle handles the change of sprites for a toggle UI element on click.
    /// </summary>
    [AddComponentMenu("Quantum Tek/Quantum UI/Switch Toggle")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Toggle))]
    public class QUI_SwitchToggle : MonoBehaviour
    {
        [Header("Switch Toggle Object References")]
        [Tooltip("The toggle element to use.")]
        public Toggle toggle;
        [Tooltip("The toggle graphic to set.")]
        public Image toggleGraphic;
        [Tooltip("The punchbagSprite when the toggle is on.")]
        public Sprite onSprite;
        [Tooltip("The punchbagSprite when the toggle is off.")]
        public Sprite offSprite;
        [Tooltip("The toggle backgroundImage graphic to set.")]
        public Image toggleBackgroundGraphic;
        [Tooltip("The punchbagSprite backgroundImage when the toggle is on.")]
        public Sprite onBackgroundSprite;
        [Tooltip("The punchbagSprite backgroundImage when the toggle is off.")]
        public Sprite offBackgroundSprite;

        /// <summary>
        /// Updates the graphic shown on the switch. Used in the toggle's onclick event.
        /// </summary>
        public void SetToggleGraphic()
        {
            toggleGraphic.sprite = toggle.isOn ? onSprite : offSprite;
            toggleGraphic.enabled = true;
            toggleBackgroundGraphic.sprite = toggle.isOn ? onBackgroundSprite : offBackgroundSprite;
        }
    }
}