namespace SudokuSolver;

public class Solver(IBoard board)
{
  public bool Solve()
  {
    if (!IsValid()) { return false; }
    int size = _board.Size;
    for (int row = 0; row < size; ++row)
    {
      for (int col = 0; col < size; ++col)
      {
        if (_board[row, col] == 0)
        {
          for (int num = 1; num <= 9; ++num)
          {
            _board[row, col] = num;
            if (Solve()) { return true; }
          }
        }
      }
    }
    return false;
  }
  public bool IsValid()
  {
    int size = _board.Size;
    var usedNumsRow = new HashSet<int>();
    var usedNumsCol = new HashSet<int>();

    for (int i = 0; i < size; ++i)
    {
      usedNumsRow.Clear();
      usedNumsCol.Clear();

      for (int j = 0; j < size; ++j)
      {
        int numRow = _board[i, j];
        int numCol = _board[j, i];

        if (numRow != 0)
        {
          if (usedNumsRow.Contains(numRow)) { return false; }
          usedNumsRow.Add(numRow);
        }
        if (numCol != 0)
        {
          if (usedNumsCol.Contains(numCol)) { return false; }
          usedNumsCol.Add(numCol);
        }
      }
    }

    int sqrt = _board.GridSize;
    for (int grid = 0; grid < size; ++grid)
    {
      usedNumsRow.Clear();
      int startCol = grid % sqrt * sqrt;
      int startRow = grid / sqrt * sqrt;

      for (int cell = 0; cell < size; ++cell)
      {
        int col = startCol + (cell % sqrt);
        int row = startRow + (cell / sqrt);
        int num = _board[row, col];

        if (num != 0)
        {
          if (usedNumsRow.Contains(num)) { return false; }
          usedNumsRow.Add(num);
        }
      }
    }

    return true;
  }
  private readonly IBoard _board = board;
}
