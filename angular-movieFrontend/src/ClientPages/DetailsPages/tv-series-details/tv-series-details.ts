import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReviewService } from '../../../Services/ReviewService';
import { FormsModule } from '@angular/forms';
import { TvSeriesService } from '../../../Services/TvSeriesService';

@Component({
  selector: 'app-tv-series-details',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './tv-series-details.html',
  styleUrl: './tv-series-details.css',
})
export class TvSeriesDetails implements OnInit {
  tvSeries: any;
  tvSeriesId:string = '';
  isLoading: boolean = true;
  newReview = {
  Rating: 10,
  Comment: ''
};
showReviewForm = false;
isLoggedIn = false;
  constructor(
    private route: ActivatedRoute,
    private tvSeriesService: TvSeriesService,
    private reviewService: ReviewService,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ){}
  ngOnInit(): void{
     if (isPlatformBrowser(this.platformId)) {
    const state = sessionStorage.getItem("isLoggedIn");
    this.isLoggedIn = state === "true";
    }
     this.route.paramMap.subscribe(params => {
      const idString = params.get('id');

      if (!idString) {
        this.tvSeries = null;
        this.isLoading = false;
        this.cdr.detectChanges();
        return;
      }

      this.isLoading = true;
      this.tvSeriesService.getTvSeriesById(idString).subscribe({
        next: data => {
          this.tvSeries = data;
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.tvSeries = null;
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
    });
  }
  toggleReviewForm() {
  this.showReviewForm = !this.showReviewForm;
}

submitReview() {
  const reviewData = {
    ...this.newReview,
  };

  this.reviewService.addReview(this.tvSeries.id, reviewData).subscribe({
    next: (savedReview) => {
      this.tvSeries.reviews.unshift(savedReview);
      this.showReviewForm = false;
      this.newReview = { Rating: 10, Comment: '' };
    },
    error: (err) => alert('Błąd podczas dodawania recenzji.')
  });
}

}
