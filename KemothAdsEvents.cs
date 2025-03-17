using KemothStudios.EventSystem;

namespace KemothStudios.KemothAds
{
    /// <summary>
    /// Request to show an interstitial Ad
    /// </summary>
    public struct ShowInterstitialAdEvent : IEvent{}
    
    /// <summary>
    /// Showing of interstitial Ad is completed, either user closed it or Ad was failed load
    /// </summary>
    public struct InterstitialAdCompletedEvent : IEvent{}

    /// <summary>
    /// Request to show rewarded ad
    /// </summary>
    public struct ShowRewardedAdEvent : IEvent{}
    
    /// <summary>
    /// Reward ad has rewarded user by some amount
    /// </summary>
    public struct RewardedAdCompletedEvent : IEvent
    {
        public int RewardAmount { get; private set; }
        public RewardedAdCompletedEvent(int rewardAmount) => RewardAmount = rewardAmount;
    }
    
    /// <summary>
    /// Rewarded ad failed to be shown
    /// </summary>
    public struct RewardedAdFailedEvent : IEvent{}

    /// <summary>
    /// Shows a messagebox with passed message
    /// </summary>
    public struct ShowMessageEvent : IEvent
    {
        public string Message { get; set; }
        public ShowMessageEvent(string message) => Message = message;
    }
}
