namespace ProjectPet.Domain.Models;
public record Position
{
    private int __value;
    public int Value 
    { 
        get => __value; 
        private set => __value = Math.Max(1, value);
    }

    private Position(int value)
    {
        Value = value;
    }

    public static Position Create(int position)
        => new Position(position);

    public void MoveForward(int amount = 1) 
    {
        Value += amount;
    }

    public void MoveBackward(int amount = 1)
    {
        Value -= amount;
    }

    public static implicit operator Position (int position) 
        => new Position(position);
}

