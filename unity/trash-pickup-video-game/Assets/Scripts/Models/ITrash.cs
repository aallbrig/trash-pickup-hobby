using UnityEngine;

namespace Models
{
    public interface ITrash
    {
        float WeightAddInGallons { get; }

        float Score { get; }

        AudioClip CollectSound { get; }
    }
}