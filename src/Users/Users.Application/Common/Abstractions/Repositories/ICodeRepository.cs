using Users.Domain;

namespace Users.Application.Common.Abstractions.Repositories;

public interface ICodeRepository
{
    void Add(Code code);
    Code? Get(string code);
    void PatchUsedAt(Code code);
}
