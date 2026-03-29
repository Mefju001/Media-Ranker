namespace Domain.Enums
{
    [Flags]
    public enum EPlatform
    {
        None = 0,
        PC = 1,
        Playstation = 2,
        Xbox = 4,
        Nintendo = 8,
        Mobile = 16
    }
}
