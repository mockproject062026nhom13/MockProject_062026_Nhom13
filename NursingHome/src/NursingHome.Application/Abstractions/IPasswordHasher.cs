//should be in the same namespace as the other interfaces like security
namespace NursingHome.Application.Abstractions;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}