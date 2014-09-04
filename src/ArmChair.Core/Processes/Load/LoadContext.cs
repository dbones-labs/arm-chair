namespace ArmChair.Processes.Load
{
    using IdManagement;

    public class LoadContext
    {
        public Key Key { get; set; }
        public object Entity { get; set; }
        public bool LoadedFromCache { get; set; }
    }
}