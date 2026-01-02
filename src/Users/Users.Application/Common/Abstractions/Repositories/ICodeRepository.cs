using Users.Domain;

namespace Users.Application.Common.Abstractions.Repositories;

public interface ICodeRepository
{
    void Add(Code code);
    Code? Get(string code);
    Code? GetByUserId(Guid userId);
    void Patch(Code code);
}
