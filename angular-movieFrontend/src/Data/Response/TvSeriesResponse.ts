import { GenreResponse } from "./GenreResponse";
import { MediaStatsResponse } from "./MediaStatsResponse";
import { ReviewResponse } from "./ReviewResponse";

export interface TvSeriesResponse {
    GenreResponse: any;
    mediaStats: MediaStatsResponse;
    seasons: number;
    episodes: number;
    network: string;
    status: string;
    id: string;
    title: string;
    description: string;
    
    genreResponse: GenreResponse;
    
    releaseDate: string;
    language: string;

    reviews: ReviewResponse[];
}