import { Routes } from '@angular/router';
import { MovieWeb } from '../Pages/movie-web/movie-web';
import { MovieDetails } from '../Pages/movie-details/movie-details';
import { MainWeb } from '../Pages/MainPages/main-web/main-web';
import { GameWeb } from '../Pages/MainPages/game-web/game-web';
import { TvSeriesWeb } from '../Pages/MainPages/tv-series-web/tv-series-web';
export const routes: Routes = [
    { path: '',component: MainWeb},
    {path: 'movies', component: MovieWeb},
    {path: 'games',component: GameWeb},
    {path: 'tvSeries', component: TvSeriesWeb},
    { path: 'movie/:id', component: MovieDetails},
];
