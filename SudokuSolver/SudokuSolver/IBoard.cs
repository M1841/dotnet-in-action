namespace SudokuSolver;

public interface IBoard
{
  int Size { get; }
  int GridSize { get; }

  int this[int row, int column] { get; set; }
}
