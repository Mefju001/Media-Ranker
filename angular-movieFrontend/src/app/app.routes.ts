import { Routes } from '@angular/router';
import { MovieWeb } from '../ClientPages/movie-web/movie-web';
import { MovieDetails } from '../ClientPages/DetailsPages/movie-details/movie-details';
import { MainWeb } from '../ClientPages/MainPages/main-web/main-web';
import { GameWeb } from '../ClientPages/MainPages/game-web/game-web';
import { TvSeriesWeb } from '../ClientPages/MainPages/tv-series-web/tv-series-web';
import { AdminDashboard } from '../AdminPages/admin-dashboard/admin-dashboard';
import { adminGuard } from '../ClientPages/auth/guard/admin.guard';
export const routes: Routes = [
    {path: '',component: MainWeb},
    {path: 'movies', component: MovieWeb},
    {path: 'games',component: GameWeb},
    {path: 'tvSeries', component: TvSeriesWeb},
    {path: 'movie/:id', component: MovieDetails},
    {path: 'adminDashboard', component: AdminDashboard, canActivate: [adminGuard]},
];
