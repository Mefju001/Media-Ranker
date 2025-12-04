import { GenreResponse } from "./GenreResponse";
import { MediaStatsResponse } from "./MediaStatsResponse";
import { ReviewResponse } from "./ReviewResponse";

export interface TvSeriesResponse {
    mediaStats: MediaStatsResponse;
    seasons: number;
    episodes: number;
    network: string;
    status: string;
    id: number;
    title: string;
    description: string;
    
    genre: GenreResponse;
    
    releaseDate: string; // Data jest stringiem
    language: string;
    
    // Tablica recenzji
    reviews: ReviewResponse[];
}