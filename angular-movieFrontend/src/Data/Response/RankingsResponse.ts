import { GenreResponse } from "./GenreResponse";
import { MediaStatsResponse } from "./MediaStatsResponse";

export interface RankingsResponse {
    Id: string;
    Title: string;
    Description: string;
    ReleaseDate: Date;
    MediaType: string;
    Genre: GenreResponse;
    MediaStatsResponse: MediaStatsResponse;
    type: string;
}