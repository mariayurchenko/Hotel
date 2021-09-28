namespace SB.Actions
{
    internal interface IActionTracking
    {
        void Execute(string parameters, out string response);
    }
}