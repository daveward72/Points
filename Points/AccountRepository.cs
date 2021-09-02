
using Points.Models;

namespace Points;
public class AccountRepository : IAccountRepository
{
    private static Account account = new Account();

    public Account Find()
    {
        return account;
    }

    public void Update(Account acount)
    {
        // no op: there's no db to update
    }

    public void Reset()
    {
        account = new Account();
    }
}
