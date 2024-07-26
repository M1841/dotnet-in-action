namespace HelloDotNet;

/// <summary>Provides extension methods for manipulating strings.</summary>
public static class StringExtensions
{
  /// <summary>Returns a new string containing the odd-indexed characters from the input string.</summary>
  /// <param name="str">The input string.</param>
  /// <returns>A new string containing the odd-indexed characters from the input string.</returns>
  public static string OddChars(this string str)
  {
    return OddOrEvenChars(str, true);
  }

  /// <summary>Returns a new string containing the even-indexed characters from the input string.</summary>
  /// <param name="str">The input string.</param>
  /// <returns>A new string containing the even-indexed characters from the input string.</returns>
  public static string EvenChars(this string str)
  {
    return OddOrEvenChars(str, false);
  }

  /// <summary>Returns a new string containing the odd or even indexed characters from the input string.</summary>
  /// <param name="str">The input string.</param>
  /// <param name="isOdd">A boolean value indicating whether to return odd or even indexed characters.</param>
  /// <returns>A new string containing the odd or even indexed characters from the input string.</returns>
  private static string OddOrEvenChars(string str, bool isOdd)
  {
    int offset = isOdd ? 1 : 0;
    char[] chars = new char[(str.Length + offset) / 2];

    for (int i = 1 - offset; i < str.Length; i += 2)
    {
      chars[i / 2] = str[i];
    }

    return new string(chars);
  }
};
