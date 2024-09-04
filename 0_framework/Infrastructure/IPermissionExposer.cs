namespace _0_framework.Infrastructure;

public interface IPermissionExposer
{
    Dictionary<string, List<PermissionDto>> Expose();
}