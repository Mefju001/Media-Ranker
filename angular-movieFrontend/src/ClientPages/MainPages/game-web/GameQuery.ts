export interface GameQuery {
    TitleSearch?: string | null;
    MinRating?: number | null;
    Platform?: string | null;
    Developer?: string | null;
    ReleaseDate?: number | null;
    genreName?: string | null;
    
    SortByField?: string | null;
    IsDescending?: boolean | null;
}