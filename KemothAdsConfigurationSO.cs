using UnityEngine;

namespace KemothStudios.KemothAds
{
    [CreateAssetMenu(fileName = "KemothAdsConfiguration", menuName = "Kemoth Studios/Kemoth Ads/Configuration")]

    public class KemothAdsConfigurationSO : ScriptableObject,  IKemothAdsConfiguration
    {
        [SerializeField, Tooltip("Will show test ads")] private bool _showTestAds = true;
        [SerializeField] private string _interstitialAdUnitID;
        [SerializeField] private string _rewardedAdUnitID;

        public bool ShowTestAds => _showTestAds;
        
        public string InterstitialAdUnitID => !_showTestAds ? _interstitialAdUnitID : "ca-app-pub-3940256099942544/1033173712";

        public string RewardedAdUnitID => !_showTestAds ? _rewardedAdUnitID : "ca-app-pub-3940256099942544/5224354917";
    }

    public interface IKemothAdsConfiguration
    {
        bool ShowTestAds { get; }
        string InterstitialAdUnitID { get;}
        string RewardedAdUnitID { get;}
    }
}
