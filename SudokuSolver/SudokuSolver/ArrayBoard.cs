namespace SudokuSolver;

public class ArrayBoard : IBoard
{
  public int Size => _size;
  public int GridSize => _gridSize;

  public int this[int row, int column]
  {
    get => _boardArray[row, column];
    set => _boardArray[row, column] = value;
  }
  public ArrayBoard(int size)
  {
    if (!IsValidSize(size, out _gridSize))
    {
      throw new ArgumentException($"Invalid size: {size}", nameof(size));
    }
    _boardArray = new int[size, size];
    _size = size;
  }

  private static bool IsValidSize(int size, out int sqrt)
  {
    sqrt = (int)Math.Sqrt(size);
    return size >= 4 && (sqrt * sqrt) == size;
  }
  private readonly int[,] _boardArray;
  private readonly int _size;
  private readonly int _gridSize;
}
