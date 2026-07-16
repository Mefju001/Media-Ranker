import { DirectorResponse } from "./DirectorResponse";
import { GenreResponse } from "./GenreResponse";
import { MediaStatsResponse } from "./MediaStatsResponse";
import { ReviewResponse } from "./ReviewResponse";

export interface MovieResponse{
  id: string;
  title: string;
  description: string;
  genre: GenreResponse;
  director: DirectorResponse;
  releaseDate: string;
  language: string | null;
  reviews: ReviewResponse[] | null;
  mediaStats: MediaStatsResponse;
  duration: string;
  distributionType: string;
  status: string;
}