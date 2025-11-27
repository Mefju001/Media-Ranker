import { Component } from '@angular/core';
import { MovieService } from '../../Services/MovieService';
import { GenreService } from '../../Services/GenreService';
import { MovieResponse } from '../../Data/Response/MovieResponse';
import { GenreResponse } from '../../Data/Response/GenreResponse';
@Component({
  selector: 'app-movie-web',
  imports: [],
  templateUrl: './movie-web.html',
  styleUrl: './movie-web.css'
})
export class MovieWeb {
  movies: MovieResponse[] = [];
  genres: GenreResponse[] = [];

  constructor(private movieService: MovieService,private genreService: GenreService) {
    this.loadMovies();
    this.loadGenres();
  }
  loadMovies(): void {
    this.movieService.getMovies().subscribe((data) => {
      this.movies = data;
    });
  }
  loadGenres(): void {
    this.genreService.getGenres().subscribe((data) => {
      this.genres = data;
      console.log(this.genres);
    });
  }
}
