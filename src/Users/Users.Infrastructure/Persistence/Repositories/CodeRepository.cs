using Users.Application.Common.Abstractions.Repositories;
using Users.Domain;

namespace Users.Infrastructure.Persistence.Repositories;

public sealed class CodeRepository : ICodeRepository
{
    private static List<Code> _codes = [];

    public void Add(Code code)
    {
        _codes.Add(code);
    }

    public Code? Get(string code)
    {
        return _codes.FirstOrDefault(c => c.Value.Equals(code));
    }

    public Code? GetByUserId(Guid userId)
    {
        return _codes.FirstOrDefault(c => c.UserId == userId);
    }

    public void Patch(Code code)
    {
        int idx = _codes.IndexOf(code);
        _codes[idx] = code;
    }
}