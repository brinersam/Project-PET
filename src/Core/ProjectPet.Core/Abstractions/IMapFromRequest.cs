namespace ProjectPet.Core.Abstractions;
public interface IMapFromRequest<TCmd,TReq> 
    where TCmd : class
    where TReq : class
{
    public abstract static TCmd FromRequest(TReq request);
}

public interface IMapFromRequest<TCmd, TReq, TArg1>
    where TCmd : class
    where TReq : class
{
    public abstract static TCmd FromRequest(TReq request, TArg1 arg1);
}

public interface IMapFromRequest<TCmd, TReq, TArg1, TArg2>
    where TCmd : class
    where TReq : class
{
    public abstract static TCmd FromRequest(TReq request, TArg1 arg1, TArg2 arg2);
}

public interface IMapFromRequest<TCmd, TReq, TArg1, TArg2, TArg3>
    where TCmd : class
    where TReq : class
{
    public abstract static TCmd FromRequest(TReq request, TArg1 arg1, TArg2 arg2, TArg3 arg3);
}
