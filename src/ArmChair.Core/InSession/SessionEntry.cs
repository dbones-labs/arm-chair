using ArmChair.IdManagement;

namespace ArmChair.InSession
{
    public class SessionEntry
    {
        public object Instance { get; set; }
        public Key Key { get; set; }
        public ActionType Action { get; set; }
    }
}