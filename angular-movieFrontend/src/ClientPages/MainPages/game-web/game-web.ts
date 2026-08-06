import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { debounceTime } from 'rxjs';
import { MovieQuery } from '../../../Data/Request/MovieQuery';
import { GenreResponse } from '../../../Data/Response/GenreResponse';
import { GenreService } from '../../../Services/GenreService';
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
  platforms: String[] = [];
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
      Platform: [null],
      Developer: [null],
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
        this.loadGamesByFilter(query);
      });
    this.loadGames();
    this.loadGenres();
    this.loadPlatforms();
    this.GetTheLastestReviews();
  }

  loadGames(): void {
    this.gameService.getGames().subscribe((data) => {
      this.games = data;
      console.log('Załadowano gry:', data);
      this.cdr.detectChanges();
    });
  }
  loadGamesByFilter(query: MovieQuery): void {
    this.gameService.getGamesByFilter(query).subscribe({
        next: (data) => {
          console.log('Załadowano gry z filtrami:', data);
            this.games = data;
        },
        error: (err) => {
            console.error('Błąd ładowania gier:', err);
            this.games = [];
        }
    });
  }
loadPlatforms(): void {
  this.gameService.GetPlatforms().subscribe((data) => {
    console.log('Załadowano platformy:', data);
    this.platforms = data;
  });
}
loadGenres(): void {
  this.gameService.GetGenres().subscribe((data) => {
    this.genres = data;
  });
}
  GetTheLastestReviews(): void {
    this.reviewService.getTheLastestReviews().subscribe((data) => {
      this.reviewsTitle = data;
    });
  }
}
