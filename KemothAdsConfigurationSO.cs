using KemothStudios.Utilities;
using UnityEngine;
using static KemothStudios.KemothAds.KemothAdsStatics;

namespace KemothStudios.KemothAds
{
    [CreateAssetMenu(fileName = "KemothAdsConfiguration", menuName = "Kemoth Studios/Kemoth Ads/Configuration")]

    public class KemothAdsConfigurationSO : ScriptableObject,  IKemothAdsConfiguration
    {
        [SerializeField, Tooltip("Will show test ads")] private bool _showTestAds = true;
        [SerializeField] private bool _canShowInterstitialAds;
        [SerializeField] private string _interstitialAdUnitID;
        [SerializeField] private bool _canShowRewardedAds;
        [SerializeField] private string _rewardedAdUnitID;
        [SerializeField] private bool _canShowBannerAds;
        [SerializeField] private string _bannerAdUnitID;

        public bool ShowTestAds => _showTestAds;

        public string InterstitialAdUnitID
        {
            get
            {
                if (!_showTestAds)
                {
                    if (!string.IsNullOrEmpty(_interstitialAdUnitID)) return _interstitialAdUnitID;
                    DebugUtility.LogColored("yellow", "Interstitial ad ID is not provided, test ad ID will be returned");
                }
                return TEST_INTERSTITIAL_AD_ID;
            }
        }

        public string RewardedAdUnitID
        {
            get
            {
                if (!_showTestAds)
                {
                    if (!string.IsNullOrEmpty(_rewardedAdUnitID)) return _rewardedAdUnitID;
                    DebugUtility.LogColored("yellow", "Rewarded ad ID is not provided, test ad ID will be returned");
                }
                return TEST_REWARDED_AD_ID;
            }
        }

        public string BannerAdUnitID
        {
            get
            {
                if (!_showTestAds)
                {
                    if (!string.IsNullOrEmpty(_bannerAdUnitID)) return _bannerAdUnitID;
                    DebugUtility.LogColored("yellow", "Banner ad ID is not provided, test ad ID will be returned");
                }
                return TEST_BANNER_AD_ID;
            }
        }

        public bool CanShowInterstitialAds => _canShowInterstitialAds;
        public bool CanShowRewardedAds => _canShowRewardedAds;
        public bool CanShowBannerAds => _canShowBannerAds;
    }

    public interface IKemothAdsConfiguration
    {
        bool ShowTestAds { get; }
        bool CanShowInterstitialAds { get; }
        string InterstitialAdUnitID { get;}
        public bool CanShowRewardedAds { get; }
        string RewardedAdUnitID { get;}
        public bool CanShowBannerAds { get; }
        string BannerAdUnitID { get;}
    }
}
