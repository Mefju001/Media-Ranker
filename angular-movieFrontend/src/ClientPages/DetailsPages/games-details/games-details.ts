import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, inject, OnDestroy, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReviewService } from '../../../Services/ReviewService';
import { FormsModule } from '@angular/forms';
import { GameService } from '../../../Services/GameService';
import { GameResponse } from '../../../Data/Response/GameResponse';
import { catchError, of, Subscription, switchMap } from 'rxjs';
import { ReviewRequest } from '../../../Data/Request/ReviewRequest';

@Component({
  selector: 'app-games-details',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './games-details.html',
  styleUrl: './games-details.css',
})
export class GamesDetails implements OnInit, OnDestroy {
  readonly stars = [1,2,3,4,5,6,7,8,9,10];
  private route = inject(ActivatedRoute);
  private gameService = inject(GameService);
  private reviewService = inject(ReviewService);
  private platformId = inject(PLATFORM_ID);
  private routeSub?: Subscription;  
  game: GameResponse | null = null;

  isLoading =signal(true);
  showReviewForm = signal(false);
  isLoggedIn = signal(false);

  newReview:ReviewRequest = {
    Rating: 10,
    Comment: ''
};

ngOnInit(): void{
     if (isPlatformBrowser(this.platformId)) {
     const state = sessionStorage.getItem("isLoggedIn");
     this.isLoggedIn.set(state === "true");
    }
     this.routeSub = this.route.paramMap.pipe(
      switchMap(params => {
      const idString = params.get('id');

      if (!idString) {
        this.isLoading.set(false);
        return of(null);
      }
      this.isLoading.set(true);
      return this.gameService.getGameById(idString).pipe(
        catchError(() => {
          return of(null);
        })
      );
    })
  ).subscribe({
    next: (data) => {
      this.game = data;
      this.isLoading.set(false);
    }
    });
}
ngOnDestroy():void{
  this.routeSub?.unsubscribe();
}
toggleReviewForm() {
  this.showReviewForm.set(!this.showReviewForm());
}
submitReview() {
  if (!this.game) {
    alert('Nie można dodać recenzji. Brak danych gry.');
    return;
  }
  this.reviewService.addReview(this.game.id, this.newReview).subscribe({
    next: (savedReview) => {
      this.game!.reviews = [savedReview, ...(this.game!.reviews || [])];
      this.showReviewForm.set(false);
      this.newReview = { Rating: 10, Comment: '' };
    },
    error: (err) => alert('Błąd podczas dodawania recenzji.')
  });
}

}
