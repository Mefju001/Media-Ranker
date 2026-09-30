export interface TvSeriesQuery{
    TitleSearch: string | null;
    MinRating: number | null;
    ReleaseYear: number | null;
    genreName: string | null;
    status: string | null;
    
    SortByField: string | null;
    IsDescending: boolean;
}