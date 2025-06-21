using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace DataAccessLayer;
public class AccountMemberDAO : _BaseDAO<AccountMember> {
  public AccountMemberDAO() : base(new DatabaseContext(), "AccountMembers") { }

  public AccountMember? GetItem(string id) {
    return GetItem((item) => item.MemberId == id);
  }

  public void RemoveItem(string id) {
    RemoveItems((item) => item.MemberId == id);
  }
}
