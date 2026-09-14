import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { RecommendationService } from '../../Services/RecommendationService';

export type RecommendationType = 'All' | 'Movie' | 'TvShow' | 'Game';

@Component({
  selector: 'app-user-recommendations',
  imports: [RouterLink],
  templateUrl: './user-recommendations.html',
  styleUrl: './user-recommendations.css',
})
export class UserRecommendations implements OnInit   {
  private readonly recommendationService = inject(RecommendationService);
  recommendations = signal<any[]>([]);
  selectedType = signal<RecommendationType>('All');
  isLoading = signal<boolean>(true);
  ngOnInit(): void {
    this.isLoading.set(true);
    this.fetchRecommendations();
  }
  setCategory(category: RecommendationType) {
    this.selectedType.set(category);
    this.refreshRecommendations();
    console.log(`Category set to: ${category}`);
  }
  changeRoute(type:string, id:string): string {
    if (type === 'movie') {
      return `/movie/${id}`;
    } else if (type === 'tv') {
      return `/tvSeries/${id}`;
    } else if (type === 'game') {
      return `/game/${id}`;
    } else {
      return '';
    }
  }
  refreshRecommendations() {
    this.isLoading.set(true);
    this.fetchRecommendations();
  }
  private fetchRecommendations() {
    this.recommendationService.getRecommendations(this.selectedType()).subscribe({
      next:(data) => {
        this.recommendations.set(data);
        this.isLoading.set(false);
      },
      error:(error) => {
        console.error('Error fetching recommendations:', error);
        this.isLoading.set(false);
      }}
    );
  }
}
