using KemothStudios.EventSystem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace KemothStudios.KemothAds
{
    public class KemothAdsMessageBox : MonoBehaviour
    {
        [SerializeField] private Text _messageText;
        [SerializeField] private CanvasGroup _messageGroup;

        EventBinding<ShowMessageEvent> _showMessageEvent;
        
        private void Start()
        {
            Assert.IsNotNull(_messageText, $"No message text assigned in {nameof(KemothAdsMessageBox)}");
            Assert.IsNotNull(_messageGroup, $"No message group assigned in {nameof(KemothAdsMessageBox)}");
            SetGroupVisibility(false);

            _showMessageEvent = new EventBinding<ShowMessageEvent>(ShowMessage);
            EventBus<ShowMessageEvent>.RegisterBinding(_showMessageEvent);
        }

        private void ShowMessage(ShowMessageEvent messageData)
        {
            _messageText.text = messageData.Message;
            SetGroupVisibility(true);
        }

        private void OnDestroy()
        {
            EventBus<ShowMessageEvent>.UnregisterBinding(_showMessageEvent);
        }

        public void SetGroupVisibility(bool visible)
        {
            _messageGroup.alpha = visible ? 1f : 0f;
            _messageGroup.blocksRaycasts = visible;
        }
    }
}
