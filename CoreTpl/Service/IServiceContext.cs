namespace CoreTpl.Service
{
    public interface IServiceContext
    {
        IRoleService Role { get; }
        IUserService User { get; }
    }

}