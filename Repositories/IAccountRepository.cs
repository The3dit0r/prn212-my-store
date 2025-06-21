using BusinessObjects;
namespace Repositories;
public interface IAccountRepository {
  AccountMember? GetAccountById(string accountId);
  List<AccountMember> GetAccounts();

  void AddAccount(AccountMember account);

  void DeleteAccount(string accountId);
  void DeleteAccount(AccountMember account);
  void UpdateAccount(AccountMember account);
}
