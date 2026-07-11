export interface TvSeriesRequest {
    title: string;
    description: string;
    genre: {
        name: string;
    };
    releaseDate: string;
    language: string;
    seasons: number;
    episodes: number;
    status: string;
}