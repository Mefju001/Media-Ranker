namespace WebApplication1.Models
{
    public enum EStatus
    {
        /// <summary>
        /// Status nieznany, domyślna wartość (0).
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Serial jest obecnie w ofercie i dostępny do oglądania.
        /// (Obejmuje: Dostępny, Trwa Emisja).
        /// </summary>
        Completed = 1,

        /// <summary>
        /// Serial jest aktywnie kontynuowany/odnawiany (np. ma nowe sezony).
        /// </summary>
        Continuing = 2,

        /// <summary>
        /// Serial ma ustaloną premierę, ale jeszcze nie jest dostępny.
        /// (Obejmuje: Wkrótce, Zapowiedziany).
        /// </summary>
        Upcoming = 3,

        /// <summary>
        /// Serial został zakończony przez twórców lub usunięty z platformy.
        /// (Obejmuje: Zakończony, Anulowany, Usunięty).
        /// </summary>
        EndedOrRemoved = 4
    }
}
