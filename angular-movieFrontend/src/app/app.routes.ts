import { Routes } from '@angular/router';
import { MovieWeb } from '../ClientPages/movie-web/movie-web';
import { MovieDetails } from '../ClientPages/movie-details/movie-details';
import { MainWeb } from '../ClientPages/MainPages/main-web/main-web';
import { GameWeb } from '../ClientPages/MainPages/game-web/game-web';
import { TvSeriesWeb } from '../ClientPages/MainPages/tv-series-web/tv-series-web';
export const routes: Routes = [
    { path: '',component: MainWeb},
    {path: 'movies', component: MovieWeb},
    {path: 'games',component: GameWeb},
    {path: 'tvSeries', component: TvSeriesWeb},
    { path: 'movie/:id', component: MovieDetails},
];
