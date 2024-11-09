namespace ProjectPet.Domain.Models;
public record Position
{
    public int Value { get; }

    private Position(int value)
    {
        Value = Math.Max(1, value);
    }

    public static Position Create(int position)
        => new Position(position);

    public Position MoveForward(int amount = 1)
        => new Position(Value + amount);

    public Position MoveBackward(int amount = 1)
        => new Position(Value - amount);

    public static implicit operator Position(int position)
        => new Position(position);
}

