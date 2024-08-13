using System.Security.Cryptography;
using SudokuSolver;

namespace SudokuSolver.UnitTests;

public class SolverTests
{
  [Theory]
  [InlineData(0, false)]
  [InlineData(1, false)]
  [InlineData(4, true)]
  [InlineData(8, false)]
  [InlineData(9, true)]
  [InlineData(10, false)]
  [InlineData(16, true)]
  public void EmptyBoardSizes(int size, bool isValid)
  {
    if (!isValid)
    {
      Assert.Throws<ArgumentException>(
        nameof(size),
        () => new ArrayBoard(size));
    }
    else { _ = new ArrayBoard(size); }
  }

  [Theory]
  [MemberData(nameof(Boards))]
  public void CheckRules(IBoard board, bool isValid)
  {
    var solver = new Solver(board);
    Assert.Equal(isValid, solver.IsValid());
  }

  public static IEnumerable<object[]> Boards
  {
    get
    {
      IBoard board = new ArrayBoard(4);
      board[1, 0] = 1;
      board[3, 0] = 1;
      yield return new object[] { board, false };

      board = new ArrayBoard(4);
      board[1, 0] = 1;
      board[1, 2] = 1;
      yield return new object[] { board, false };

      board = new ArrayBoard(4);
      board[1, 2] = 1;
      board[1, 3] = 1;
      yield return new object[] { board, false };

      board = new ArrayBoard(4);
      board[1, 1] = 1;
      board[2, 3] = 1;
      yield return new object[] { board, true };
    }
  }
}
