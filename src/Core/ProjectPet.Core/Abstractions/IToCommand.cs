namespace ProjectPet.Core.Abstractions;

public interface IToCommand<TCmd> where TCmd : class
{
    public TCmd ToCommand();
}

public interface IToCommand<TCmd, TArg1> where TCmd : class
{
    public TCmd ToCommand(TArg1 arg1);
}

public interface IToCommand<TCmd, TArg1, TArg2> where TCmd : class
{
    public TCmd ToCommand(TArg1 arg1, TArg2 arg2);
}

public interface IToCommand<TCmd, TArg1, TArg2, TArg3> where TCmd : class
{
    public TCmd ToCommand(TArg1 arg1, TArg2 arg2, TArg3 arg3);
}
