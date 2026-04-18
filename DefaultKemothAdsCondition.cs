using UnityEngine;

namespace KemothStudios.KemothAds
{
    internal class DefaultKemothAdsCondition : IKemothAdsCondition
    {
        public bool ValidateCondition(IKemothAdsConditionData conditionData)
        {
            return true;
        }
    }
}
