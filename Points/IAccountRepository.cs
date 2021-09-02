
using Points.Models;

namespace Points;
public interface IAccountRepository
{
    Account Find(); // there's only one!

    void Update(Account account); 
}
