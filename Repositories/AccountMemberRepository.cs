using BusinessObjects;
using DataAccessLayer;

namespace Repositories;
public class AccountMemberRepository: IAccountRepository {
  AccountMemberDAO dao = new AccountMemberDAO();

  public void AddAccount(AccountMember account) {
    dao.AddItem(account);
  }

  public void DeleteAccount(string accountId) {
      dao.RemoveItem(accountId);
    }

  public void DeleteAccount(AccountMember account) {
    dao.RemoveItem(account);
  }

  public AccountMember? GetAccount(string accountId) {
    return dao.GetItem(accountId);
  }

  public List<AccountMember> GetAccounts() {
    return dao.GetAllItems();
  }

  public void UpdateAccount(AccountMember account) {
    dao.UpdateItem(account);
  }
}
