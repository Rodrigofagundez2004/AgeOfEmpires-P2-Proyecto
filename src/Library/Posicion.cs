namespace Library;

public class Posicion
{
    public int X { get; set; }
    public int Y { get; set; }

    public Posicion(int x, int y)
    {
        X = x;
        Y = y;
    }

    public double DistanciaA(Posicion otra)
    {
        return Math.Sqrt(Math.Pow(X - otra.X, 2) + Math.Pow(Y - otra.Y, 2));
    }

    public override bool Equals(object? obj)
    {
        if (obj is Posicion pos)
            return X == pos.X && Y == pos.Y;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}