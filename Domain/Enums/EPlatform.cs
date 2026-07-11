using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum EPlatform
    {
        [Display(Name = "PC")]
        PC,

        [Display(Name = "PlayStation 5")]
        PlayStation5,

        [Display(Name = "PlayStation 4")]
        PlayStation4,

        [Display(Name = "Xbox Series X/S")]
        XboxSeries,

        [Display(Name = "Xbox One")]
        XboxOne,

        [Display(Name = "Nintendo Switch")]
        NintendoSwitch,

        [Display(Name = "Urządzenia mobilne")]
        Mobile,

        [Display(Name = "Steam Deck")]
        SteamDeck,

        [Display(Name = "Virtual Reality")]
        VR,

        [Display(Name = "Przeglądarka WWW")]
        WebBrowser
    }
}
