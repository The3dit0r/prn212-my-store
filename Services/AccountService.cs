using BusinessObjects;
using Repositories;


namespace Services;
public class AccountService: IAccountService {
  private readonly IAccountRepository repo;

  public AccountService() {
    repo = new AccountMemberRepository();
  }

  public void AddAccount(AccountMember account) {
    repo.AddAccount(account);
  }

  public void DeleteAccount(string accountId) {
    repo.DeleteAccount(accountId);
  }

  public void DeleteAccount(AccountMember account) {
    repo.DeleteAccount(account);
  }

  public AccountMember? GetAccount(string accountId) {
    return repo.GetAccount(accountId);
  }

  public List<AccountMember> GetAccounts() {
    return repo.GetAccounts();
  }

  public void UpdateAccount(AccountMember account) {
    repo.UpdateAccount(account);
  }
}
