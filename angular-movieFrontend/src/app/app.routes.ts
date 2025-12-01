import { Routes } from '@angular/router';
import { MovieWeb } from '../Pages/movie-web/movie-web';
import { MovieDetails } from '../Pages/movie-details/movie-details';
export const routes: Routes = [
    { path: '',component: MovieWeb, runGuardsAndResolvers: 'always' },
    { path: 'movie/id/:id', component: MovieDetails, runGuardsAndResolvers: 'always' },
];
