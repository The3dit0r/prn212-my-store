using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;
public interface IAccountService {
  AccountMember? GetAccount(string accountId);
  List<AccountMember> GetAccounts();

  AccountMember Login(string username, string password);
}
