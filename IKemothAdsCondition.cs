namespace KemothStudios.KemothAds
{
    public interface IKemothAdsCondition
    {
        bool ValidateCondition(IKemothAdsConditionData conditionData);
    }
    
    public interface IKemothAdsCondition<in T> : IKemothAdsCondition where T : IKemothAdsConditionData
    {
        bool ValidateCondition(T conditionData);
    }
}
