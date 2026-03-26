import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { debounceTime } from 'rxjs';
import { MovieQuery } from '../../../Data/Request/MovieQuery';
import { GenreResponse } from '../../../Data/Response/GenreResponse';
import { MovieResponse } from '../../../Data/Response/MovieResponse';
import { GenreService } from '../../../Services/GenreService';
import { MovieService } from '../../../Services/MovieService';
import { ReviewService } from '../../../Services/ReviewService';
import { RouterLink } from '@angular/router';
import { GameService } from '../../../Services/GameService';
import { GameResponse } from '../../../Data/Response/GameResponse';

@Component({
  selector: 'app-game-web',
  imports: [RouterLink,ReactiveFormsModule],
  templateUrl: './game-web.html',
  styleUrl: './game-web.css',
})
export class GameWeb implements OnInit {
  filterForm: FormGroup;
  games: GameResponse[] = [];
  genres: GenreResponse[] = [];
  reviewsTitle: String[] = [];
  sortFields = [
    { name: 'Tytuł (A-Z)', value: 'Title|false' }, 
    { name: 'Ocena (najniższa)', value: 'average|false' }, 
    { name: 'Rok Wydania (najstarsze)', value: 'releaseDate|false' },
    { name: 'Tytuł (Z-A)', value: 'Title|true' },
    { name: 'Ocena (najwyższa)', value: 'average|true' },
    { name: 'Rok Wydania (najnowsze)', value: 'releaseDate|true' },
    ];
  constructor(private fb: FormBuilder,private cdr: ChangeDetectorRef,private gameService: GameService,private genreService: GenreService,private reviewService: ReviewService) {
  this.filterForm = this.fb.group({
      TitleSearch: [null],
      MinRating: [null],
      ReleaseYear: [null],
      genreName: [null],
      DirectorName: [null],
      DirectorSurname: [null],
      SortByField: [null],
      IsDescending: [false]
    });
  }
ngOnInit(): void {
    this.filterForm.valueChanges
      .pipe(
        debounceTime(300)
      )
      .subscribe((query: MovieQuery) => {
        this.loadMoviesByFilter(query);
      });
    this.loadGames();
    this.loadGenres();
    this.GetTheLastestReviews();
  }

  loadGames(): void {
    this.gameService.getGames().subscribe((data) => {
      this.games = data;
      console.log('Załadowano gry:', data);
      this.cdr.detectChanges();
    });
  }
  loadMoviesByFilter(query: MovieQuery): void {
    this.gameService.getGamesByFilter(query).subscribe({
        next: (data) => {
          console.log('Załadowano filmy z filtrami:', data);
            this.games = data;
        },
        error: (err) => {
            console.error('Błąd ładowania filmów:', err);
            this.games = [];
        }
    });
  }

loadGenres(): void {
  this.genreService.getGenres().subscribe((data) => {
    this.genres = data;
  });
}
  GetTheLastestReviews(): void {
    this.reviewService.getTheLastestReviews().subscribe((data) => {
      this.reviewsTitle = data;
    });
  }
}
