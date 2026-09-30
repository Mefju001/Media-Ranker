import { GameResponse } from "./GameResponse";
import { MovieResponse } from "./MovieResponse";
import { TvSeriesResponse } from "./TvSeriesResponse";

export type MediaResponse = MovieResponse | GameResponse | TvSeriesResponse;