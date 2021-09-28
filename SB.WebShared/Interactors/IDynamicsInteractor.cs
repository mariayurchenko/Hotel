using System.Threading.Tasks;

namespace SB.WebShared.Interactors
{
    public interface IDynamicsInteractor
    {
        Task<string> SendAction(string actionName, object obj = null);
    }
}