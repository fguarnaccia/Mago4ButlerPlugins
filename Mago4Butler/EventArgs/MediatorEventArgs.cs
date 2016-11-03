using System;

namespace Microarea.Mago4Butler
{
    public class JobEventArgs : EventArgs
    {
        public NotificationTypes NotificationType { get; set; }
        public string Progress { get; set; }
        public string Notification { get; set; }
        public Exception Error { get; set; }
        public AskForParametersBag Bag { get; set; }
    }

    [Flags]
    public enum NotificationTypes
    {
        None = 0,
        JobStarted = 1,
        JobEnded = 2,
        Progress = 4,
        Notification = 8,
        Error = 16
    }
}