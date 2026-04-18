namespace KemothStudios.KemothAds
{
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
