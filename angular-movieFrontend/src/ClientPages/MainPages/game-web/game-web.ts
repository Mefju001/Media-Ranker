import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { GenreResponse } from '../../../Data/Response/GenreResponse';
import { ReviewService } from '../../../Services/ReviewService';
import { RouterLink } from '@angular/router';
import { GameService } from '../../../Services/GameService';
import { GameResponse } from '../../../Data/Response/GameResponse';
import { GameQuery } from './GameQuery';

@Component({
  selector: 'app-game-web',
  imports: [RouterLink,ReactiveFormsModule],
  templateUrl: './game-web.html',
  styleUrl: './game-web.css',
})
export class GameWeb implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly gameService = inject(GameService)
  private readonly reviewService = inject(ReviewService);
  readonly filterForm = this.fb.group({
      TitleSearch: [null],
      MinRating: [null],
      Platform: [null],
      Developer: [null],
      ReleaseYear: [null],
      genreName: [null],
      
      SortByField: [null as { sortBy: string; isDescending: boolean } | null]
    });
  games: GameResponse[] = [];
  genres: GenreResponse[] = [];
  reviewsTitle: string[] = [];
  platforms: string[] = [];
  sortFields = [
    { name: 'Tytuł (A-Z)', sortBy: 'Title', isDescending: false }, 
    { name: 'Ocena (najniższa)', sortBy: 'average', isDescending: false }, 
    { name: 'Rok Wydania (najstarsze)', sortBy: 'releaseDate', isDescending: false },
    { name: 'Tytuł (Z-A)', sortBy: 'Title', isDescending: true },
    { name: 'Ocena (najwyższa)', sortBy: 'average', isDescending: true },
    { name: 'Rok Wydania (najnowsze)', sortBy: 'releaseDate', isDescending: true },
    ];

ngOnInit(): void {
    this.loadGames();
    this.loadGenres();
    this.loadPlatforms();
    this.GetTheLastestReviews();
  }
onFilter(): void {
    const rawValue = this.filterForm.getRawValue();
    const selectedSortField = rawValue.SortByField;
    const gameQuery: GameQuery = {
      TitleSearch: rawValue.TitleSearch,
      MinRating: rawValue.MinRating,
      Platform: rawValue.Platform,
      Developer: rawValue.Developer,
      ReleaseDate: rawValue.ReleaseYear,
      genreName: rawValue.genreName,
      
      SortByField: selectedSortField ? selectedSortField.sortBy : null,
      IsDescending: selectedSortField ? selectedSortField.isDescending : false
    };
    this.loadGamesByFilter(gameQuery);
  }
onReset(): void {
    this.filterForm.reset();
    this.loadGames();
  }
loadGames(): void {
    this.gameService.getGames().subscribe((data) => {
      this.games = data;
      console.log('Załadowano gry:', data);
    });
  }
loadGamesByFilter(query: GameQuery): void {
    this.gameService.getGamesByFilter(query).subscribe({
        next: (data) => {
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
