namespace MediaGallery.API.Helpers;

public class PathBuilder
{
  public static string Build(string part1)
  {
    return string.Format("{0}/", part1);
  }
  public static string Build(string part1, string part2)
  {
    return string.Format("{0}/{1}/", part1, part2);
  }

  public static string Build(string part1, string part2, string part3)
  {
    return string.Format("{0}/{1}/{2}/", part1, part2, part3);
  }
}
