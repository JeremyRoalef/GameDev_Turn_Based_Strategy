//A struct is a value type, and a class is a reference type

/*
 * Common structs are Vectors
 */
public struct GridPosition
{
    public int x;
    public int z;

    public GridPosition(int x, int z)
    {
        this.x = x; 
        this.z = z;
    }

    public override string ToString()
    {
        return $"({x}, {z})";
    }
}
