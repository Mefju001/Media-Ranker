export interface MovieQuery {
        TitleSearch: string | null;
        MinRating: number | null;
        ReleaseYear: number|null;
        genreName: string | null;
        DirectorName: string | null;
        DirectorSurname: string | null;

        SortByField: string | null;
        IsDescending: Boolean;
}