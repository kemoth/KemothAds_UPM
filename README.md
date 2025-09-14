# Usage
## Install dependencies
1. Install [Event System](https://github.com/kemoth/EventSystem_UPM) package by adding `https://github.com/kemoth/EventSystem_UPM` in package manager
2. Install [Utilities](https://github.com/kemoth/Utilities_UPM) package by adding `https://github.com/kemoth/Utilities_UPM` in package manager
3. Install [Mobile Ads SDK](https://developers.google.com/admob/unity/quick-start)

## Setup
1. Create an empty gameobject in scene and add **KemothAdsManager** to it
2. Drag-Drop **MessageBoxCanvas** prfab from **Packages > KemothAds** folder inside this empty gameobject we created above
3. Create ads configuration by right-clicking project window and select `Create > Kemoth Studios > KemothAds > Configuration`
   - Enable **Show Test Ads** while testing game or ads
   - Enabel the the type of ads you want to show
   - If and ad is enabled then assign an **AdUnitID** for it, if test ads are enabled then this is not necessary
4. Drag-Drop this configuration file in the **Kemoth Ads Configuration** field of the **KemothAdsManager**

**NOTE:**
> **KemothAdsManager** is not a singleton object

## Using package
This package can show *Interstitial, Banner* and *Reward* ads and we are using [Event Bus](https://youtu.be/4_DTAnigmaQ?si=PDmstGMSgqmc9iwi) pattern to show ads and receive callbacks from them. Below are the examples for all the ads:
1. **Interstitial Ads**
      - To show an ad call
        ```c#
        EventBus<ShowInterstitialAdEvent>.RaiseEvent(new ShowInterstitialAdEvent());
        ```
   - In any case like either user closed the ad or ad could not be shown, we will receive same event, to use that event follow the below code:
     ```c#
     // Create a binding to the event
     private EventBinding<InterstitialAdCompletedEvent> _interstitialAdCompleted;

     private void Start()
     {
       // Register binding to the event bus
       _interstitialAdCompleted = new EventBinding<InterstitialAdCompletedEvent>(OnInterstialAdCompleted);
       EventBus<InterstitialAdCompletedEvent>.RegisterBinding(_interstitialAdCompleted);
     }

     private void OnDestroy()
     {
       // Unregister binding from event bus
       EventBus<InterstitialAdCompletedEvent>.UnregisterBinding(_interstitialAdCompleted);
     }

     private void OnInterstialAdCompleted()
     {
       // code to run after ad is closed ...
     }
     ```
2. **Banner Ads**
   - To show an ad call
     ```c#
     EventBus<ShowBannerAdEvent>.RaiseEvent(new ShowBannerAdEvent);
     ```
   - To hide an ad call
     ```c#
     EventBus<HideBannerAdEvent>.RaiseEvent(new HideBannerAdEvent);
     ```
3. **Rewarded Ads**
   - To Show an ad call
     ```c#
     EventBus<ShowRewardedAdEvent>.RaiseEvent(new ShowRewardedAdEvent());
     ```
   - If user watched the ad and we need to award them then code below is an example for that:
     ```c#
     // Create a binding to the event
     private EventBinding<RewardedAdCompletedEvent> _rewardedAdCompleted;

     private void Start()
     {
       // Register binding to the event bus
       _rewardedAdCompleted = new EventBinding<RewardedAdCompletedEvent>(OnRewardUser);
       EventBus<RewardedAdCompletedEvent>.RegisterBinding(_rewardedAdCompleted);
     }

     private void OnDestroy()
     {
       // Unregister binding from event bus
       EventBus<RewardedAdCompletedEvent>.UnregisterBinding(_rewardedAdCompleted);
     }

     // This method can used without the argument also
     private void OnRewardUser(RewardedAdCompletedEvent rewardData)
     {
       // rewardData holds a RewardAmount integer which holds the amount we need to reward user with
       // For eg. we can use it to reward coins to user.
       // reward user here...
     }
     ```
   - If user cannot be rewarded in any case like they closed the rewarded ad or the ad not loaded itself then code below is an example for that:
     ```c#
     // Create a binding to the event
     private EventBinding<RewardedAdFailedEvent> _rewardFailedEvent;

     private void Start()
     {
       // Register binding to the event bus
       _rewardFailedEvent = new EventBinding<RewardedAdFailedEvent>(OnRewardFailed);
       EventBus<RewardedAdCompletedEvent>.RegisterBinding(_rewardFailedEvent);
     }

     private void OnDestroy()
     {
       // Unregister binding from event bus
       EventBus<RewardedAdFailedEvent>.UnregisterBinding(_rewardFailedEvent);
     }

     private void OnRewardFailed()
     {
       // handle failed reward here ...
     }
     ```
