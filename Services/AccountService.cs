using BusinessObjects;
using Repositories;


namespace Services;
public class AccountService: IAccountService {
  private readonly IAccountRepository repo;

  public AccountService() {
    repo = new AccountMemberRepository();
  }
  public AccountMember? GetAccount(string accountId) {
    return repo.GetAccount(accountId);
  }

  public List<AccountMember> GetAccounts() {
    return repo.GetAccounts();
  }

  public AccountMember Login(string username, string password) {
    var user = repo.GetAccount(username);
    if (user == null) {
      throw new Exception($"Cannot find account with username: {username}");
    }

    if (user.MemberPassword.Equals(password)) {
      return user;
    }

    throw new Exception($"Login password for {username} is incorrect!");
  }
}
