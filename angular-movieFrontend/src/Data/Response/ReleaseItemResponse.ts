import { GenreResponse } from "./GenreResponse";
import { MediaStatsResponse } from "./MediaStatsResponse";

export interface ReleaseItemResponse {
    Id:string;
    Title:string;
    Description:string;
    ReleaseDate:Date;
    MediaType:string;
    GenreResponse: GenreResponse;
    MediaStatsResponse: MediaStatsResponse;
}