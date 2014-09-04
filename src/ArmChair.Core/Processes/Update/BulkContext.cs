namespace ArmChair.Processes.Update
{
    using IdManagement;
    using InSession;

    public class BulkContext
    {
        public Key Key { get; set; }
        public object Entity { get; set; }
        public ActionType ActionType { get; set; }
        public bool TrackingRequiresReset { get; set; }
    }
}