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

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public override bool Equals(object obj)
    {
        if (obj is Posicion otra)
            return X == otra.X && Y == otra.Y;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}