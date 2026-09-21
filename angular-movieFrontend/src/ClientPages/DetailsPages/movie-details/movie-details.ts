import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MovieService } from '../../../Services/MovieService';
import { ReviewService } from '../../../Services/ReviewService';
import { MovieResponse } from '../../../Data/Response/MovieResponse';
import { ReviewRequest } from '../../../Data/Request/ReviewRequest';
import { catchError, of, Subscription, switchMap } from 'rxjs';
import { AuthService } from '../../../Services/AuthService';
import { ETypeInteractions } from '../Data/ETypeInteractions';
import { ERatingVote } from '../Data/ERatingVote';
import { UserInteractionService } from '../../../Services/UserInteractionsService';
@Component({
  standalone: true,
  selector: 'app-movie-details',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './movie-details.html',
  styleUrl: './movie-details.css',
})
export class MovieDetails implements OnInit, OnDestroy {
[x: string]: any;
  readonly stars = [1,2,3,4,5,6,7,8,9,10];
  readonly ETypeInteractions = ETypeInteractions;
  readonly ERatingVote = ERatingVote;
  private route = inject(ActivatedRoute);
  private movieService = inject(MovieService);
  private reviewService = inject(ReviewService);
  private authService = inject(AuthService);
  private userInteractionService = inject(UserInteractionService);
  private routeSub?: Subscription;  

  movie: MovieResponse|null = null;
  currentStatus = signal<ETypeInteractions | null>(null);
  currentVote = signal<ERatingVote | null>(null);
  isLoading = signal(true);
  showReviewForm = signal(false);
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
      return this.movieService.getMovieById(idString).pipe(
        catchError(() =>
        {
          return of(null);
        })
      );
    })
    ).subscribe({
        next: data => {
          this.movie = data;
          this.isLoading.set(false);
        }
    });
  }
  toggleReviewForm() {
    this.showReviewForm.set(!this.showReviewForm);
  }
  ngOnDestroy(): void {
    this.routeSub?.unsubscribe();
  }
  submitReview() {
    if(!this.movie)
    {
      alert('Nie można dodać recenzji. Brak danych gry.');
      return;
    }
    this.reviewService.addReview(this.movie!.id, this.newReview).subscribe({
      next: (savedReview) => {
        this.movie!.reviews = [savedReview, ...(this.movie!.reviews ?? [])];
        this.showReviewForm.set(false);
        this.newReview = { Rating: 10, Comment: '' };
      },
      error: (err) => alert('Błąd podczas dodawania recenzji.')
    });
  }

  toggleVote(vote: ERatingVote): void {
    const newVote = this.currentVote() === vote ? null : vote;
    this.currentVote.set(newVote);
    
    this.addUserInteraction(this.movie!.id, null, newVote);
  }

  onStatusChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    const newStatus = select.value === 'null' ? null : (select.value as ETypeInteractions);
    this.currentStatus.set(newStatus);

    this.addUserInteraction(this.movie!.id, newStatus, null);
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
