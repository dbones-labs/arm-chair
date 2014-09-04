namespace ArmChair.Tracking
{
    public interface ITrackingProvider
    {
        object TrackInstance(object instance);
        void Reset(object instance);
        bool HasChanges(object instance);
        void CeaseTracking(object instance);
    }
}
