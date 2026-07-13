import { Routes } from '@angular/router';
import { MovieWeb } from '../ClientPages/MainPages/movie-web/movie-web';
import { MovieDetails } from '../ClientPages/DetailsPages/movie-details/movie-details';
import { GamesDetails } from '../ClientPages/DetailsPages/games-details/games-details';
import { MainWeb } from '../ClientPages/MainPages/main-web/main-web';
import { GameWeb } from '../ClientPages/MainPages/game-web/game-web';
import { TvSeriesWeb } from '../ClientPages/MainPages/tv-series-web/tv-series-web';
import { AdminDashboard } from '../AdminPages/admin-dashboard/admin-dashboard';
import { adminGuard } from '../ClientPages/auth/guard/admin.guard';
import { TvSeriesDetails } from '../ClientPages/DetailsPages/tv-series-details/tv-series-details';
export const routes: Routes = [
    {path: '',component: MainWeb},
    {path: 'movies', component: MovieWeb},
    {path: 'games',component: GameWeb},
    {path: 'tvSeries', component: TvSeriesWeb},
    {path: 'movie/:id', component: MovieDetails},
    {path: 'game/:id', component: GamesDetails},
    {path: 'tvSeries/:id', component: TvSeriesDetails},
    {path: 'adminDashboard', component: AdminDashboard, canActivate: [adminGuard]},
];
