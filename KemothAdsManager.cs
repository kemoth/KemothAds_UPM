using GoogleMobileAds.Api;
using KemothStudios.EventSystem;
using KemothStudios.Utilities.Attributes;
using KemothStudios.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace KemothStudios.KemothAds
{
    public class KemothAdsManager : MonoBehaviour
    {
        [SerializeField, RequireInterface(typeof(IKemothAdsConfiguration))]
        private Object _kemothAdsConfiguration;

        private IKemothAdsConfiguration _configuration;
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;
        private BannerView _bannerAd;
        private EventBinding<ShowInterstitialAdEvent> _showInterstitialAd;
        private EventBinding<ShowRewardedAdEvent> _showRewardedAd;
        private EventBinding<RewardedAdFailedEvent> _rewardedAdFailed;
        private EventBinding<HideBannerAdEvent> _hideBannerAd;
        private EventBinding<ShowBannerAdEvent> _showBannerAd;
        private bool _isSDKInitialized;
        private ShowMessageEvent _messageData;

        private void Start()
        {
            Assert.IsNotNull(_kemothAdsConfiguration, $"Ads configuration not provided to {nameof(KemothAdsManager)}");
            _configuration = _kemothAdsConfiguration as IKemothAdsConfiguration;
            MobileAds.Initialize(x =>
            {
                _isSDKInitialized = true;
                if (_configuration.CanShowInterstitialAds)
                {
                    LoadInterstitialAd();
                    _showInterstitialAd = new EventBinding<ShowInterstitialAdEvent>(ShowInterstitialAd);
                    EventBus<ShowInterstitialAdEvent>.RegisterBinding(_showInterstitialAd);
                }

                if (_configuration.CanShowRewardedAds)
                {
                    LoadRewardedAd();
                    _showRewardedAd = new EventBinding<ShowRewardedAdEvent>(ShowRewardedAd);
                    EventBus<ShowRewardedAdEvent>.RegisterBinding(_showRewardedAd);
                    _rewardedAdFailed = new EventBinding<RewardedAdFailedEvent>(RewardedAdFailed);
                    EventBus<RewardedAdFailedEvent>.RegisterBinding(_rewardedAdFailed);
                }

                if (_configuration.CanShowBannerAds)
                {
                    _showBannerAd = new EventBinding<ShowBannerAdEvent>(ShowBannerAd);
                    EventBus<ShowBannerAdEvent>.RegisterBinding(_showBannerAd);
                    _hideBannerAd = new EventBinding<HideBannerAdEvent>(HideBannerAd);
                    EventBus<HideBannerAdEvent>.RegisterBinding(_hideBannerAd);
                }

                _messageData = new ShowMessageEvent();
            });
        }

        private void OnDestroy()
        {
            if (_isSDKInitialized)
            {
                if (_configuration.CanShowInterstitialAds)
                    EventBus<ShowInterstitialAdEvent>.UnregisterBinding(_showInterstitialAd);
                if (_configuration.CanShowRewardedAds)
                {
                    EventBus<ShowRewardedAdEvent>.UnregisterBinding(_showRewardedAd);
                    EventBus<RewardedAdFailedEvent>.UnregisterBinding(_rewardedAdFailed);
                }

                if (_configuration.CanShowBannerAds)
                {
                    EventBus<ShowBannerAdEvent>.UnregisterBinding(_showBannerAd);
                    EventBus<HideBannerAdEvent>.UnregisterBinding(_hideBannerAd);
                }
                DestroyInterstitialAd();
                DestroyRewardedAd();
                DestroyBannerAd();
            }
        }

        private void LoadInterstitialAd()
        {
            DestroyInterstitialAd();

            AdRequest req = new AdRequest();
            InterstitialAd.Load(_configuration.InterstitialAdUnitID, req, (ad, reqErr) =>
            {
                if (reqErr != null)
                    DebugUtility.LogError($"Interstitial Ad failed to load: {reqErr}");
                else
                {
                    _interstitialAd = ad;
                    _interstitialAd.OnAdFullScreenContentClosed += () =>
                    {
                        EventBus<InterstitialAdCompletedEvent>.RaiseEvent(new InterstitialAdCompletedEvent());
                        LoadInterstitialAd();
                    };
                    _interstitialAd.OnAdFullScreenContentFailed += adError =>
                    {
                        DebugUtility.LogColored("red", $"Interstitial Ad failed to load: {adError}");
                        EventBus<InterstitialAdCompletedEvent>.RaiseEvent(new InterstitialAdCompletedEvent());
                        LoadInterstitialAd();
                    };
                }
            });
        }

        private void LoadRewardedAd()
        {
            DestroyRewardedAd();

            AdRequest req = new AdRequest();
            RewardedAd.Load(_configuration.RewardedAdUnitID, req, (ad, reqErr) =>
            {
                if (reqErr != null)
                    DebugUtility.LogError($"Rewarded Ad failed to load: {reqErr}");
                else
                {
                    _rewardedAd = ad;
                    _rewardedAd.OnAdFullScreenContentClosed += LoadRewardedAd;
                    _rewardedAd.OnAdFullScreenContentFailed += adErr =>
                    {
                        DebugUtility.LogColored("red", $"Rewarded Ad failed to load: {adErr}");
                        EventBus<RewardedAdFailedEvent>.RaiseEvent(new RewardedAdFailedEvent());
                        LoadRewardedAd();
                    };
                }
            });
        }

        private void ShowBannerAd()
        {
            if (_bannerAd == null)
            {
                AdRequest req = new AdRequest();
                _bannerAd = new BannerView(_configuration.BannerAdUnitID, AdSize.Banner, AdPosition.Bottom);
                _bannerAd.LoadAd(req);
            }else _bannerAd.Show();
        }

        private void HideBannerAd()
        {
            if (_bannerAd != null)
            {
                _bannerAd.Hide();
            }
        }
        
        private void ShowInterstitialAd()
        {
            if (!_configuration.CanShowInterstitialAds)
            {
                DebugUtility.LogError("Trying to show  Interstitial Ad but they are disabled in Ads configuration");
                return;
            }

            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                _interstitialAd.Show();
            }
            else
            {
                EventBus<InterstitialAdCompletedEvent>.RaiseEvent(new InterstitialAdCompletedEvent());
                LoadInterstitialAd();
            }
        }

        private void ShowRewardedAd()
        {
            if (!_configuration.CanShowRewardedAds)
            {
                DebugUtility.LogError("Trying to show Rewarded Ad but they are disabled in Ads configuration");
                return;
            }

            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show(reward => { EventBus<RewardedAdCompletedEvent>.RaiseEvent(new RewardedAdCompletedEvent(_configuration.ShowTestAds ? 10 : (int)reward.Amount)); });
            }
            else
            {
                EventBus<RewardedAdFailedEvent>.RaiseEvent(new RewardedAdFailedEvent());
            }
        }

        private void RewardedAdFailed()
        {
            _messageData.Message = "Reward video failed to load, try again later";
            EventBus<ShowMessageEvent>.RaiseEvent(_messageData);
        }

        private void DestroyInterstitialAd()
        {
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }
        }

        private void DestroyRewardedAd()
        {
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }
        }

        private void DestroyBannerAd()
        {
            if (_bannerAd != null)
            {
                _bannerAd.Destroy();
                _bannerAd = null;
            }
        }
    }
}