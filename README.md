# Usage
## Unity Side
1. Create an empty gameobject in scene and add **KemothAdsManager** to it
2. Drag-Drop **MessageBoxCanvas** prfab from **Packages > KemothAds** folder in side this empty gameobject we created above

## Programming Side
This package can show *Interstial, Banner* and *Reward* ads and we are using [Event Bus](https://youtu.be/4_DTAnigmaQ?si=PDmstGMSgqmc9iwi) pattern to show ads and receive callbacks from them. Below are the examples for all the ads:
1. **Interstial Ads**
   - To show an ad call `EventBus<ShowInterstitialAdEvent>.Raise(new ShowInterstitialAdEvent())`
   - In any case like either user closed the ad or ad could not be shown, we will receive same event, to use that event follow the below code:
     ```c#
     // Create a binding to the event
     private EventBinding<InterstitialAdCompletedEvent> _interstitialAdCompleted;

     private void Stat()
     {
       // Register binding to the event bus
       _interstitialAdCompleted = new EventBinding<InterstitialAdCompletedEvent>();
       EventBus<InterstitialAdCompletedEvent>.RegisterBinding(_interstitialAdCompleted);
     }

     private void OnDestroy()
     {
       // Unregister binding from event bus
       EventBus<InterstitialAdCompletedEvent>.UnRegisterBinding(_interstitialAdCompleted);
     }

     private void OnInterstialAdCompleted()
     {
       // code to run after ad is closed ...
     }
     ```
   - sdfsdf
