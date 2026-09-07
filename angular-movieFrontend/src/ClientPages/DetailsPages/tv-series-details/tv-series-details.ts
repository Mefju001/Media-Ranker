import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReviewService } from '../../../Services/ReviewService';
import { FormsModule } from '@angular/forms';
import { TvSeriesService } from '../../../Services/TvSeriesService';
import { TvSeriesResponse } from '../../../Data/Response/TvSeriesResponse';
import { ReviewRequest } from '../../../Data/Request/ReviewRequest';
import { catchError, of, Subscription, switchMap } from 'rxjs';
import { AuthService } from '../../../Services/AuthService';

@Component({
  selector: 'app-tv-series-details',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './tv-series-details.html',
  styleUrl: './tv-series-details.css',
})
export class TvSeriesDetails implements OnInit, OnDestroy {
  tvSeries: TvSeriesResponse | null = null;
  
  newReview:ReviewRequest = {
    Rating: 10,
    Comment: ''
  };

  readonly stars = [1,2,3,4,5,6,7,8,9,10];
  private route = inject(ActivatedRoute);
  private tvSeriesService = inject(TvSeriesService);
  private reviewService = inject(ReviewService);
  private authService = inject(AuthService);
  private routeSub?: Subscription;  

  
  isLoading = signal(true);
  showReviewForm = signal(false);
  readonly isLoggedIn = this.authService.isLoggedIn;

  ngOnInit(): void{
     this.routeSub = this.route.paramMap.pipe(
      switchMap(params => {
      const idString = params.get('id');

      if (!idString) {
        this.tvSeries = null;
        this.isLoading.set(false);
        return of(null);
      }

      this.isLoading.set(true);
      return this.tvSeriesService.getTvSeriesById(idString).pipe(
        catchError(() => {
          return of(null);
        })
        );
      })
    ).subscribe({
        next: data => {
          this.tvSeries = data;
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
    if (!this.tvSeries) {
        alert('Nie można dodać recenzji. Brak danych serialu.');
        return;
    }
    this.reviewService.addReview(this.tvSeries.id, this.newReview).subscribe({
      next: (savedReview) => {
        this.tvSeries!.reviews = [savedReview, ...(this.tvSeries!.reviews ?? [])];
        this.showReviewForm.set(false);
        this.newReview = { Rating: 10, Comment: '' };
      },
      error: (err) => alert('Błąd podczas dodawania recenzji.')
    });
  }
}
