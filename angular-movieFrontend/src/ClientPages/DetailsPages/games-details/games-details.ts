import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReviewService } from '../../../Services/ReviewService';
import { FormsModule } from '@angular/forms';
import { GameService } from '../../../Services/GameService';
import { GameResponse } from '../../../Data/Response/GameResponse';
import { catchError, of, Subscription, switchMap } from 'rxjs';
import { ReviewRequest } from '../../../Data/Request/ReviewRequest';
import { AuthService } from '../../../Services/AuthService';
import { ERatingVote } from '../Data/ERatingVote';
import { ETypeInteractions } from '../Data/ETypeInteractions';
import { UserInteractionService } from '../../../Services/UserInteractionsService';

@Component({
  selector: 'app-games-details',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './games-details.html',
  styleUrl: './games-details.css',
})
export class GamesDetails implements OnInit, OnDestroy {
  readonly stars = [1,2,3,4,5,6,7,8,9,10];
  readonly ETypeInteractions = ETypeInteractions;
  readonly ERatingVote = ERatingVote;
  private route = inject(ActivatedRoute);
  private gameService = inject(GameService);
  private reviewService = inject(ReviewService);
  private authService = inject(AuthService);
  private userInteractionService = inject(UserInteractionService);
  private routeSub?: Subscription;  
  game: GameResponse | null = null;

  isLoading =signal(true);
  showReviewForm = signal(false);
  currentStatus = signal<ETypeInteractions | null>(null);
  currentVote = signal<ERatingVote | null>(null);
  readonly isLoggedIn = this.authService.isLoggedIn;

  newReview:ReviewRequest = {
    Rating: 10,
    Comment: ''
};

ngOnInit(): void{
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
  this.showReviewForm.update(val=>!val);
}
submitReview() {
  if (!this.game) {
    alert('Nie można dodać recenzji. Brak danych gry.');
    return;
  }
  this.reviewService.addReview(this.game.id, this.newReview).subscribe({
    next: (savedReview) => {
      this.game!.reviews = [savedReview, ...(this.game!.reviews ?? [])];
      this.showReviewForm.set(false);
      this.newReview = { Rating: 10, Comment: '' };
    },
    error: (err) => alert('Błąd podczas dodawania recenzji.')
  });
}
  toggleVote(vote: ERatingVote): void {
    const newVote = this.currentVote() === vote ? null : vote;
    this.currentVote.set(newVote);
    
    this.addUserInteraction(this.game!.id, null, newVote);
  }

  onStatusChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    const newStatus = select.value === 'null' ? null : (select.value as ETypeInteractions);
    this.currentStatus.set(newStatus);

    this.addUserInteraction(this.game!.id, newStatus, null);
  }
  addUserInteraction(mediaId: string, typeInteractions?: ETypeInteractions|null, ratingVote?: ERatingVote|null): void 
  {
    this.userInteractionService.addUserInteraction(mediaId, typeInteractions, ratingVote).subscribe({
      next: (response) => {
        console.log('Interakcja użytkownika dodana:', response);
      },
      error: (error) => {
        console.error('Błąd podczas dodawania interakcji użytkownika:', error);
      }
    });
  }
}
