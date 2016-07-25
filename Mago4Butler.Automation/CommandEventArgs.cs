namespace Microarea.Mago4Butler.Automation
{
    public class CommandEventArgs : System.EventArgs
    {
        public string Command { get; set; }
        public string Response { get; set; }
        public string Args { get; set; }
    }
}