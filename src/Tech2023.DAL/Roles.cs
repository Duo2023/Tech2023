namespace Tech2023.DAL;

public static class Roles
{
    public const string Administrator = nameof(Administrator);

    public static IEnumerable<string> All()
    {
        yield return Administrator;
    }
}
